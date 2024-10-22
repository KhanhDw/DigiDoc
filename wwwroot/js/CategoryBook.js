function formatNumbersInSpecificParagraphs() {
  // Lấy tất cả các thẻ p có class 'format-number'
  let paragraphs = document.querySelectorAll(".priceShow");

  paragraphs.forEach((paragraph) => {
    let value = paragraph.innerText.replace(/,/g, ""); // Loại bỏ dấu phẩy cũ nếu có

    if (value !== "" && !isNaN(value)) {
      paragraph.innerText = parseFloat(value).toLocaleString("en"); // Định dạng số có dấu phẩy
    }
  });
}

// Kiểm tra và định dạng khi tải trang
window.onload = function () {
  formatNumbersInSpecificParagraphs(); // Định dạng giá trị ban đầu trong các thẻ p có class 'format-number'
};
