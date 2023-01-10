
const editPriem = e => {

    var id = e.currentTarget.getAttribute('data-priem-id')

    $.get({
        url: "/admin/priem/editPriem/" + id, success: form => {

            $("#layout_modal_header").text("Редактировать прием")

            let modal = $("#layout_modal")
            modal.modal('show')

            $("#modal_root").empty().append(form)

            $("#modal_root > form").on('submit', f => {
                f.stopPropagation()
                f.preventDefault()
                formData = new FormData(f.currentTarget)
                $.ajax({
                    type: 'POST',
                    processData: false,
                    contentType: false,
                    url: f.currentTarget.action,
                    data: formData,
                    complete: () => {
                        modal.modal('hide')
                    },
                    success: id => {
                        loadData('/admin/priem/GetPriemsAll', priems => {
                            fillInputFields(priems)
                            updateSelectListData(data[currPage].priems)
                        })
                    }
                })

            })
        }
    })
}

const deletePriem = e => {

    if (confirm("Удалить прием?")) {

        let priemId = e.currentTarget.getAttribute("data-priem-id")

        $.ajax({
            type: 'POST',
            url: "/admin/priem/deletePriem/" + priemId,
            success: () => {
                loadData('/admin/priem/GetPriemsAll', priems => {

                    Object.keys(data).forEach(key => {
                        let pId = Number(priemId)

                        const index = data[key].priems.indexOf(pId);
                        if (index > -1) {
                            data[key].priems.splice(index, 1);
                        }
                    })

                    fillInputFields(priems)
                    onPageChanged(currPage)
                })
            }
        })
    }
}

const editGroup = e => {
    $("#layout_modal_header").text("Редактировать группу")

    var groupId = e.currentTarget.getAttribute("data-group-id")

    $.get({
        url: "/admin/priem/editGroup/" + groupId, success: form => {

            let modal = $("#layout_modal")
            modal.modal('show')

            $("#modal_root").empty().append(form)

            $("#modal_root > form").on('submit', f => {
                f.stopPropagation()
                f.preventDefault()



                formData = new FormData(f.currentTarget)
                $.ajax({
                    type: 'POST',
                    processData: false,
                    contentType: false,
                    url: f.currentTarget.action,
                    data: formData,
                    complete: () => {
                        modal.modal('hide')
                    },
                    success: id => {
                        $("#label_group_" + groupId).text($("#modal_root #Name").val())
                    }
                })

            })
        }
    })
}

const deleteGroup = e => {

    if (confirm("Удалить группу? Удалятся также все связанные с ней приемы.")) {

        var groupId = e.currentTarget.getAttribute("data-group-id")

        $.ajax({
            type: 'POST',
            url: "/admin/priem/deleteGroup/" + groupId,
            success: () => {
                $.get({
                    url: "/admin/priem/GetPriemsAll", success: priems => {
                        let priemsSet = new Set(priems.map(x => x.id))

                        Object.keys(data).forEach(key => {

                            let result = data[key].priems.filter(p => priemsSet.has(p.id))

                            //todo если получится, что массив приемов для страницы пустой?

                            data[key].priems = result
                        })
                    }
                })

                $("#root_group_" + groupId).remove()
            }
        })
    }
}