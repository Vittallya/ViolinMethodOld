
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
