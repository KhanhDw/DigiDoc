function formatNumbersInSpecificParagraphs() {
    // Lấy tất cả các thẻ p có class 'format-number'
    let paragraphs = document.querySelectorAll(".formatNumber");

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

//------------------------------------------------------
var xacNhan = document.querySelectorAll(".btn-XacNhan");
xacNhan.forEach(function (btn) {
    btn.addEventListener("click", function () {
        var id = this.getAttribute("data_id");

        fetch("/BillManage/XacNhanThanhToan", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                IdLichSuNap: id,
            }),
        })
            .then((response) => response.json())
            .then((result) => {
                if (result.success) {
                    window.history.back();
                    location.reload();
                    alert("Cập nhật thành công!!");
                    window.location.replace("/BillManage");
                } else {
                    alert("success: false::: " + result);
                }
            })
            .catch((error) => {
                console.error("Error:", error);
                alert("Có lỗi xảy ra. Vui lòng thử lại: " + error);
            });
    });
});

// --------------------- từ chối thanh toán------------------------------------------
var Tuchoi = document.querySelectorAll(".btn-TuChoi");
Tuchoi.forEach(function (btn) {
    btn.addEventListener("click", function () {
        var id = this.getAttribute("data_id");

        fetch("/BillManage/TuChoiThanhToan", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                IdLichSuNap: id,
            }),
        })
            .then((response) => response.json())
            .then((result) => {
                if (result.success) {
                    window.history.back();
                    location.reload();
                    alert("Cập nhật thành công!!");
                    window.location.replace("/BillManage");
                } else {
                    alert("success: false::: " + result);
                }
            })
            .catch((error) => {
                console.error("Error:", error);
                alert("Có lỗi xảy ra. Vui lòng thử lại: " + error);
            });
    });
});

//------------------------- xóa -----------------
var Btndelete = document.querySelectorAll(".btn-delete");
Btndelete.forEach(function (btn) {
    btn.addEventListener("click", function () {
        var id = this.getAttribute("data_id");

        fetch("/BillManage/XoaYeuCauXacThucNap", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                IdLichSuNap: id,
            }),
        })
            .then((response) => response.json())
            .then((result) => {
                if (result.success) {
                    window.history.back();
                    location.reload();
                    alert("Cập nhật thành công!!");
                    window.location.replace("/BillManage");
                } else {
                    alert("success: false::: " + result);
                }
            })
            .catch((error) => {
                console.error("Error:", error);
                alert("Có lỗi xảy ra. Vui lòng thử lại: " + error);
            });
    });
});

var currentPoints = document.getElementById("TongSoTienNap");
var currentPointsValue = parseInt(currentPoints.innerText);
currentPoints.innerText = formatNumberWithCommas(currentPointsValue);

function formatNumberWithCommas(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
