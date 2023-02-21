// -------------------------------------  Header-sidebar  ---------------------------------
const headerSidebar = document.getElementById("header-sidebar");
const headerSidebarCloseBtn = document.getElementById("header-sidebar-close");
const headerSidebarOpenBtn = document.getElementById("header-sidebar-open-btn");

function toggleHeaderSidebarAutomatically(y) {
  if (y.matches) {
    headerSidebar.setAttribute("style", "display:none !important");
  }
}

var y = window.matchMedia("(min-width: 576px)");
toggleHeaderSidebarAutomatically(y);
y.addListener(toggleHeaderSidebarAutomatically);

headerSidebarOpenBtn.addEventListener("click", (e) => {
  headerSidebar.style.display = "block";
});

headerSidebarCloseBtn.addEventListener("click", (e) => {
  headerSidebar.style.display = "none";
});
