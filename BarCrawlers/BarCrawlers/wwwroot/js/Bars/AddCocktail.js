// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#btnAdd").on('click', function () {
    $.ajax({
        async: true,
        data: $('#form_edit').serialize(),
        type: "POST",
        url: '/Bars/AddCocktailToBar',
        success: function (partialView) {
            console.log("partialView: " + partialView);
            $('#BarCocktailsContainer').html(partialView);
        }
    });
});

