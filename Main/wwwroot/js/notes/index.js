let model = {
    totalCount: 1,
    takeCount: 0,
    currentPage: 1,
    selectedView: null,
    filters: null
}

function initIndex(curPage, totalItemsCount, take, viewName) {
    jQuery.ajaxSettings.traditional = true;
    model.currentPage = curPage
    model.selectedView = viewName
    model.takeCount = take
    model.totalCount = totalItemsCount
}

function definePagination(curPage, totalPages) {

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

function loadView(data = null, onSuccessFunc = null) {
    if (data == null)
        data = model

    $.get({
        url: "/admin/note/getNotes",
        data: data,
        success: view => {
            var root = appendView(view)

            if (model.selectedView == "TableView") {
                //document.querySelectorAll('.table_sort thead').forEach(tableTH => tableTH.addEventListener('click', () => getSort(event)));
                $(".table_sort thead").each((i, e) => e.addEventListener('click', () => getSort(event)))
            }
            root.find(".bt_delete_note").on('click', onDelete)
            root.find(".bt_edit_note").on('click', onEdit)

            if (onSuccessFunc != null)
                onSuccessFunc(view)

        }
    })
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
    $("#main_item").after($('<div id = "root_edit"></div>').append(content)).hide()
}

function clearAjaxView() {
    $("#main_item").show().next().remove()
}