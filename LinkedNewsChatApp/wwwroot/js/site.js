// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("widget").classList.add("show");
    } else {
        document.getElementById("widget").classList.remove("show");
    }
}

function scrollToTop() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}


function footer() {
    const
        main = document.getElementsByTagName('main')[0],
        footer = document.getElementsByTagName('footer')[0]

    main.style.paddingBottom = footer.clientHeight + 'px'
}

const container = document.querySelector('.container-fluid');
container.classList.add('active');
