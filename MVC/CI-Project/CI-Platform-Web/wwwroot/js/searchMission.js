const searchBar = document.getElementById("search-input");
const missionCards = document.querySelectorAll(".card-to-hide");

let cardsCount = missionCards.length;

searchBar.addEventListener("keyup", () => {
    let searchedValue = searchBar.value.toLowerCase();

    for (let i = 0; i < missionCards.length; i++) {
        var title = missionCards[i].getElementsByClassName("mission-title-to-hide")[0];
        if (title.innerHTML.toLowerCase().indexOf(searchedValue) > -1) {
            missionCards[i].style.display = "";
        }
        else {
            missionCards[i].style.display = "none";
        }
    }
});
