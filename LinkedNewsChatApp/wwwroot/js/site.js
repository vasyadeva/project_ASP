// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




function footer() {
    const
        main = document.getElementsByTagName('main')[0],
        footer = document.getElementsByTagName('footer')[0]

    main.style.paddingBottom = footer.clientHeight + 'px'
}

const container = document.querySelector('.container-fluid');
container.classList.add('active');

function scrollFunction() {
    if (document.body.scrollTop >235 || document.documentElement.scrollTop >235) {
        document.getElementById("my_Btn").style.display = "block";
    } else {
        document.getElementById("my_Btn").style.display = "none";
    }
}

// Викликаємо функцію scrollFunction() при кожному скролі сторінки
window.onscroll = function () {
    scrollFunction();
};

// Визначаємо функцію topFunction(), яка буде прокручувати сторінку до верху,
// коли користувач клікає на кнопку
function topFunction() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}

