
// var APP = function() {

//     // PATHS
//     // ======================
//     //this.ASSETS_PATH = '../../assets/';
//     this.ASSETS_PATH = './assets/';
//     this.SERVER_PATH = this.ASSETS_PATH + 'demo/server/';

//     // GLOBAL HELPERS
//     // ======================
// 	this.is_touch_device = function() {
//         return !!('ontouchstart' in window) || !!('onmsgesturechange' in window);
// 	};
// };

// var APP = new APP();

// APP UI SETTINGS
// ======================

// APP.UI = {
// 	scrollTop: 0, // Minimal scrolling to show scrollTop button
// };


// PAGE PRELOADING ANIMATION
$(window).on('load', function() {
	setTimeout(function() {
		$('.preloader-backdrop').fadeOut(200);
		$('body').addClass('has-animation');
	},0);
});

// Hide sidebar on small screen
$(window).on('load resize scroll', function () {
    if ($(this).width() < 992) {
        $('body').addClass('sidebar-mini');
    }
});


$(function () {

    // SIDEBAR ACTIVATE METISMENU
    $(".metismenu").metisMenu();

    // Activate Tooltips
    $('[data-toggle="tooltip"]').tooltip();

    // Activate Popovers
    $('[data-toggle="popover"]').popover();

    // Activate slimscroll
    $('.scroller').each(function(){
        $(this).slimScroll({
            height: $(this).attr('data-height'),
            color: $(this).attr('data-color'),
            railOpacity: '0.9',
        });
    });

    // LAYOUT SETTINGS
    // ======================

    // SIDEBAR TOGGLE ACTION
    $('.js-sidebar-toggler').click(function () {
        $('body').toggleClass('sidebar-mini');
    });
    
    // fixed layout
    $('#_fixedlayout').change(function(){
        if( $(this).is(':checked') ) {
           $('body').addClass('fixed-layout');
            $('#sidebar-collapse').slimScroll({
                height: '100%',
                railOpacity: '0.9',
            });
        } else {
            $('#sidebar-collapse').slimScroll({destroy: true}).css({overflow: 'visible', height: 'auto'});
            $('body').removeClass('fixed-layout');
        }
    });

    // fixed navbar
    $('#_fixedNavbar').change(function() {
        if($(this).is(':checked')) $('body').addClass('fixed-navbar');
        else $('body').removeClass('fixed-navbar');
    });
    
    // Boxed layout
    $("[name='layout-style']").change(function(){
        if(+$(this).val()) $('body').addClass('boxed-layout');
        else $('body').removeClass('boxed-layout');
    });


	// BACK TO TOP
	// $(window).scroll(function() {
	// 	if($(this).scrollTop() > APP.UI.scrollTop) $('.to-top').fadeIn();
    //     else $('.to-top').fadeOut();
	// });
	$('.to-top').click(function(e) {
		$("html, body").animate({scrollTop:0},500);
	});



    // PANEL ACTIONS
    // ======================
    $('.fullscreen-link').click(function(){
        if($('body').hasClass('fullscreen-mode')) {
            $('body').removeClass('fullscreen-mode');
            $(this).closest('div.ibox').removeClass('ibox-fullscreen');
            $(window).off('keydown',toggleFullscreen);
        } else {
            $('body').addClass('fullscreen-mode');
            $(this).closest('div.ibox').addClass('ibox-fullscreen');
            $(window).on('keydown', toggleFullscreen);
        }
    });
    function toggleFullscreen(e) {
        // pressing the ESC key - KEY_ESC = 27 
        if(e.which == 27) {
            $('body').removeClass('fullscreen-mode');
            $('.ibox-fullscreen').removeClass('ibox-fullscreen');
            $(window).off('keydown',toggleFullscreen);
        }
    }
    
    // Backdrop functional

    $.fn.backdrop = function() {
	    $(this).toggleClass('shined');
	    $('body').toggleClass('has-backdrop');
        return $(this);
	};

    $('.backdrop').click(closeShined);

    function closeShined() {
        $('body').removeClass('has-backdrop');
        $('.shined').removeClass('shined');
    }

});
