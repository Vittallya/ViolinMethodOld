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
    $("#view_root").empty().append(view)
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
            appendView(view)
            if (onSuccessFunc != null)
                onSuccessFunc(view)

        }
    })
}