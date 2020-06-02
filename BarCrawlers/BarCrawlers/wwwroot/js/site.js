// //Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// //for details on configuring this project to bundle and minify static web assets.

// //Write your JavaScript code.

const stars = document.querySelector(".ratings").children;
const ratingValue = document.querySelector("#rating-value");
//var vars2 = $('#vars').data();
let index;

window.addEventListener('load', (event) => {
    for (var j = 0; j < $('#vars').data().record; j++) {
        stars[j].classList.remove("fa-star-o");
        stars[j].classList.add("fa-star");
    }
});

for (let i = 0; i < stars.length; i++)
{
    stars[i].addEventListener("mouseover", function () {
        for (let j = $('#vars').data().record; j >= i; j--) {
            stars[j].classList.remove("fa-star");
            stars[j].classList.add("fa-star-o");
        }
        for (let j = $('#vars').data().record; j <= i; j++) {
            stars[j].classList.remove("fa-star-o");
            stars[j].classList.add("fa-star");
        }
    });
    stars[i].addEventListener("click", function () {
        ratingValue.value = (i + 1);
        index = i;
        var $vars = $('#vars').data();
        $.ajax({
            async: true,
            type: "POST",
            data: { "id": $vars.id, "userId": $vars.userid, "rating": ratingValue.value},
            url: $vars.url,
        });
    });
    stars[i].addEventListener("mouseout", function () {
        for (let j = 0; j < $('#vars').data().record; j++) {
            stars[j].classList.remove("fa-star-o");
            stars[j].classList.add("fa-star");
        };
        for (let j = $('#vars').data().record; j <= stars.length; j++) {
            stars[j].classList.remove("fa-star");
            stars[j].classList.add("fa-star-o");
        };
    });
}