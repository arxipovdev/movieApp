$(function () {
    $('[data-toggle="tooltip"]').tooltip()

    $('.producer-list__btn-remove').on('click', function () {
        const id = $(this).data('id')
        const name = $(this).data('name')
        $('#confirmDeleteProducerModal .modal-body span').text(name)
        $('#confirmDeleteProducerModal a.producer-list__btn-confirm').attr('href', `/Producer/Delete/${id}`)
        $('#confirmDeleteProducerModal').modal('show')
    })

    $('.movie-list__btn-remove').on('click', function () {
        const id = $(this).data('id')
        const name = $(this).data('name')
        $('#confirmDeleteMovieModal .modal-body span').text(name)
        $('#confirmDeleteMovieModal a.movie-list__btn-confirm').attr('href', `/Home/Delete/${id}`)
        $('#confirmDeleteMovieModal').modal('show')
    })
    
    // page /home/edit/{id}
    $('#movie__file-edited')
        .on('change.bs.fileinput', function () {
            const data = new FormData()
            data.append('file', $('#file')[0].files[0])
            $.ajax({
                type: 'POST',
                url: location.origin + '/Home/Upload',
                processData: false,
                contentType: false,
                data,
                success: function (response) {
                    if(response) {
                        $('#Post').val(response.fileName)
                    }
                }, error: function (error) { console.error(error) }
            })
        }).on('clear.bs.fileinput', function () {
        $('.movie__post').val('')
    })
})
