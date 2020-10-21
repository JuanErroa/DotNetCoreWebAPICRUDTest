$(document).ready(function () {
    jobTable = $('#jobTable').DataTable({
        "language": {
            "sProcessing": "Loading rows...",
            "lengthMenu": "Showing _MENU_ rows per page",
            "zeroRecords": "There is no jobs in the database",
            "info": "Showing page _PAGE_ from _PAGES_ pages",
            "sSearch": "Search:",
            "infoEmpty": "There is no jobs in the database",
            "infoFiltered": "(Filtered _MAX_ from Jobs)",
            "sLoadingRecords": "Loading...",
            "oPaginate": {
                "sFirst": "First",
                "sLast": "Last",
                "sNext": "Next",
                "sPrevious": "Previous"
            }
        },
        "ajax": {
            "url": "/Jobs/GetJobList",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                console.log(data);
                return data;
            },
            "dataSrc": function (json) {
                $.each(json.data, function (key, value) {
                    value.createdAt = moment(value.createdAt).format('DD-MM-YYYY');
                    value.expiresAt = moment(value.expiresAt).format('DD-MM-YYYY');
                });
                return json.data;
            },
        },
        "columns": [
            { "data": "job", "name": "Job" },
            { "data": "jobTitle", "name": "JobTitle" },
            { "data": "description", "name": "Description" },
            { "data": "createdAt", "name": "CreatedAt" },
            { "data": "expiresAt", "name": "ExpiresAt" },
            {
                sortable: false,
                "render": function (full, type, data, meta) {
                    return '<div class="w-100 text-center">' +
                        ' <a href="/Jobs/Edit/' + data.job + '" class="btn btn-outline-warning">Edit<a>' +
                        ' <a href="#" class="btn btn-outline-danger delBtn">Delete<a>' +
                        '</div>';
                }
            }
        ],
        "columnDefs": [
            {
                "targets": 3,
                "className": "text-center",
            },
            {
                "targets": 4,
                "className": "text-center",
            },
            {
                "targets": 5,
                "className": "text-center",
            }
        ],
        "serverSide": "true",
        "processing": "true",
        "order": [0, "asc"],
    });
});


$('#jobTable tbody').on('click', '.delBtn', function () {
    let row = jobTable.row($(this).parent().parent()).data();

    Swal.fire({
        title: 'Confirm',
        text: 'Do you want to remove \n' + row.jobTitle + '?',
        icon: 'error',
        confirmButtonText: 'Cool'
    }).then(function (confirm) {
        if (confirm.isConfirmed) {
            removeJob(row.job, row.jobTitle);
        }
    });
});


function removeJob(jobId, title) {
    $.ajax({
        url: '/Jobs/RemoveJob/' + jobId,
        type: 'post',
        dataType: 'json',
        data: { jobId },
        success: function (data) {

            Swal.fire({
                icon: 'success',
                title: title + ' ' + 'Has been Removed',
                showConfirmButton: false,
                timer: 1500
            });

            jobTable.ajax.reload();
        },
        error: function (data) {
            alert('error');
        }
    });
}