const gridMissionCards = document.querySelectorAll(".grid-mission-card");
console.log(gridMissionCards)
const listMissionCards = document.querySelectorAll(".list-mission-card");
console.log(listMissionCards)
const paginationNumbers = document.getElementById("pagination-numbers");
const paginatedList = document.getElementById("mission-card-grid-div");
console.log(paginatedList)
const listItems = paginatedList.querySelectorAll(".grid-mission-card");
console.log(listItems)
const doublePrevButton = document.getElementById("double-prev-button");
console.log(doublePrevButton)
const prevButton = document.getElementById("prev-button");
console.log(prevButton)
const nextButton = document.getElementById("next-button");
console.log(nextButton)
const doubleNextButton = document.getElementById("double-next-button");
console.log(doubleNextButton)



const paginationLimit = 3;
const pageCount = Math.ceil(listItems.length / paginationLimit);
let currentPage;


const appendPageNumber = (index) => {
    let pageNumber = `<li id="${index}" class="page-item pagination-number me-2"><a class="page-link text-dark" href="#">${index}</a></li>`;
    paginationNumbers.insertAdjacentHTML("beforeend", pageNumber);
};
const getPaginationNumbers = () => {
    for (let i = 1; i <= pageCount; i++) {
        appendPageNumber(i);
    }
};


const setCurrentPage = (pageNum) => {
    currentPage = pageNum;

    const prevRange = (pageNum - 1) * paginationLimit;
    const currRange = pageNum * paginationLimit;

    listItems.forEach((item, index) => {
        item.classList.add("hidden");
        if (index >= prevRange && index < currRange) {
            item.classList.remove("hidden");
        }
    });
};

const handleActivePageNumber = () => {
    document.querySelectorAll(".pagination-number").forEach((button) => {
        button.classList.remove("active");

        const pageIndex = Number(button.getAttribute("page-index"));
        if (pageIndex == currentPage) {
            button.classList.add("active");
        }
    });
};


window.addEventListener("load", () => {
    getPaginationNumbers();
    setCurrentPage(1);

    handleActivePageNumber();

    let pageNumbersLi = document.querySelectorAll(".pagination-number");

    for (let i = 0; i < pageNumbersLi.length; i++) {
        
        pageNumbersLi[i].addEventListener("click", () => {
            const pageIndex = Number(pageNumbersLi[i].id);
            setCurrentPage(pageIndex);
        });
    }

});



