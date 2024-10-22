let currentPage = 1;
let pdfDocVar = null;
let totalPages = 0;

var srcbook = document.getElementById("scrBook").getAttribute("data-scrBook");

// let srcbookURL = "./../storage/filedoc/doc3.pdf";
let srcbookURL = "./../storage/filedoc/" + srcbook;

initPDFViewer = () => {
    pdfjsLib
        .getDocument(srcbookURL)
        .promise.then((pdfDoc) => {
            // Output current page
            console.log(pdfDoc);
            pdfDocVar = pdfDoc;
            totalPages = pdfDoc._pdfInfo.numPages;
            document.querySelector("#page-count").textContent = totalPages;
            renderPage(currentPage); // Hiển thị trang đầu tiên
        })
        .catch((err) => {
            console.log(err);
        });
};

renderPage = (pageNumber) => {
    pdfDocVar
        .getPage(pageNumber)
        .then((page) => {
            document.querySelector("#page-num").textContent = pageNumber;
            let canvas = document.getElementById("pdf-render");
            let context = canvas.getContext("2d");
            let viewport = page.getViewport({ scale: 1.7 });
            canvas.height = viewport.height;
            canvas.width = viewport.width;
            page.render({
                canvasContext: context,
                viewport: viewport,
            });
        })
        .catch((err) => {
            console.log(err);
        });
};

$(function () {
    initPDFViewer();
});

// Button Events
// Hàm để chuyển sang trang tiếp theo
document.getElementById("next-page").addEventListener("click", () => {
    if (currentPage < totalPages) {
        currentPage++;
        renderPage(currentPage);
    }
});

// Hàm để chuyển về trang trước (nếu cần)
document.getElementById("prev-page").addEventListener("click", () => {
    if (currentPage > 1) {
        currentPage--;
        renderPage(currentPage);
    }
});

//------------------------------------------------------

document.getElementById("showOverlay").addEventListener("click", function () {
    document.getElementById("overlay").style.display = "block";
});

document.getElementById("closeOverlay").addEventListener("click", function () {
    document.getElementById("overlay").style.display = "none";
});

var btnSave = document.getElementById("btnSave");
var titleBtnSave = document.getElementById("titleBtnSave");

document.addEventListener("DOMContentLoaded", function () {
    btnSave.addEventListener("click", function () {
        if (titleBtnSave.getAttribute("data-user-id") == -1) {
            window.location.href = "/Login";
        } else {
            const bookId = titleBtnSave.getAttribute("data-book-id");
            const userId = titleBtnSave.getAttribute("data-user-id");

            if (this.classList.contains("btnSave")) {
                this.classList.remove("btnSave");
                this.classList.add("btnSaved");
                titleBtnSave.textContent = "Đã lưu";

                fetch("/Cart/AddToCart", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({ BookId: bookId, Userid: userId }),
                })
                    .then((response) => response.json())
                    .then((data) => {
                        if (data.success) {
                            alert("Sách đã được thêm vào giỏ hàng!");
                            window.history.back();
                        } else {
                            alert("Có lỗi xảy ra. Vui lòng thử lại.");
                        }
                    })
                    .catch((error) => {
                        console.error("Error:", error);
                        alert("Có lỗi xảy ra. Vui lòng thử lại. da");
                    });
            } else {
                this.classList.remove("btnSaved");
                this.classList.add("btnSave");
                titleBtnSave.textContent = "Lưu"; // hủy lưu sách => nên chuyển về lưu

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
                            alert("Sách đã được xóa khỏi giỏ hàng!");
                            window.history.back();
                        } else {
                            alert(
                                `Có lỗi xảy ra: ${data.message}. Vui lòng thử lại.`
                            );
                        }
                    })
                    .catch((error) => {
                        console.error("Error:", error);
                        alert("Có lỗi xảy ra. Vui lòng thử lại. da");
                    });
            }
            console.log("test");
        }
    });
});

//---------------------post comment------------------------
var BtnComment = document.getElementsByClassName("BtnPostComment");

// BtnComment.addEventListener("click", function () {
//   fetch("/Cart/RemoveToCart", {
//     method: "POST",
//     headers: {
//       "Content-Type": "application/json",
//     },
//     body: JSON.stringify({ BookId: bookId, Userid: userId }),
//   })
//     .then((response) => response.json())
//     .then((data) => {
//       if (data.success) {
//         alert("Sách đã được xóa khỏi giỏ hàng!");
//         window.history.back();
//       } else {
//         alert(`Có lỗi xảy ra: ${data.message}. Vui lòng thử lại.`);
//       }
//     })
//     .catch((error) => {
//       console.error("Error:", error);
//       alert("Có lỗi xảy ra. Vui lòng thử lại. da");
//     });
// });
//-------------------------------------------------------------------

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

///----------------show notification when user dont enough mark------------------------------
const notification = document.getElementById("notification");
const showNotificationBtn = document.getElementById("thanhtoan");

function showNotification() {
    notification.classList.add("show");
    setTimeout(() => {
        hideNotification();
    }, 5000);
}

function hideNotification() {
    notification.style.opacity = "0";
    setTimeout(() => {
        notification.classList.remove("show");
        notification.style.opacity = "1";
    }, 300);
}

showNotificationBtn.addEventListener("click", showNotification);
notification.addEventListener("click", hideNotification);
//--------------------------------------------

// document.getElementById("thanhtoan").addEventListener("click", function (e) {
//     // Gọi đến route cụ thể "/detail/KhongDuDiem"
//     fetch("/DetailDoc/download/KhongDuDiem")
//         .then((response) => response.json()) // Chuyển phản hồi thành JSON
//         .then((data) => {
//             // alert(data.message);
//         })
//         .catch((error) => {
//             console.error("Error:", error);
//         });
// });
