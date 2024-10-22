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

    if (value !== "") {
        formatNumberInput(); // Định dạng ngay lập tức nếu không rỗng
    }
}

// Thực hiện kiểm tra khi tải trang
window.onload = function () {
    checkInitialValue(); // Kiểm tra giá trị ban đầu của input
};

var btnPostRequestEditBook = document.getElementById("btnPostRequestEditBook");
// var btnXoaFILE = document.getElementById("btnXoaFILE");
var sendDataBookID = document.getElementById("sendDataBookID");
const bookID = sendDataBookID.getAttribute("data-bookidDel");
const inputFilePDFnew = document.getElementById("fileDoc");

// btnPostRequestEditBook.addEventListener("click", function () {
//   // Kiểm tra xem có tệp nào được chọn không
//   if (inputFilePDFnew.files && inputFilePDFnew.files.length > 0) {
//     // Lấy tên tệp đầu tiên được chọn
//     var fileName = inputFilePDFnew.files[0].name;
//     let result = confirm(
//       "Bạn có chắc chắn muốn thực hiện hành động này không?\nFile tài liệu trước đó sẽ bị xóa khi bạn nhấn 'OK'"
//     );
//     if (result) {
//       fetch("/DocumentsManage/UpdateFileBookNew", {
//         method: "POST",
//         headers: {
//           "Content-Type": "application/json",
//         },
//         body: JSON.stringify({ BookId: bookID, FileDocNew: fileName }),
//       })
//         .then((response) => {
//           if (!response.ok) {
//             throw new Error("Network response was not ok");
//           }
//           // Kiểm tra xem phản hồi có dữ liệu hay không
//           if (
//             response.headers.get("Content-Type").includes("application/json")
//           ) {
//             return response.json(); // Lấy phản hồi dưới dạng JSON
//           }
//         })
//         .then((data) => {
//           if (data.success) {
//             location.reload();
//             window.history.back();
//             window.history.back();
//             location.reload();
//             alert("Sách đã được thành File mới!");
//           } else {
//             alert(`Có lỗi xảy ra: ${data.message}. Vui lòng thử lại.`);
//           }
//         })
//         .catch((error) => {
//           console.error("Error:", error);
//           alert("Có lỗi xảy ra. Vui lòng thử lại. da");
//         });
//     } else {
//       console.log("djf111");
//     }
//   } else {
//     alert("Bạn chưa chọn file tài liệu mới!");
//   }
// });
