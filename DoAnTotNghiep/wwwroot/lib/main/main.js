// BROWSER
function openCity(evt, cityName) {
    // Declare all variables
    var i, tabcontent, tablinks;
  
    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
      tabcontent[i].style.display = "none";
    }
  
    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
      tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
  
    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
  }
// END BROWSER
// 
var mybutton = document.getElementById("myBtn");
var myheader = document.getElementById("header-m");
var mypire = document.getElementById("price-cart ");
//header-m
// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function() {scrollFunction()};

function scrollFunction() {
  if ((document.body.scrollTop > 20 || document.documentElement.scrollTop > 20)) {
    mybutton.style.display = "block";
 
  } else {
    mybutton.style.display = "none";
   
  }
  if ((document.body.scrollTop > 700   || document.documentElement.scrollTop > 700)) {
    mypire.style.display = "none";
 
  } else {
    mypire.style.display = "block";
   
  }
  
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
  document.body.scrollTop = 0;
  document.documentElement.scrollTop = 0;
}
// STICKY
$(document).ready(function(){
  $('.js--section-today').waypoint(function(direction){
    if(direction == "down")
    {
      $('nav').addClass('sticky');
    }
    else
    {
      $('nav').removeClass('sticky');

    }
    
  }),
  {
    offset: '5%;'
  },



//  $('.js-scroll-to-regisster').click(function(){
//    $('html,body').animate({scrollTop: $('.js-section-SignIn').offset().top},1000);
//  })

 $(function() {
  $('a[href*=#]:not([href=#])').click(function() {
    if (location.pathname.replace(/^\//,'') == this.pathname.replace(/^\//,'') && location.hostname == this.hostname) {
      var target = $(this.hash);
      target = target.length ? target : $('[name=' + this.hash.slice(1) +']');
      if (target.length) {
        $('html,body').animate({
          scrollTop: target.offset().top
        }, 1000);
        return false;
      }
    }
  });
});

$('.js--wp-1').waypoint(function(direction) {
  $('.js--wp-1').addClass('animated animate__fadeIn');
}, {
  offset: '50%'
});
$('.js--wp-2').waypoint(function(direction) {
  $('.js--wp-2').addClass('animated animate__flipInX');
}, {
  offset: '50%'
});
$('.js--wp-3').waypoint(function(direction) {
  $('.js--wp-3').addClass('animated animate__slideInUp');
}, {
  offset: '50%'
});
$('.js--wp-5').waypoint(function(direction) {
  $('.js--wp-5').addClass('animated animate__zoomIn');
}, {
  offset: '50%'
});
$('.js--wp-4').waypoint(function(direction) {
  $('.js--wp-4').addClass('animated animate__fadeIn');
}, {
  offset: '50%'
});

})

// END STICKY