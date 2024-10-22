function formatNumberInput() {
    let input = document.getElementById("price");

    let value = input.value.replace(/,/g, ""); // Loại bỏ dấu phẩy cũ nếu có
    if (!isNaN(value) && value !== "") {
        input.value = parseFloat(value).toLocaleString("en"); // Định dạng số có dấu phẩy
    }
}

// Hàm kiểm tra giá trị hiện tại khi tải trang
function checkInitialValue() {
    let input = document.getElementById("price");
    let value = input.value;

    if (value !== "" && value.length < 15) {
        formatNumberInput(); // Định dạng ngay lập tức nếu không rỗng
    }
}

// Thực hiện kiểm tra khi tải trang
window.onload = function () {
    checkInitialValue(); // Kiểm tra giá trị ban đầu của input
};

//--------------------------------------------------
document
    .getElementById("BtnSubmitUploadFile")
    .addEventListener("click", function (event) {
        let input = document.getElementById("price");

        let valuePrice = input.value.trim();

        var isValid = true;

        if (valuePrice === "" && valuePrice === null) {
            alert("Vui lòng nhập giá cho tài liệu");
            isValid = false;
        }

        // Nếu có lỗi, ngăn không cho form submit
        if (!isValid) {
            event.preventDefault();
        }
    });
//--------------------------------------------------
// Hàm chỉ cho phép nhập ký tự là số
function isNumberKey(evt) {
    var charCode = evt.which ? evt.which : evt.keyCode;
    // Kiểm tra mã phím, chỉ cho phép các phím số từ 0-9
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
