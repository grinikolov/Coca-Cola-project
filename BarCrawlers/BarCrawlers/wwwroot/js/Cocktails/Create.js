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
//$("#btnAdd").on('click', function () {
//    var ingredientsSelector = $('#ingredients-selector');

//    var select = ingredientsSelector.children('select').first();
//    var ingredient = select.children()[select[0].selectedIndex].innerHTML;
//    var ingredientId = select.children()[select[0].selectedIndex].attributes['value'];
//    var quantity = ingredientsSelector.children('input').first().val();
//    var el = '<div><span value="' + ingredientId + '">' + ingredient + '</span><span>' + quantity + '</span><span class="delete-row">X</span><div>';

//    ingredientsSelector.prepend(el);

//    var delBtn = $('.delete-row').last();

//    delBtn.on('click', function () {
//        var parent = delBtn.parent();
//        parent.remove();
//    });
//});
