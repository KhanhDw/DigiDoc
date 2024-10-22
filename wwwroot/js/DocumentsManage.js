// Hàm định dạng số và thêm dấu phẩy cho các thẻ p có class 'format-number'
function formatNumbersInSpecificParagraphs() {
    // Lấy tất cả các thẻ p có class 'format-number'
    let paragraphs = document.querySelectorAll("p.format-number");

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

var btnDels = document.querySelectorAll(".btnDel");
btnDels.forEach((btnDel) => {
    btnDel.addEventListener("click", function () {
        let XacNhan = confirm(
            "Bạn chắc chắn muốn xóa tài liệu này đúng không?"
        );
        if (XacNhan) {
            fetch("/DocumentsManage/RemoveBook", {
                method: "POST",
                body: formData,
            })
                .then((response) => response.text())
                .then((result) => {
                    window.history.back();
                    location.reload();
                    console.log("thanfh coong xpas");
                })
                .catch((error) => {
                    console.error("Error:", error);
                });
        }
    });
});
