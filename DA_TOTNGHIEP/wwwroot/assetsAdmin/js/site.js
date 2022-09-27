// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});


showInPopup = (url) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal').modal('show');
            $(".close").click(function () {
                $('#form-modal').modal('hide');
            });
        }
    })
}

showInPopup2 = (url) => {
    $.ajax({
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal').modal('show');
            $(".close").click(function () {
                $('#form-modal').modal('hide');
            });
        }
    })
}



jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                setInterval('refreshPage()', 500);
                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxDelete = form => {
    if (confirm('Bạn muốn xoá record này?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    setInterval('refreshPage()', 500);
                    $('#view-all').html(res.html);
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}

deleteCart = form => {
{
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,

                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
}

    //prevent default form submit event
    return false;
}


/*$(document).ready(function () {
    $(".updatecartitem").click(function (event) {
        event.preventDefault();
        var productid = $(this).attr("data-productid");
        var quantity = $("#quantity-" + productid).val();
        $.ajax({
            type: "POST",
            url: "@Url.RouteUrl("UpdateCart")",
            data: {
                productid: productid,
                quantity: quantity
            },
            success: function (result) {
                window.location.href = "@Url.RouteUrl("Carts")";
            }
        });
    });
});
*/

/*    var path_to_delete;
    var root = location.protocol + "//" + location.host;

    $("#deleteItem").click(function (e) {
        path_to_delete = $(this).data('path');
    $('#myform').attr('action', root + path_to_delete);
        });

*/

function refreshPage() {
    location.reload(true);
}

function copyToClipboard(element) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(element).text()).select();
    document.execCommand("copy");
    $temp.remove();
}