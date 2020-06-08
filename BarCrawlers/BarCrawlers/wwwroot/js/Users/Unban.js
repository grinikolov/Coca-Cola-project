// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#btnUnban").on('click', function () {
    var other = document.querySelector('[name="Id"]').value;
    $.ajax({
        async: true,
        data: { "id": other, __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val() },
        type: "POST",
        url: '/Magician/User/Unban',
        success: function (recover) {
            console.log("recoverd: " + recover);
            window.location.replace("/Magician/User/Index")
        }
    });
});

