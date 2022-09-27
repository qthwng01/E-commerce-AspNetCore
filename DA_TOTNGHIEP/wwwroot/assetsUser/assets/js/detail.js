
//Swiper JS
var swiper = new Swiper(".mySwiper", {
  autoplay: {
    delay: 2500,
    disableOnInteraction: false,
  },
  loop: true,
  pagination: {
    el: ".swiper-pagination",
    type: "progressbar",
  },
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },
});


//showMore and hideButton Content
  function myFunction() {
  var x = document.getElementById("showMore");
  var y = document.getElementById("hideButton")
  if (x.style.height === "400px") {
    x.style.height = "";
    y.style.display = "none";
  } else {
    x.style.height = "400px";
    y.style.display = "flex";
  }
}

