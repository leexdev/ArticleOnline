﻿(function () {
    "use strict";
    
    // Dropdown on mouse hover
    $(document).ready(function () {
        function toggleNavbarMethod() {
            if ($(window).width() > 992) {
                $('.navbar .dropdown').on('mouseover', function () {
                    $('.dropdown-toggle', this).trigger('click');
                }).on('mouseout', function () {
                    $('.dropdown-toggle', this).trigger('click').blur();
                });
            } else {
                $('.navbar .dropdown').off('mouseover').off('mouseout');
            }
        }
        toggleNavbarMethod();
        $(window).resize(toggleNavbarMethod);
    });
    
    
    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({scrollTop: 0}, 1500, 'easeInOutExpo');
        return false;
    });


    // Main News carousel
    $(".main-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1500,
        items: 1,
        dots: true,
        loop: true,
        center: true,
    });


    // Tranding carousel
    $(".tranding-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 2000,
        items: 1,
        dots: false,
        loop: true,
        nav : true,
        navText : [
            '<i class="fa fa-angle-left"></i>',
            '<i class="fa fa-angle-right"></i>'
        ]
    });


    // Carousel item 1
    $(".carousel-item-1").owlCarousel({
        autoplay: true,
        smartSpeed: 1500,
        items: 1,
        dots: false,
        loop: true,
        nav : true,
        navText : [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ]
    });

    // Carousel item 2
    $(".carousel-item-2").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        margin: 30,
        dots: false,
        loop: true,
        nav : true,
        navText : [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:2
            }
        }
    });


    // Carousel item 3
    $(".carousel-item-3").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        margin: 30,
        dots: false,
        loop: true,
        nav : true,
        navText : [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:2
            },
            992:{
                items:3
            }
        }
    });
    

    // Carousel item 4
    $(".carousel-item-4").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        margin: 30,
        dots: false,
        loop: true,
        nav : true,
        navText : [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:2
            },
            992:{
                items:3
            },
            1200:{
                items:4
            }
        }
    });
    
})(jQuery);

let cutTextHeader3 = document.querySelectorAll('.cut-text-3');
cutTextHeader3.forEach(cutText => {
  let lineHeight = parseInt(window.getComputedStyle(cutText).lineHeight);
  let height = cutText.clientHeight;
  let lineCount = Math.ceil(height / lineHeight) - 1;
    if (lineCount == 2) {
        cutText.classList.add("pb-12");
    }
    else if (lineCount == 1)
    {
        cutText.classList.add("pb-24");
    }
});

let cutTextHeader4 = document.querySelectorAll('.cut-text-4');
cutTextHeader4.forEach(cutText => {
    let lineHeight = parseInt(window.getComputedStyle(cutText).lineHeight);
    let height = cutText.clientHeight;
    let lineCount = Math.ceil(height / lineHeight) - 1;
    if (lineCount == 2) {
        cutText.classList.add("pb-24");
    }
    else if (lineCount == 1) {
        cutText.classList.add("pb-36");
    }
    else if (lineCount == 0)
        cutText.classList.add("pb-45");
    }
);


let cutTextDescription3 = document.querySelectorAll('.cut-text-3');
cutTextDescription3.forEach(cutText => {
    let lineHeight = parseInt(window.getComputedStyle(cutText).lineHeight);
    let height = cutText.clientHeight;
    let lineCount = Math.ceil(height / lineHeight) - 1;
    if (lineCount == 2) {
        cutText.classList.add("pt-12");
    }
    else if (lineCount == 1) {
        cutText.classList.add("pt-24");
    }
});

let cutTextDescription4 = document.querySelectorAll('.cut-text-4');
cutTextDescription4.forEach(cutText => {
    let lineHeight = parseInt(window.getComputedStyle(cutText).lineHeight);
    let height = cutText.clientHeight;
    let lineCount = Math.ceil(height / lineHeight) - 1;
    if (lineCount == 2) {
        cutText.classList.add("pt-24");
    }
    else if (lineCount == 1) {
        cutText.classList.add("pt-36");
    }
    else if (lineCount == 0)
        cutText.classList.add("pt-45");
}
);

function handleSearch() {
    var searchText = document.getElementById("search-input").value;
  
    if (searchText.trim() === "") {
        alert("Vui lòng nhập từ khóa tìm kiếm trước khi tìm kiếm.");
      return;
    }
  
    localStorage.setItem("searchText", searchText);
  
    window.location.href = "./search.html";
  }
  
  var button = document.getElementById("btn-search");
  button.addEventListener("click", handleSearch);
  

  var searchText = localStorage.getItem("searchText");

if (searchText) {
  var heading = document.getElementById("search-result");
  heading.textContent = "Kết quả tìm kiếm cho \"" + searchText + "\"";
} else {

}

-

    $(document).ready(function () {
        $(".xp-menubar").on('click', function () {
            $("#sidebar").toggleClass('active');
            $("#content").toggleClass('active');
        });

    $('.xp-menubar,.body-overlay').on('click', function () {
        $("#sidebar,.body-overlay").toggleClass('show-nav');
            });

        });


