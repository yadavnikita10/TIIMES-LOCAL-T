$(document).ready(function(){

if($(window).width() > 991){
	var num = 50; //number of pixels before modifying styles
	$(window).bind('scroll', function () {
	    if ($(window).scrollTop() > num) {
	        $('.tuv-navbar').addClass('nav-position-fixed');
	        $('.logo-header-main').addClass('logo-header-hide');
	    } else {
	        $('.tuv-navbar').removeClass('nav-position-fixed');
	        $('.logo-header-main').removeClass('logo-header-hide');
	    }
	});
}

});