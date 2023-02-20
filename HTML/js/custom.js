const searchButton = document.getElementById("search-button");
const filterMissionNavbar = document.getElementById("filter-mission-navbar");
let flag = 0;
searchButton.addEventListener("click", (e) => {
  e.preventDefault();
  if (flag % 2 == 0) {
    filterMissionNavbar.setAttribute("style", "display:block !important");
  } else {
    filterMissionNavbar.setAttribute("style", "display:none !important");
  }
  flag++;
});

function myFunction(x) {
  if (x.matches) {
    filterMissionNavbar.setAttribute("style", "display:block !important");
  } else {
    filterMissionNavbar.setAttribute("style", "display:none !important");
  }
}

var x = window.matchMedia("(min-width: 576px)");
myFunction(x);

// ------------------------   toggle sidebar   -----------------------------------
// const sidebarOpenBtn = document.querySelectorAll(".sidebar-open");
// const sidebarCloseBtn = document.getElementById("sidebar-close");
// const sidebar = document.getElementById("sidebar");

// function myFunction2(y) {
//   if (y.matches) {
//     sidebar.setAttribute("style", "display:none !important");
//   }
// }

// var y = window.matchMedia("(min-width: 768px)");
// myFunction2(y);
// y.addListener(myFunction2);

// // let sidebarStatus = "close";

// for (let i = 0; i < sidebarOpenBtn.length; i++) {
//   sidebarOpenBtn[i].addEventListener("click", (e) => {
//     sidebar.style.display = "block";
//   });
// }

// sidebarCloseBtn.addEventListener("click", (e) => {
//   sidebar.style.display = "none";
// });
