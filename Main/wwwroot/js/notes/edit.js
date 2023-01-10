let priemId = null


function initEdit(item1) {
    item = item1
    fillInputFields(item.allPriems)


    data = {}


    if (item.isNew) {
        $("#file_change_box_radio").addClass('d-none')
        $("#file_change_box").removeClass('d-none')
    }
    else {
        loadFile('/notefiles/' + item.id + '/' + item.fileName, (doc, totalPages) => {
            selectPageAsMain(item.showPageNumber)
            doc.getPage(item.showPageNumber).
                then(page => onPageClicked($("#pdf_page_" + item.showPageNumber), item.showPageNumber, page))
            //
        })

        item.pageInfo.forEach(page => {
            data[page.pageNumber] = page
        })
    }

    //updateSelectListData(item.pageInfo.reduce((arr, curr) => arr.concat(curr.priems), []))
}

function onSelectAsMainClicked(e) {
    item.showPageNumber = currPage
    selectPageAsMain(currPage)
}

function loadFile(file, finishedCallback) {
    $("#div_details").removeClass('d-none')
    var rootImg = $("#root_images").empty()

    showFilePdf(file,
        (page, doc, pageNum) => {
            editPageIterator(page, doc, pageNum, rootImg, e => {
                onPageClicked($(e.currentTarget), pageNum, page)
            }, item.pageInfo.findIndex(y => y.pageNumber === pageNum) > -1, pageNum === item.showPageNumber)
        }, finishedCallback)
}

function onFileUplodaded(e) {
    let file = e.files[0];
    let reader = new FileReader()
    reader.readAsArrayBuffer(file);
    item.pageInfo = []
    reader.onload = () => loadFile(reader.result, (doc, total) => {
        if (total > 0) {
            doc.getPage(1).then(page => onPageClicked($("#pdf_page_1"), 1, page))
        }
    })
}

function selectPageAsMain(pageNum) {
    if (lastEyeIcon != null)
        lastEyeIcon.remove()

    lastEyeIcon = getEyeIcon()
    $("#pdf_page_" + pageNum).children('div').append(lastEyeIcon)
    $("#bt_make_page_main").addClass('d-none')
}

function onClearBtnClicked() {
    if (data[currPage] != undefined) {
        delete data[currPage]

        fillDataFromLessNearestPage(currPage)
        var svg = lastSelected.find('svg:nth-child(2)')
        svg.css('visibility', 'hidden')
    }
}

function onPageClicked(div, pageNum, page) {

    if (lastSelected != null)
        lastSelected.removeClass('pdf_page_selected')

    currPage = pageNum
    lastSelected = div
    div.addClass('pdf_page_selected')
    renderPageToCanvas(page, $("#target-img")[0])
    onPageChanged(pageNum)
}

function fillDataFromLessNearestPage(pageNum) {

    let page = 1;

    Object.keys(data).forEach(val => {
        if (val > page && val < pageNum)
            page = val;
    })

    if (page < pageNum)
        updateSelectListData(data[page].priems)
    else
        updateSelectListData()
}

function onPageChanged(pageNum) {

    if (Object.keys(data).length > 0) {
        if (data[pageNum] != undefined) {
            updateSelectListData(data[pageNum].priems)
            $("#bt_clear_page_data").removeClass('d-none')
        }
        else {
            fillDataFromLessNearestPage(pageNum)
            $("#bt_clear_page_data").addClass('d-none')
        }
    }
    if (item.showPageNumber === pageNum) {
        $("#bt_make_page_main").addClass('d-none')
    }
    else {
        $("#bt_make_page_main").removeClass('d-none')
    }


}

function checkBoxToggle(checkBox) {
    var val = $(checkBox).prop('checked')
    if (val)
        $('#file_change_box').removeClass('d-none')
    else
        $('#file_change_box').addClass('d-none')
}


function editPageIterator(page, pdf, pageNum, root, pageOnClick, isPointVis = false, isMain = false) {
    var pageNum = page.pageIndex + 1;
    var div = $(`<div id='pdf_page_${pageNum}' class = 'pdf_page'></div>`).
        on('click', pageOnClick)

    var canvas = $("<canvas class = 'canvasImg'></canvas>")

    var ctx = canvas[0].getContext('2d');
    var viewport = page.getViewport(0.4);
    canvas[0].width = viewport.width;
    canvas[0].height = viewport.height;

    page.render({
        canvasContext: ctx,
        viewport: viewport
    });

    var visibility = isPointVis ? 'visible' : 'hidden'
    var secondDiv = $('<div></div>').append(`<span>${pageNum}</span>`).append(getPointIcon(visibility))

    if (isMain) {
        lastShowedIcon = getEyeIcon()
        secondDiv.append(lastEyeIcon)
    }

    div.append(secondDiv).append(canvas)
    root.append(div)
}

