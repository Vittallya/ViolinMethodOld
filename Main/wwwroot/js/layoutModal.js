const MODAL_ID = "layout_modal"
const MODAL_BODY_ID = "modal_root"
const MODAL_HEADER_ID = "layout_modal_header"


function modalShowErrors(errors, text = 'Ошибка') {
    $("#"+MODAL_ID).modal('show')
    $("#" + MODAL_HEADER_ID).text(text)
    var ul = $("<ul></ul>")
    errors.forEach(error => {
        ul.append(`<li>${error}</li>`)
    })
    $("#" + MODAL_BODY_ID).empty().append(ul)
}

function modalGetSpinner() {
    return $(`<div class="spinner-border" role="status">
                        <span class="sr-only">Загрузка...</span>
                    </div>`)
}

function modalSetupSpinner() {
    $("#" + MODAL_ID).modal('show')
    $("#" + MODAL_HEADER_ID).text("")
    $("#" + MODAL_BODY_ID).empty().append(modalGetSpinner())
}

function hideModal() {
    $("#" + MODAL_ID).modal('hide')
}