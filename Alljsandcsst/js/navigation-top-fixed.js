$(document).ready(function(){


var num = 50; //number of pixels before modifying styles
$(window).bind('scroll', function () {
    if ($(window).scrollTop() > num) {
        $('.innov-navbar').addClass('nav-position-fixed');
    } else {
        $('.innov-navbar').removeClass('nav-position-fixed');
    }
});



});