// -------------------------------------  Filter-Mission-sidebar  ---------------------------------

const filterMissionSidebarOpenBtn = document.getElementById(
  "filter-mission-sidebar-open-btn"
);
const filterMissionSidebarCloseBtn = document.getElementById(
  "filter-mission-sidebar-close-btn"
);
const filterBar = document.getElementById("filter-bar");

filterMissionSidebarOpenBtn.addEventListener("click", (e) => {
  filterBar.style.left = "2vw";
});

filterMissionSidebarCloseBtn.addEventListener("click", (e) => {
  filterBar.style.left = "-50vw";
});

// function toggleFilterMissionSidebarAutomatically(x) {
//   if (x.matches) {
//     filterBar.setAttribute("style", "left:-50vw");
//   }
// }

// var x = window.matchMedia("(min-width: 768px)");
// toggleFilterMissionSidebarAutomatically(x);
// y.addListener(toggleFilterMissionSidebarAutomatically);

// ----------------------------------------------------------------------------
// const filters = document.querySelectorAll(".filter > li");
// for (let currFilter of filters) {
//   currFilter.addEventListener("click", (e) => {
//     let nextSibling = currFilter.nextElementSibling;

//     if (nextSibling.style.display == "block") {
//       nextSibling.style.display = "none";
//     } else {
//       nextSibling.style.display = "block";
//     }
//   });
// }
