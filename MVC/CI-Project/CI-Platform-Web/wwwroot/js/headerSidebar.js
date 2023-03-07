// -------------------------------------  Header-sidebar  ---------------------------------
const headerSidebar = document.getElementById("header-sidebar");
const headerSidebarCloseBtn = document.getElementById("header-sidebar-close");
const headerSidebarOpenBtn = document.getElementById("header-sidebar-open-btn");

function toggleHeaderSidebarAutomatically(y) {
  if (y.matches) {
    headerSidebar.setAttribute("style", "display:none !important");
  } else {
    headerSidebar.setAttribute("style", "display:flex !important");
  }
}

var y = window.matchMedia("(max-width: 576px)");
toggleHeaderSidebarAutomatically(y);
y.addListener(toggleHeaderSidebarAutomatically);

headerSidebarOpenBtn.addEventListener("click", (e) => {
  headerSidebar.style.display = "block";
});

headerSidebarCloseBtn.addEventListener("click", (e) => {
  console.log(headerSidebar.style.display);
  headerSidebar.style.display = "none";
  console.log(headerSidebar.style.display);
});
