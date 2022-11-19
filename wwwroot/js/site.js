$(document).ready(() => {

    $("#btnfillTable").on("click", function () {
        $.ajax({
            url: "/home/TableData",
            success: function (data) {
                $('#myTableId tbody').empty();
                console.log(data);
                for (var i = 0; i < data.length; i++) {
                    $('tbody').append(`<tr> <td>${data[i].noteId}</td> <td>${data[i].title}</td> <td>${data[i].description}</td>   </tr>`);
                }
            },
            error: function (er) {
                console.log(er);
            }
        });

    });

});