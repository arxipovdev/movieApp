$(function () {
    $('[data-toggle="tooltip"]').tooltip()

    $('.producer-list__btn-remove').on('click', function () {
        const id = $(this).data('id')
        const name = $(this).data('name')
        $('#confirmDeleteProducerModal .modal-body span').text(name)
        $('#confirmDeleteProducerModal a.producer-list__btn-confirm').attr('href', `/Producer/Delete/${id}`)
        $('#confirmDeleteProducerModal').modal('show')
    })
})
