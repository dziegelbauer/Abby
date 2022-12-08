var dataTable;
$(document).ready( function () {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/MenuItem",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "25%"},
            { "data": "price", "width": "15%"},
            { "data": "category.name", "width": "15%"},
            { "data": "foodType.name", "width": "15%"},
            { 
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                                <a href="/Admin/MenuItems/Upsert?id=${data}" class="btn btn-success text-white mx-2">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                                <a onclick="Delete('/api/MenuItem/' + ${data})" class="btn btn-danger text-white mx-2">
                                    <i class="bi bi-trash-fill"></i>
                                </a>
                           </div>`
                },
                "width": "25%"
            }
        ],
        "width": "100%"
    });
} );

function Delete(url)
{
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this menu item!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
    .then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if(data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                }
            })
        } else {
            toastr.error(data.message);
        }
    });
}