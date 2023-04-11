const carousalImagesFromSlider = document.querySelectorAll(
    ".carousal-images-from-slider"
);
const carousalPreviewImage = document.getElementById("carousal-preview-image");

const leftArrow = document.getElementById("carouselLeftShiftArrow");
const rightArrow = document.getElementById("carouselRightShiftArrow");
const smallImageDiv = document.querySelector(".small-images-div");
let firstImageWidth = carousalImagesFromSlider[0].clientWidth;

for (let i = 0; i < carousalImagesFromSlider.length; i++) {
    carousalImagesFromSlider[i].addEventListener("click", (e) => {
        e.preventDefault();
        carousalPreviewImage.setAttribute(
            "src",
            carousalImagesFromSlider[i].getAttribute("src")
        );
    });
}

leftArrow.addEventListener("click", () => {
    smallImageDiv.scrollLeft -= firstImageWidth;
});

rightArrow.addEventListener("click", () => {
    smallImageDiv.scrollLeft += firstImageWidth;
});