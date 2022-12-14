let model = {
    totalCount: 1,
    takeCount: 0,
    currentPage: 1,
    selectedView: null,
    filters: null,
    paginationDefined: false
}

let filtersLoaded = false

function initIndex(curPage, totalItemsCount, take, viewName) {
    jQuery.ajaxSettings.traditional = true;
    model.currentPage = curPage
    model.selectedView = viewName
    model.takeCount = take
    model.totalCount = totalItemsCount
}

function definePagination(curPage, totalPages, root) {

    root.empty()

    if (totalPages == 1)
        return;

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
    if (url == null)
        url = "/admin/note/getNotes"

    if (data == null)
        data = model

    $.get({
        url: url,
        data: data,
        success: view => {
            var root = appendView(view)

            if (model.selectedView == "TableView") {
                //document.querySelectorAll('.table_sort thead').forEach(tableTH => tableTH.addEventListener('click', () => getSort(event)));
                $(".table_sort thead").each((i, e) => e.addEventListener('click', () => getSort(event)))
            }
            root.find(".bt_delete_note").on('click', onDelete)
            root.find(".bt_edit_note").on('click', onEdit)

            definePagination(model.currentPage, Math.ceil(model.totalCount / model.takeCount), $("#p_root"))

            if (onSuccessFunc != null)
                onSuccessFunc(view)

        }
    })
}

function loadFilters(root) {
    if (!filtersLoaded) {
        $.get({
            url: "/admin/note/getfilterview",
            success: view => {
                $(root).empty().append(view)
            }
        })
        filtersLoaded = true
    }
}

function onDelete(e) {
    e.stopPropagation()

    let id = $(e.currentTarget).attr('data-id')
    let url = "/admin/note/delete/" + id;

    if (confirm("Подвердить удаление?")) {
        $.ajax({
            url: url,
            success: tr => {
                loadView()
            }
        })
    }
}

function onEdit(e) {
    e.stopPropagation()

    //$("#modal_loading").modal('show')
    let id = $(e.currentTarget).attr('data-id')
    let url = window.location.origin + "/admin/note/edit/" + id;

    $.get(url).done(view => {
        placeAjaxView(view)
        //$('#ajax-content-place').empty().append(page)
        //$("#modal_loading").modal('hide')
    }).fail(err => {
        //$("#modal_loading").modal('hide')
        //$("#modal_error").modal('show')
        //$("#modal_error #modal_error_div > ul").empty().append(`<li>Ошибка</li>`)
    })

}

function placeAjaxView(content) {

    $("#root_edit").remove()

    $("#main_item").after($('<div id = "root_edit"></div>').append(content)).hide()
}

function clearAjaxView() {
    $("#main_item").show().next().remove()
}