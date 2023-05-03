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

(document).ready(function () {
    $(window).scroll(function () {
        // sticky navbar on scroll script
        if (this.scrollY > 20) {
            $('.navbar').addClass("sticky");
        } else {
            $('.navbar').removeClass("sticky");
        }

        // scroll-up button show/hide script
        if (this.scrollY > 500) {
            $('.scroll-up-btn').addClass("show");
        } else {
            $('.scroll-up-btn').removeClass("show");
        }
    });

    // slide-up script
    $('.scroll-up-btn').click(function () {
        $('html').animate({ scrollTop: 0 });
        // removing smooth scroll on slide-up button click
        $('html').css("scrollBehavior", "auto");
    });
}
