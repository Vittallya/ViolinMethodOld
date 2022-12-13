
let lastDiv = null;

function initPriem() {
    //loadData('/admin/priem/getGroups', onGroupsLoaded, onError)
    loadData('/admin/priem/editPriem', form => {
        $("#test").append(form)
        $("form").on('submit', f => {
            f.stopPropagation()
            f.preventDefault()
            formData = new FormData(f.currentTarget)
            $.ajax({
                type: 'POST',
                processData: false,
                contentType: false,
                url: f.currentTarget.action,
                data: formData,                
            })
        })
    }, onError)
}

function onGroupsLoaded(groups) {
    var root = $("#tabs_div")

    groups.forEach(g => {
        let div = $(`<div id="content-${g.id}"></div>`)

        let onClick = e => {
            if (lastDiv != null)
                lastDiv.hide()
            lastDiv = div
            div.show()
        }

        $("#tab_btns").
            append($(`<input type="radio" name="tab_bt" id="tab_bt_${g.id}"/> `)).
            append($(`<label for="tab_bt_${g.id}" >${g.Name}</label> `))
        root.append(div)
    })
}

function onError(data) {
    alert("Error")
}

function loadData(url, onSuccess, onError) {
    $.get({
        url: url,
        success: onSuccess,
        error: onError
    })
}
