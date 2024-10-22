//---------slider show-------------------------------------------

let currentIndex = 0;
const slides = document.querySelectorAll(".itemxemnhieu");
const totalSlides = slides.length;
const sliderWrapper = document.querySelector(".listXemNhieu");
const slideWidth = slides[0].clientWidth;

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

function formatNumbersInSpecificParagraphsHeader() {
    // Lấy tất cả các thẻ p có class 'format-number'
    let paragraphs = document.querySelectorAll(".priceShow");

    paragraphs.forEach((paragraph) => {
        let value = paragraph.innerText.replace(/,/g, ""); // Loại bỏ dấu phẩy cũ nếu có

        if (value !== "" && !isNaN(value)) {
            paragraph.innerText = parseFloat(value).toLocaleString("en"); // Định dạng số có dấu phẩy
        }
    });
}

//thế này sẽ không bị gi đè
window.addEventListener("load", function () {
    formatNumbersInSpecificParagraphsHeader(); // Định dạng giá trị ban đầu trong các thẻ p có class 'format-number'
});
