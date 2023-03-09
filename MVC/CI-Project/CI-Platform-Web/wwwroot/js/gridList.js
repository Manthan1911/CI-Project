const gridView = document.getElementById("mission-card-grid-div");
const listView = document.getElementById("mission-card-list-div");
const gridBtn = document.getElementById("grid-view");
const listBtn = document.getElementById("list-view"); 

gridBtn.addEventListener("click", () => {
    gridView.setAttribute("style", "display:flex !important");
    listView.setAttribute("style", "display:none !important");
});

listBtn.addEventListener("click", () => {
    gridView.setAttribute("style", "display:none !important");
    listView.setAttribute("style", "display:block !important");
});

window.matchMedia("(max-width:768px)").addListener(() => {
    gridView.setAttribute("style", "display:flex !important");
    listView.setAttribute("style", "display:none !important");
});