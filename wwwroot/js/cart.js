var btndel = document.querySelectorAll(".btndel");

// Gán sự kiện click cho tất cả các button
btndel.forEach((button, index) => {
    button.addEventListener("click", function (event) {
        button.getAttribute("data-userid");
        button.getAttribute("data-bookid");

        console.log("Button clicked:", index);
        console.log("user id:", button.getAttribute("data-userid"));
        console.log("book id:", button.getAttribute("data-bookid"));

        const bookId = button.getAttribute("data-bookid");
        const userId = button.getAttribute("data-userid");
        // Bạn có thể thực hiện thêm các thao tác khác tại đây

        fetch("/Cart/RemoveToCart", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ BookId: bookId, Userid: userId }),
        })
            .then((response) => response.json())
            .then((data) => {
                if (data.success) {
                    window.history.back();
                    location.reload();
                    alert("Sách đã được xóa khỏi giỏ hàng!");
                    window.location.replace("/Cart");
                } else {
                    alert(`Có lỗi xảy ra: ${data.message}. Vui lòng thử lại.`);
                }
            })
            .catch((error) => {
                console.error("Error:", error);
                alert("Có lỗi xảy ra. Vui lòng thử lại. da");
            });
    });
});

var titleSearch = document.querySelector(".titleSearch");
var btnsubmitSearch = document.querySelector("#btnsubmitSearch");
var inputSearch = document.querySelector("#inputSearch");
var listDocumentSearch = document.querySelector(".listDocumentSearch");
var listDocument = document.querySelector(".listDocument");

///----------------show notification when user dont enough mark------------------------------
const notification = document.getElementById("notification");

function showNotification() {
    notification.classList.add("show");
    setTimeout(() => {
        hideNotification();
    }, 3000);
}

function hideNotification() {
    notification.style.opacity = "0";
    setTimeout(() => {
        notification.classList.remove("show");
        notification.style.opacity = "1";
    }, 300);
}
// showNotificationBtn.addEventListener("click", showNotification);
notification.addEventListener("click", hideNotification);
//
//
//------------ format giá tiền-------------------
window.addEventListener("DOMContentLoaded", function () {
    var GiaTienTaiLieu = document.querySelectorAll(".GiaTienTaiLieu");

    GiaTienTaiLieu.forEach((item) => {
        item.innerHTML = formatNumberWithCommas(parseInt(item.innerText));
    });
});
function formatNumberWithCommas(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
