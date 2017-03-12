//STYLING based on window size on load and resize

$(window).on("load", function () {
    if (document.documentElement.clientWidth >= 992) {
        // Move paragraphs outside of colored blocks on large screen sizes
        //$('.indexBlocksContainer').removeClass('col-sm-6').addClass('col-lg-1');
        $('.indexBlocksContainer').removeClass('col-sm-6');
        $('.indexBlocks').each(function (index) {
            $(this).children('p').appendTo($(this).parent());
        });

        $('#myAcctText').show();

    };


});

$(window).resize(function () {
    // Same as above on window resize
    if (document.documentElement.clientWidth >= 992) {
        //$('.indexBlocksContainer').removeClass('col-sm-6').addClass('col-lg-1');
        $('.indexBlocksContainer').removeClass('col-sm-6');
        $('.indexBlocks').each(function (index) {
            $(this).children('p').appendTo($(this).parent());
        });
        $('#myAcctText').show();
    }
    // Return to original settings on window size reduction
    else {
        $('.indexBlocksContainer').addClass('col-sm-6').removeClass('col-lg-1');
        $('.indexBlocksContainer').each(function (index) {
            $(this).children('p').appendTo($(this).children('.indexBlocks'));
        });
        $('#secondaryNavbar').addClass('navbar-fixed-bottom');
        $('#myAcctText').hide();
    }


});

$(window).on("load", function () {
    // CAROUSEL

        $('.mySlickCarousel').slick({
            autoplay: true,
            pauseOnFocus: false,
            pauseOnHover: false,
            dots: false,
            arrows: false,
            infinite: true,
            speed: 500,
            fade: true,
            cssEase: 'linear'
        });
    


        $('#largeNavbarBox').animate({
        marginLeft: "0px"
    }, 1500);
        

    // SECONDARY NAVIGATION on M,L,XL
    $('#largeNavbarBox').mouseenter(function () {
        $(this).animate({
            height: "auto",
        }, 300);
        
        $('.largeNavbarItems').each(function (index) {
            $(this).delay(300 * (index)).removeAttr('style').animate({
                opacity: 1
            }, 700);
        });    

    }); //end mouseenter 

    $('#largeNavbarBox').mouseleave(function () {
        $('.largeNavbarItems').attr('style', 'display: none');

    }); // end mouseleave 

    //$('#largeNavbarBox').mouseleave(function () {
    //    $('.largeNavbarItems').each(function (index) {
    //        $(this).delay(200 * (index)).css('style', 'display: none').animate({
    //            opacity: 0
    //        }, 400, function () {
    //            $('#largeNavbarBox').delay(500).animate({
    //                height: "80px"
    //            }, 300);
    //        })
    //    })

    //}); // end mouseleave 
}); //end on load




/*Element Fade-in*/
/* TODO: This gets a little wonky if you scroll up and down quickly. Why? */
$(window).on("load", function () {
    $(window).scroll(function () {
        var windowBottom = $(this).scrollTop() + $(this).innerHeight();
        $(".indexBlocksContainer").each(function () {
            /* Check the location of each desired element */
            var objectBottom = $(this).offset().top + $(this).outerHeight();
            var percentBottom = (objectBottom * .85);

            /* If the element is completely within bounds of the window, fade it in */
            if (percentBottom < windowBottom) { //object comes into view (scrolling down)
                if ($(this).css("opacity") == 0) { $(this).stop().fadeTo(750, 1); }
            } else { //object goes out of view (scrolling up)
                if ($(this).css("opacity") == 1) { $(this).stop().fadeTo(750, 0); }
            }
        });
    }).scroll(); //invoke scroll-handler on page-load
});
            

