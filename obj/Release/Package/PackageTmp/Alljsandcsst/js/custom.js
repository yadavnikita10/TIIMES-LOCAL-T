$(document).ready(function(){

    /*initialize tooltip*/
	$('[data-toggle="tooltip"]').tooltip();

    /*Navigation: Hoverable dropdown*/
	if ($(window).width() > 991) {
	    $('ul.nav li.dropdown').hover(function () {
	        $(this).find('> .dropdown-menu').stop(true, true).delay(100).slideDown(300);
	    }, function () {
	        $(this).find('> .dropdown-menu').stop(true, true).delay(100).slideUp(300);
	    });
	    $('ul.nav li.dropdown-submenu').hover(function () {
	        $(this).find('> .dropdown-menu').stop(true, true).delay(100).slideDown(300);
	    }, function () {
	        $(this).find('> .dropdown-menu').stop(true, true).delay(100).slideUp(300);
	    });
	}
	$('ul.nav li.dropdown-submenu').click(function (e) {
	    e.stopPropagation();
	    $(this).find('> .dropdown-menu').slideToggle();
	});

/*Calling Owl Carousel*/
	$('#equity-carousel').owlCarousel({
			items: 4,
			loop: true,
			autoplay: true,
			nav:false, 
			responsiveClass:true,
			responsive:{
				0:{
					items:1,
					nav:true,
					dots:false
				},
				480:{
					items:1,
					nav:true,
					dots:false
				},
				767:{
					items:3,
					nav:true,
					dots:false
				},
				1199:{
					items:3
				},
				1200:{
					items:4
				}
			}
	});
	$('#mutual-funds-carousel').owlCarousel({
			items: 4,
			loop: true,
			autoplay: true,
			nav:false, 
			responsiveClass:true,
			responsive:{
				0:{
					items:1,
					nav:true,
					dots:false
				},
				480:{
					items:1,
					nav:true,
					dots:false
				},
				767:{
					items:3,
					nav:true,
					dots:false
				},
				1199:{
					items:3
				},
				1200:{
					items:4
				}
			}
	});


});

