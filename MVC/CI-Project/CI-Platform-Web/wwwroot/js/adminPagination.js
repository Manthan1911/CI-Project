let totalRows, rowsPerPage, totalPage, currentPageSet, totalPageSet, currentPage, pageBtnDisplay = 5;
let paginationContainer;
function createPagination(row = 2) {

    paginationContainer = $('#pagination-container');
    $(paginationContainer).empty();
    currentPage = 1;
    rowsPerPage = row;
    totalRows = $('tbody > tr').length;
    totalPage = Math.ceil(totalRows / rowsPerPage);
    currentPageSet = 1;
    totalPageSet = Math.ceil(totalPage / pageBtnDisplay);

    createPaginationButton();

    handleDisplayTableRow();
    if (totalPageSet > 1) handleButtonDisplayPagination();
}

function createHTMLPageButton() {
    var btnStyle = $('<button>', { 'class': 'btn p-2 px-3 d-flex align-items-center border bg-transparent' });
    return btnStyle;
}

function createPaginationButton() {
    if (totalRows <= rowsPerPage)
        return;
    if (totalPageSet > 1) createPrevButton(createHTMLPageButton());
    createLeftButton(createHTMLPageButton());
    createPageNumberedButton();
    createRightButton(createHTMLPageButton());
    if (totalPageSet > 1) createNextButton(createHTMLPageButton());
}


function createPrevButton(btnPrev) {
    var imgPrev = $('<img>');
    imgPrev.attr('src', '/images/previous.png');
    btnPrev.append(imgPrev);
    paginationContainer.append(btnPrev);
    $(btnPrev).click(() => {
        if (currentPageSet == 1) return;
        $(`[data-page='${currentPage}'`).removeClass('active');
        currentPageSet--;
        handleButtonDisplayPagination();
        handleDisplayTableRow();
    });
}

function handleButtonDisplayPagination() {
    let high = currentPageSet * pageBtnDisplay;
    let low = high - pageBtnDisplay + 1;
    currentPage = low;
    $.each($('[data-page]'), (_, item) => {
        let btnNumber = $(item).data('page');
        if (btnNumber >= low && btnNumber <= high) {
            $(item).removeClass('d-none');
        }
        else {
            $(item).addClass('d-none');
        }
    });
}

function handleDisplayTableRow() {
    let low = (currentPage - 1) * rowsPerPage;
    let high = (currentPage) * rowsPerPage;

    $('tbody>tr').each((index, item) => {
        if (index >= low && index < high)
            $(item).removeClass('d-none');
        else
            $(item).addClass('d-none');
    });

    $(document).scrollTop(0);
    $(`[data-page='${currentPage}'`).addClass('active');
}

function createLeftButton(btnLeft) {
    var imgLeft = $('<img>');
    imgLeft.attr('src', '/images/left.png');
    btnLeft.append(imgLeft);
    paginationContainer.append(btnLeft);
    $(btnLeft).click(() => {
        if (currentPage == 1) return;
        $(`[data-page='${currentPage}'`).removeClass('active');
        currentPage--;
        if (totalPageSet > 1 && currentPage < (currentPageSet * pageBtnDisplay)) {
            currentPageSet--;
            handleButtonDisplayPagination();
        }
        handleDisplayTableRow();
    });
}

function createPageNumberedButton() {

    for (let i = 1; i <= totalPage; i++) {
        let btnPage = createHTMLPageButton();
        btnPage.text(i);
        btnPage.attr('data-page', i);
        paginationContainer.append(btnPage);
        $(btnPage).click(onPageNumberButtonClick);
    }
}

function onPageNumberButtonClick() {

    $(`[data-page='${currentPage}'`).removeClass('active');
    currentPage = $(this).data('page');
    $(`[data-page='${currentPage}'`).addClass('active');
    handleDisplayTableRow();
}

function createRightButton(btnRight) {
    var imgLeft = $('<img>');
    imgLeft.attr('src', '/images/right-arrow1.png');
    btnRight.append(imgLeft);
    paginationContainer.append(btnRight);
    $(btnRight).click(() => {
        if (currentPage == totalPage) return;
        console.log(currentPage);
        $(`[data-page='${currentPage}'`).removeClass('active');
        currentPage++;
        if (totalPageSet > 1 && currentPage > (currentPageSet * pageBtnDisplay)) {
            currentPageSet++;
            handleButtonDisplayPagination();
        }
        handleDisplayTableRow();
    });
}

function createNextButton(btnNext) {
    var imgNext = $('<img>');
    imgNext.attr('src', '/images/next.png');
    btnNext.append(imgNext);
    paginationContainer.append(btnNext);
    $(btnNext).click(() => {
        if (currentPageSet == totalPageSet) return;
        $(`[data-page='${currentPage}'`).removeClass('active');
        currentPageSet++;
        handleButtonDisplayPagination();
        handleDisplayTableRow();
    });
}