// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#btnRecover").on('click', function () {
    $.ajax({
        async: true,
        data: $('#form_recover').serialize(),
        type: "POST",
        url: '/Cocktails/Recover',
        success: function (recover) {
            console.log("recoverd: " + recover);
            window.location.replace("/Cocktails/Index")
            //$('#BarCocktailsContainer').html(partialView);
        }
    });
});