function fillInputFields(allPriems) {
    let groups = []

    var rootInputs = $("#priems_edit_root").empty();

    allPriems.forEach(p => {

        var select = null;

        if (groups.indexOf(p.group.id) === -1) {
            let div = $(`<div contextmenu="menu" id="root_group_${p.group.id}" class = "form-group"></div>`)
                .append($("<div class='d-flex'></div>")
                    .append($(`<label id="label_group_${p.group.id}" class="form-label">${p.group.name}</label>`))
                    .append($(`<input type='button' data-group-id = "${p.group.id}" id="bt_edit_group_${p.group.id}" value = "Ред."/>`).on('click', editGroup))
                    .append($(`<input type='button' data-group-id = "${p.group.id}" id="bt_delete_group_${p.group.id}" value = "Удал."/>`).on('click', deleteGroup)))
                .append((select = $(`<div id="select_group_${p.group.id}" class = "priems_group_root"></div>`)))

            rootInputs.append(div)
            groups.push(p.group.id)
            select.on('change', onSelectListChanged)
        }
        else {
            select = rootInputs.find(`#select_group_${p.group.id}`)
        }

        select
            .append($('<div class="priem_element"></div>')
                .append($('<div class="d-flex"></div>')
                    .append($(`<input data-priem-id="${p.id}" contextmenu="menu" class="option_priem" id = "option_priem_${p.id}" type="checkbox" data-value="${p.id}" value ="${p.id}"/>`))
                    .append($(`<label for="option_priem_${p.id}">${p.name}</label>`)))
                .append($(`<input type='button' value='Ред.' data-priem-id="${p.id}"/>`).on('click', editPriem))
                .append($(`<input type='button' value='Удал.' data-priem-id="${p.id}"/>`).on('click', deletePriem)))
    })

}

function updateSelectListData(priems = null) {

    var root = $("#priems_edit_root")
    root.find("input").each((i, e) => {
        //e.selected = false
        e.checked = false
    })

    if (priems != null) {
        priems.forEach(priem => {
            var option = $(`#option_priem_${priem.id}`)[0]
            option.checked = true
        })
    }
}

function onSelectListChanged(e) {

    var root = $("#priems_edit_root")

    var priems = []

    root.find("input").each((i, e) => {
        if (e.checked)
            priems.push({ id: Number(e.value) })
    })
    if (data[currPage] == undefined)
        data[currPage] = {
            priems: null,
            diffLvl: 0,
            recs: null
        }

    data[currPage].priems = priems

    var svg = lastSelected.find('svg')
    svg.css('visibility', 'visible')

    $("#bt_clear_page_data").removeClass('d-none')
}

function onSaveClicked(e) {

    if (item.showPageNumber == 0) {
        alert("Укажите главную страницу")
        return
    }

    if (Object.keys(data) == 0 || Object.keys(data).every(p => data[p].priems.length == 0)) {
        alert("Хотя бы для одной страницы должен быть указан хотя бы один прием")
        return
    }

    let pageInfo = Object.keys(data).reduce((arr, key) => {
        data[key].pageNumber = Number(key)
        arr.push(data[key])
        return arr
    }, [])

    $("#page_info_json").val(JSON.stringify(pageInfo))
    $("#inp_page_number").val(item.showPageNumber)
    var form = $("#form")[0]
    var formData = new FormData(form)

    $.ajax({
        type: "POST",
        url: '/admin/note/edit',
        data: formData,
        contentType: false,
        processData: false,
        success: () => {
            clearAjaxView()
            loadView()
        }
    });
}


function hideMenu() {
    document.getElementById("contextMenu")
        .style.display = "none"
}


function rightClick(e) {
    e.preventDefault();


    console.log(e)
    priemId = e.currentTarget.getAttribute('data-priem-id')


    if (document.getElementById("contextMenu")
        .style.display == "block")
        hideMenu();
    else {
        var menu = document.getElementById("contextMenu")

        menu.style.display = 'block';
        menu.style.left = e.pageX + "px";
        menu.style.top = e.pageY + "px";
    }
}



function getEyeIcon() {
    return $(`
        <svg height="25" preserveaspectratio="xMidYMid meet" viewbox="0 0 100 100" width="25" x="0" xmlns="http://www.w3.org/2000/svg" y="0">
             <circle class="svg-fill-primary" cx="50" cy="50" r="6.7">
             </circle>
             <path class="svg-fill-primary" d="M50,26.4A33.6,33.6,0,0,0,17.9,50a33.6,33.6,0,0,0,64.2,0A33.6,33.6,0,0,0,50,26.4Zm0,37.1A13.5,13.5,0,1,1,63.5,50,13.6,13.6,0,0,1,50,63.5Z" fill-rule="evenodd">
             </path>
            </svg>`)
}

function getPointIcon(visibility) {
    return $(`<svg style='visibility:${visibility}' viewBox="0 0 60 60" version="1.1" xmlns="http://www.w3.org/2000/svg">
                <circle cx="25" cy="25" r="25"/>
            </svg>`)
}