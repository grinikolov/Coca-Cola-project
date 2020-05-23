// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#btnAdd").on('click', function () {
    $.ajax({
        async: true,
        data: $('#form').serialize(),
        type: "POST",
        url: '/Cocktails/AddCocktailIngredient',
        success: function (partialView) {
            console.log("partialView: " + partialView);
            $('#CocktailIngredientsContainer').html(partialView);
        }
    });
});
