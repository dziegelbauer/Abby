﻿var dataTable;
$(document).ready( function () {
    var url = window.location.search;
    
    if(url.includes("cancelled")) {
        loadList("cancelled");
    }
    else {
        if(url.includes("completed")) {
            loadList("completed");
        }
        else {
            if (url.includes("ready")) {
                loadList("ready");
            } 
            else {
                if (url.includes("inProcess")) {
                    loadList("inProcess");
                }
                else
                {
                    loadList("");
                }
            }
        }
    }    
} );

function loadList(param) {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/order?status=" + param,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "15%"},
            { "data": "pickUpName", "width": "15%"},
            { "data": "applicationUser.email", "width": "25%"},
            { "data": "orderTotal", "width": "15%"},
            { "data": "pickUpTime", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                                <a href="/Admin/Order/OrderDetails?id=${data}" class="btn btn-success text-white mx-2">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                           </div>`
                },
                "width": "15%"
            }
        ],
        "width": "100%"
    });
}