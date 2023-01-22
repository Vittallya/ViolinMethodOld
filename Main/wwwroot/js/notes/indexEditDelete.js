
function onDelete(e) {
    e.stopPropagation()

    let id = $(e.currentTarget).attr('data-id')
    let url = "/admin/note/delete/" + id;

    if (confirm("Подвердить удаление?")) {
        modalSetupSpinner()
        $.ajax({
            url: url,
            success: tr => {
                hideModal()
                loadView()
            },
            error: e => {
                modalShowErrors(["Ошибка удаления"])
            }
        })
    }
}

function onEdit(e) {
    e.stopPropagation()

    modalSetupSpinner()
    let id = $(e.currentTarget).attr('data-id')
    let url = window.location.origin + "/admin/note/edit/" + id;

    $.get(url).done(view => {
        placeAjaxView(view)
        hideModal()
    }).fail(err => {
        modalShowErrors(["Ошибка загрузки"])
    })
}
