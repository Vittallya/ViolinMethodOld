let item = null
let lastSelected = null
let lastEyeIcon = null
let currPage = null

function initEdit(item1) {
    item = item1
    fillInputFields(item.allPriems)
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
    }
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
}



function onPageClicked(div, pageNum, page) {

    if (lastSelected != null)
        lastSelected.removeClass('pdf_page_selected')

    currPage = pageNum
    lastSelected = div
    div.addClass('pdf_page_selected')
    renderPageToCanvas(page, $("#target-img")[0])
    //onPageChanged(pageNum)
}

function checkBoxToggle(checkBox) {
    var val = $(checkBox).prop('checked')
    if (val)
        $('#file_change_box').removeClass('d-none')
    else
        $('#file_change_box').addClass('d-none')
}

function renderPageToCanvas(page, taregtCanvas) {

    var ctx = taregtCanvas.getContext('2d');
    var viewport = page.getViewport(1);
    taregtCanvas.width = viewport.width;
    taregtCanvas.height = viewport.height;

    page.render({
        canvasContext: ctx,
        viewport: viewport
    });
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
            let div = $(`<div class = "form-group"></div>`).
                append($(`<label class="form-label">${p.group.name}</label>`)).
                append((select = $(`<select id="select_group_${p.group.id}" multiple class = "form-control"></select>`)))
            rootInputs.append(div)
            groups.push(p.group.id)
        }
        else {
            select = rootInputs.find(`#select_group_${p.group.id}`)
        }

        select.append($(`<option class="option_priem" id = "option_priem_${p.id}" value=${p.id} >${p.name}</option>`))
    })
}

function updateSelectListData(selectedPriems) {
    var root = $("#priems_edit_root")
    root.find(".option_priem").each((i, e) => {
        e.selected = false
    })

    selectedPriems.forEach(p => {
        var option = $(`#option_priem_${p.id}`)[0]
        option.selected = true
    })
}

function onSaveClicked(e) {
    
    $("#page_info_json").val(JSON.stringify(pageInfo))
    var form = $("#form")[0]
    var formData = new FormData(form)

    $.ajax({
        type: "POST",
        url: '/admin/note/edit',
        data: formData,
        dataType: 'json',
        cache: false,
        contentType: false,
        processData: false,
        success: function (msg) {
            if (msg.error == '') {
                $("#js-file").hide();
                $('#result').html(msg.success);
            } else {
                $('#result').html(msg.error);
            }
        }
    });
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