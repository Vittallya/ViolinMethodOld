
let item = null
let currPage = null
let lastSelected = null
let lastEyeIcon = null
let data = {

}

function showFilePdf(fileInfo, pageCallback, finishedCallback) {

    pdfjsLib.getDocument(fileInfo).then((doc) => {
        

        for (let i = 0; i < doc._pdfInfo.numPages; i++) {
            doc.getPage(i + 1).then(page => pageCallback(page, doc, i + 1));
        }
        if (finishedCallback != undefined) {
            finishedCallback(doc, doc._pdfInfo.numPages)
        }
    });
}

function detailsPageIterator(page) {
}

function initDetails(item1) {
    item = item1
    //fillFields(item.allPriems)

    data = {}

    var rootImg = $("#root_images").empty()
    var rootImgSlider = $("#root_images_slider").empty()

    showFilePdf('/notefiles/' + item.id + '/' + item.fileName,
        (page, doc, pageNum) => {
            detailsPageIterator(page, doc, pageNum, rootImg, e => {
                onPageClickedDetails($(e.currentTarget), pageNum, page)
            }, item.pageInfo.findIndex(y => y.pageNumber === pageNum) > -1, pageNum === item.showPageNumber)

        },
        (doc, totalPages) => {
            doc.getPage(item.showPageNumber).
                then(page => onPageClickedDetails($("#pdf_page_" + item.showPageNumber), item.showPageNumber, page))
        })

    item.pageInfo.forEach(page => {
        data[page.pageNumber] = page
    })

    //updateSelectListData(item.pageInfo.reduce((arr, curr) => arr.concat(curr.priems), []))
}

function detailsPageIterator(page, pdf, pageNum, root, pageOnClick, isPointVis = false, isMain = false) {
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

    var secondDiv = $('<div></div>').append(`<span>${pageNum}</span>`)

    div.append(secondDiv).append(canvas)
    root.append(div)
}


function onPageClickedDetails(div, pageNum, page) {

    if (lastSelected != null)
        lastSelected.removeClass('pdf_page_selected')

    currPage = pageNum
    lastSelected = div
    div.addClass('pdf_page_selected')
    renderPageToCanvas(page, $("#target-img")[0])

    var root = $("#priems_root").empty()



    if (data[pageNum] != undefined) {

        var groupsId = Array.from( new Set(data[pageNum].priems.map(x => x.group.id)))
        var groups = data[pageNum].priems.map(x => x.group).filter((g, i) => {

            let index = groupsId.indexOf(g.id)
            if (index > -1) {
                groupsId.splice(index, 1)
                return true
            }
            return false
        })

        groups.forEach(g => {

            var priemsRoot = $("<div class='div_details_hashtags'></div>")
            data[pageNum].priems.filter(x => x.group.id == g.id).forEach(p => {
                priemsRoot.append($('<div class="div_details_hashtag"></div>').text(p.name))
            })

            root.append($('<div class="div_details_section"></div>').append($(`<h5>${g.name}</h5>`)).append(priemsRoot))
        })
    }
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