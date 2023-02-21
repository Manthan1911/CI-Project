// -------------------------------------  Filter-Mission-sidebar  ---------------------------------

const filterMissionSidebar = document.getElementById("filter-mission-sidebar");
const filterMissionSidebarOpenBtn = document.getElementById(
  "filter-mission-sidebar-open-btn"
);

const filterMissionSidebarCloseBtn = document.getElementById(
  "filter-mission-sidebar-close-btn"
);

function toggleFilterMissionSidebarAutomatically(x) {
  if (x.matches) {
    filterMissionSidebar.setAttribute("style", "display:none !important");
  }
}

var x = window.matchMedia("(min-width: 768px)");
toggleFilterMissionSidebarAutomatically(x);
y.addListener(toggleFilterMissionSidebarAutomatically);

filterMissionSidebarOpenBtn.addEventListener("click", (e) => {
  filterMissionSidebar.style.display = "block";
});

filterMissionSidebarCloseBtn.addEventListener("click", (e) => {
  filterMissionSidebar.style.display = "none";
});
