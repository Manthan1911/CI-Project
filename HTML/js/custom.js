const searchButton = document.getElementById("search-button");
const filterMissionNavbar = document.getElementById("filter-mission-navbar");

console.log("custom");
console.log(searchButton);
console.log(filterMissionNavbar);

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
x.addListener(myFunction);
