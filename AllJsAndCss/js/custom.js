$(document).ready(function(){

    /*initialize tooltip*/
	$('[data-toggle="tooltip"]').tooltip();

	/*Navigation: Hoverable dropdown*/
	if($(window).width() > 991){
	$('ul.nav li').hover(function() {
	  $(this).find('> .dropdown-menu').stop(true, true).delay(100).slideDown(300);
	}, function() {
	  $(this).find('> .dropdown-menu').stop(true, true).delay(100).slideUp(300);
	});
	$('ul.nav li.dropdown-submenu').hover(function() {
	  $(this).find('> .dropdown-menu').stop(true, true).delay(100).slideDown(300);
	}, function() {
	  $(this).find('> .dropdown-menu').stop(true, true).delay(100).slideUp(300);
	});
	}
	$('ul.nav li.dropdown-submenu').click(function(e) {
      e.stopPropagation();
      $(this).find('> .dropdown-menu').slideToggle();
	});

	/*Remove animate class from carousel*/
	if ($(window).width() < 768) {
		$('.carousel').removeClass('slide');
	}
	

	
	


});

