function initFilters(root) {  
    root.find("input[type=checkbox]").on('change', e => {

        let checked = e.currentTarget.checked
        let priemId = e.currentTarget.getAttribute("data-priem-id")
        let groupId = e.currentTarget.getAttribute("data-group-id")

        let labelId = priemId + '-title'
        let labelClass = groupId + '-title'

        if (checked) {
            var title = createFilterDisplay(labelId, $(e.currentTarget).next().text(), labelClass,
                () => { $(e.currentTarget).prop('checked', false).change(); });

            $('#filters-place').append(title);
        }
        else {
            $('#' + labelId).remove();

            //$.ajax({ url: "/admin/note/getNotes", data: model, success: onViewGetted })
        }
        currentReload()
    })
}

function getFilterData(collection, model) {
    model.selectedPriems = []
    collection.each((ii, ee) => model.selectedPriems.push(Number(ee.getAttribute("data-priem-id"))))
}


function createFilterDisplay(labelId, text, htmlClass, resetCallBack) {
    var $root = $(`<div id = "${labelId}" class="filter-display ${htmlClass}-label"></div>`);

    var $btn = $(` <button class="border-0 bg-transparent">
                        <svg height="20px" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512">
                            <path d="M310.6 361.4c12.5 12.5 12.5 32.75 0 45.25C304.4 412.9 296.2 416 288 416s-16.38-3.125-22.62-9.375L160 301.3L54.63 406.6C48.38 412.9 40.19 416 32 416S15.63 412.9 9.375 406.6c-12.5-12.5-12.5-32.75 0-45.25l105.4-105.4L9.375 150.6c-12.5-12.5-12.5-32.75 0-45.25s32.75-12.5 45.25 0L160 210.8l105.4-105.4c12.5-12.5 32.75-12.5 45.25 0s12.5 32.75 0 45.25l-105.4 105.4L310.6 361.4z" />
                        </svg>
                    </button>`).on('click', resetCallBack);

    var $span = $('<span></span>').html(text);

    $root.append($span).append($btn);
    return $root;
}

function updateCount() {
    var count = $('.cb_priem:checked').length + ids.length;

    var filterText = $('#filter-count');

    if (count > 0) {
        filterText.text(count);
    }
    else {
        filterText.text('');
    }
}