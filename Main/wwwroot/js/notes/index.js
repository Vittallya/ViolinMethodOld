let model = {
    totalCount: 1,
    takeCount: 0,
    currentPage: 1,
    selectedView: null,
    paginationDefined: false,
    selectedPriems: [],
}

let onDeleteFunc, onEditFunc;
let selectedGroupId;


let filtersLoaded = false
let hasPagination = false

function initIndex(curPage, totalItemsCount, take, viewName) {
    model.currentPage = curPage
    model.selectedView = viewName
    model.takeCount = take
    model.totalCount = totalItemsCount
}

function definePagination(curPage, totalPages, root) {

    root.empty()

    if (totalPages == 1)
        return;

    hasPagination = true
    root.append($('<li class="page-item active"></li>').append(`<span class = "page-link">${curPage}</span>`))
    var i = curPage - 1

    for (; i >= curPage - 1 && i > 0; i--) {
        root.prepend(getLi(i))
    }

    if (i > 1) {
        root.prepend($('<li class="page-item"></li>').append(`<span>...</span>`))
        root.prepend(getLi(1))
    }

    var i = curPage + 1

    for (; i <= curPage + 1 && i <= totalPages; i++) {
        root.append(getLi(i))
    }


    if (i < totalPages) {
        root.append($('<li class="page-item"></li>').append(`<span>...</span>`))
        root.append(getLi(totalPages))
    }
    model.paginationDefined = true
    function getLi(pageNumber) {
        return $('<li class="page-item" ></li>').append($(`<a class="page-link" href="#">${pageNumber}</a>`)).on('click', e => {
            e.stopPropagation()
            model.currentPage = pageNumber
            loadView()
        })
    }
}

function appendView(view) {
    return $("#view_root").empty().append(view)
}

const getSort = ({ target }) => {

    const order = (target.dataset.order = -(target.dataset.order || -1));
    const index = [...target.parentNode.cells].indexOf(target);
    const collator = new Intl.Collator(['en', 'ru'], { numeric: true });
    const comparator = (index, order) => (a, b) => order * collator.compare(
        a.children[index].innerHTML,
        b.children[index].innerHTML
    );

    for (const tBody of target.closest('table').tBodies)
        tBody.append(...[...tBody.rows].sort(comparator(index, order)));

    for (const cell of target.parentNode.cells)
        cell.classList.toggle('sorted', cell === target);
};

function loadView(data = null, onSuccessFunc = null, url = null) {
    jQuery.ajaxSettings.traditional = true;

    if (url == null)
        url = "/admin/note/getNotes"

    if (data == null)
        data = model

    $.get({
        url: url,
        data: data,
        success: view => {
            onViewGetted(view)
            if (onSuccessFunc != null)
                onSuccessFunc(view)
        }
    })
}

function currentReload() {
    var filtersInput = $('.cb_priem:checked:enabled');

    if (filtersInput.length > 0) {
        filters = getFilterData(filtersInput, model)
    }
    else {
        delete model.selectedPriems
    }

    loadView()

    //getTable($('#sel-limit').val(), curPage, filters, curMode)
}


function onViewGetted(view) {
    var root = appendView(view)

    if (model.selectedView == "TableView") {
        //document.querySelectorAll('.table_sort thead').forEach(tableTH => tableTH.addEventListener('click', () => getSort(event)));
        $(".table_sort thead").each((i, e) => e.addEventListener('click', () => getSort(event)))
    }
    root.find(".bt_delete_note").on('click', onDeleteFunc)
    root.find(".bt_edit_note").on('click', onEditFunc)

    //definePagination(model.currentPage, Math.ceil(model.totalCount / model.takeCount), $("#p_root"))
}

function loadFilters(root, data = null) {

    if (data != null && data.id != selectedGroupId || data == null) {
        $.get({
            url: "/admin/note/getfilterview",
            data: data,
            success: view => {

                if (data != null && data.id != undefined)
                    selectedGroupId = data.id;

                $(root).empty().append(view)
                initFilters($(root))
            }
        })
    }
}


function placeAjaxView(content) {

    $("#root_edit").remove()

    $("#main_item").after($('<div id = "root_edit"></div>').append(content)).hide()
}

function clearAjaxView() {
    $("#main_item").show().next().remove()
}