$('form').on('submit', function (e) {
    let createdAt = moment($('#datetimepicker1 input').val(), "DD/MM/YYYY");
    let expiresAt = moment($('#datetimepicker2 input').val(), "DD/MM/YYYY");

    if (!expiresAt.isAfter(createdAt)) {
        e.preventDefault();
        Swal.fire({
            icon: 'error',
            title: 'Expire date would be greater than Creation date',
            showConfirmButton: false,
            timer: 1500
        });
    }
    
});