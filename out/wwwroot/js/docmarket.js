//---------slider show-------------------------------------------

let currentIndex = 0;
const slides = document.querySelectorAll(".itemxemnhieu");
const totalSlides = slides.length;
const sliderWrapper = document.querySelector(".listXemNhieu");
const slideWidth = slides[0].clientWidth;

console.log(totalSlides);

const updateSliderPosition = () => {
    sliderWrapper.style.transform = `translateX(-${
        currentIndex * slideWidth
    }px)`;
};

const showNextSlides = () => {
    currentIndex = (currentIndex + 2) % totalSlides;
    updateSliderPosition();
};

const showPreviousSlides = () => {
    currentIndex = (currentIndex - 2 + totalSlides) % totalSlides;
    updateSliderPosition();
};

document.querySelector(".nav.right").addEventListener("click", showNextSlides);
document
    .querySelector(".nav.left")
    .addEventListener("click", showPreviousSlides);

setInterval(showNextSlides, 5000);

//---------end slider show-------------------------------------------
