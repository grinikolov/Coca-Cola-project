// //Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// //for details on configuring this project to bundle and minify static web assets.

// //Write your JavaScript code.

const stars = document.querySelector(".ratings").children;
const ratingValue = document.querySelector("#rating-value");
let index;

//$(function () {
//    $("#_BarPartial").on("load", function () {
//        for (var j = 0; j < $('#vars').data().record; j++) {
//            stars[j].classList.remove("fa-star-o");
//            stars[j].classList.add("fa-star");
//        }
//    });
//});


window.addEventListener('load', (event) => {
    for (var j = 0; j < $('#vars').data().record; j++) {
        stars[j].classList.remove("fa-star-o");
        stars[j].classList.add("fa-star");
    }
});