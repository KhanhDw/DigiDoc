document.addEventListener("DOMContentLoaded", function () {
    var currentPoints = document.getElementById("currentPointsCoins");
    var currentPointsValue = parseInt(currentPoints.innerText);
    currentPoints.innerText = formatNumberWithCommas(currentPointsValue);

    // const historyContainer = document.getElementById("history");
    // history.forEach((item) => {
    //     const historyItem = document.createElement("div");
    //     historyItem.className = `history-item ${
    //         item.status === "failed" ? "failed" : ""
    //     }`;
    //     historyItem.innerHTML = `
    //         <span>${item.date} - ${item.time}</span>
    //          <span>
    //          ${item.status === "failed" ? "Thất bại" : "Thành công"}
    //         </span>
    //         <span><span class="tien_lichSuNap">${item.SoDiem}</span> điểm</span>
    //         <span>
    //             <span><span class="tien_lichSuNap">${
    //                 item.value
    //             }</span> VND</span>
    //             <span class="status-indicator ${item.status}"></span>
    //         </span>
    //     `;
    //     historyContainer.appendChild(historyItem);
    // });

    const packagesContainer = document.getElementById("packages");
    // packages.forEach((pkg) => {
    //     const packageItem = document.createElement("div");
    //     packageItem.className = "package-item";
    //     packageItem.innerHTML = `
    //         <span>${pkg.SoDiem} điểm</span>
    //         <span><span class="SotienCanNap">${pkg.value}</span> VND</span>
    //     `;
    //     packagesContainer.appendChild(packageItem);
    // });

    const boxes = document.querySelectorAll(".package-item");

    var ThongTinSoTienNap = document.getElementById("ThongTinSoTienNap");
    var btn_select_package = document.getElementById("btn-select-package");
    var boxCard = document.getElementById("boxCard");
    var soDiemNap = "";
    var soTienNap = "";
    boxes.forEach((box) => {
        box.addEventListener("click", function () {
            if (this.classList.contains("select-package-active")) {
                // this.classList.remove("select-package-active");

                // Xóa class "active" ở tất cả các thẻ div
                boxes.forEach((b) => {
                    b.classList.remove("select-package-active");
                });

                //chặn hiển thị giao diện nạp
                btn_select_package.addEventListener("click", function () {
                    boxCard.hidden = true;
                });
            } else {
                // Thêm class "active" cho thẻ div được click
                this.classList.add("select-package-active");
                soDiemNap = box.querySelector(".SoDiemNap").textContent;
                soTienNap = box.querySelector(".SotienCanNap").textContent;

                // gỡ chặn hiển thị giao diện nạp
                btn_select_package.addEventListener("click", function () {
                    boxCard.hidden = false;
                    ThongTinSoTienNap.innerText = soDiemNap;
                });
            }

            if (this.classList.contains("select-package-active")) {
                document
                    .getElementById("btn-select-package")
                    .classList.add("active-btn-nap");
            } else {
                document
                    .getElementById("btn-select-package")
                    .classList.remove("active-btn-nap");
            }
        });
    });

    var btnXacNhanChuyenKhoanNapDiem = document.getElementById(
        "btnXacNhanChuyenKhoanNapDiem"
    );

    var UserId = btnXacNhanChuyenKhoanNapDiem.getAttribute("data_UserId");
    var UserName = btnXacNhanChuyenKhoanNapDiem.getAttribute("data_UserName");

    console.log(formatNumberWithCommas(soDiemNap));

    btnXacNhanChuyenKhoanNapDiem.addEventListener("click", function () {
        fetch("/BillManage/GuiYeuCauXacThucNap", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                UserId: UserId,
                DiemNap: parseInt(soDiemNap.replace(/,/g, "")),
                SotienNap: parseInt(soTienNap.replace(/,/g, "")),
            }),
        })
            .then((response) => response.json())
            .then((result) => {
                if (result.success) {
                    window.history.back();
                    location.reload();
                    // alert("Cập nhật thành công!!");
                    window.location.replace("/Coins");
                } else {
                    alert("success: false::: " + result);
                }
            })
            .catch((error) => {
                console.error("Error:", error);
                alert("Có lỗi xảy ra. Vui lòng thử lại: " + error);
            });
    });

    //----------------------------------format number money-------------
    let SoDiemNap = document.querySelectorAll(".SoDiemNap");
    let paragraphs = document.querySelectorAll(".SotienCanNap");
    let tien_lichSuNap = document.querySelectorAll(".tien_lichSuNap");

    function formatNumbersInSpecificParagraphs(paragraphs) {
        // Lấy tất cả các thẻ p có class 'format-number'

        paragraphs.forEach((paragraph) => {
            let value = paragraph.innerText.replace(/,/g, ""); // Loại bỏ dấu phẩy cũ nếu có

            if (value !== "" && !isNaN(value)) {
                paragraph.innerText = parseFloat(value).toLocaleString("en"); // Định dạng số có dấu phẩy
            }
        });
    }

    // Kiểm tra và định dạng khi tải trang
    window.onload = function () {
        formatNumbersInSpecificParagraphs(SoDiemNap); // Định dạng giá trị ban đầu trong các thẻ p có class 'format-number'
        formatNumbersInSpecificParagraphs(paragraphs); // Định dạng giá trị ban đầu trong các thẻ p có class 'format-number'
        formatNumbersInSpecificParagraphs(tien_lichSuNap); // Định dạng giá trị ban đầu trong các thẻ p có class 'format-number'
    };

    //-----------------------Nhập điểm nạp-------------------------

    const pointsList = document.getElementById("pointsList");
    const rechargeForm = document.getElementById("rechargeForm");
    const pointsInput = document.getElementById("SoDiem");
    const priceInput = document.getElementById("SoTien");
    const result = document.getElementById("result");
    const selectedPoints = document.getElementById("selectedPoints");
    const selectedPrice = document.getElementById("selectedPrice");
    const CloseOverPlay = document.getElementById("CloseOverPlay");
    const ContaintOverPlayDieuChinhDiem = document.getElementById(
        "ContaintOverPlayDieuChinhDiem"
    );
    const ShowSystemRecharge = document.getElementById("ShowSystemRecharge");
    const CapNhatDataVaoHeThong = document.getElementById(
        "CapNhatDataVaoHeThong"
    );

    let enteredData = [];

    rechargeForm.addEventListener("submit", function (e) {
        e.preventDefault();

        const SoDiem = parseInt(pointsInput.value.replace(/,/g, ""));
        const SoTien = parseInt(priceInput.value.replace(/,/g, ""));

        // Add SoDiem and SoTien to the list
        enteredData.push({ SoDiem, SoTien });
        updatePointsList();

        // Update result display
        selectedPoints.textContent = `${SoDiem} SoDiem`;
        selectedPrice.textContent = `$${SoTien.toFixed(2)}`;
        result.classList.remove("hidden");

        // Clear form inputs
        pointsInput.value = "";
        priceInput.value = "";
    });

    function updatePointsList() {
        // Xóa tất cả các thẻ li bên trong ul

        // Lấy tất cả các thẻ li bên trong ul
        let listItems = pointsList.querySelectorAll("li");

        // Lặp qua từng thẻ li và kiểm tra class
        listItems.forEach((li) => {
            if (!li.classList.contains("server")) {
                pointsList.removeChild(li);
            }
        });

        // Sort the enteredData array in ascending order based on SoDiem
        enteredData.sort((a, b) => a.SoDiem - b.SoDiem);

        // pointsList.innerHTML = "";
        enteredData.forEach((data, index) => {
            const li = document.createElement("li");
            li.innerHTML = `
                <span>${formatNumberWithCommas(
                    data.SoDiem
                )} Điểm - ${formatNumberWithCommas(
                data.SoTien.toFixed(0)
            )} VND - <span>Chưa được Lưu</span></span>
                <button class="btnRemoveEntry" onclick="removeEntry(${index})">Xóa</button>
            `;

            pointsList.appendChild(li);

            console.log(data);

            // Gán sự kiện click cho nút "Xóa" sau khi thêm phần tử vào DOM
            li.querySelector(".btnRemoveEntry").addEventListener(
                "click",
                function () {
                    removeEntry(index);
                }
            );
        });
    }

    function removeEntry(index) {
        enteredData.splice(index, 1);
        updatePointsList();
    }

    //-----------------------ẩn hiện giao diện nhập điểm nạp----------

    CloseOverPlay.addEventListener("click", function () {
        ContaintOverPlayDieuChinhDiem.hidden = true;
    });
    ShowSystemRecharge.addEventListener("click", function () {
        ContaintOverPlayDieuChinhDiem.hidden = false;
    });

    //-------------gửi dữ liệu lên server đê thêm dữu liệu điểm nạp------------------------

    CapNhatDataVaoHeThong.addEventListener("click", function (event) {
        // event.preventDefault(); // Ngăn chặn hành động mặc định khi click vào link

        if (enteredData === null) {
            return;
        }

        // console.log(enteredData); // Xử lý k

        // Gửi dữ liệu tới Controller qua AJAX
        fetch("/Coins/CapNhatHeThongNapDiem", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ data: enteredData }),
        })
            .then((response) => response.json())
            .then((result) => {
                if (result.success) {
                    window.history.back();
                    location.reload();
                    alert("Cập nhật thành công!!");
                    window.location.replace("/Coins");
                } else {
                    alert("success: false::: " + result);
                }
            })
            .catch((error) => {
                console.error("Error:", error);
                alert("Có lỗi xảy ra. Vui lòng thử lại: " + error);
            });
    });

    //-----------------xóa mục nạp đã thêm--------------------

    const RemoveMucNap = document.querySelectorAll(".RemoveMucNap");

    RemoveMucNap.forEach((item) => {
        item.addEventListener("click", function () {
            var dataID = item.getAttribute("data-DiemNapID");

            fetch("/Coins/RemoveMucNap", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    DiemNapID: parseInt(dataID),
                    data: null,
                }),
            })
                .then((response) => response.json())
                .then((result) => {
                    if (result.success) {
                        window.history.back();
                        location.reload();
                        // alert("Cập nhật thành công!!");
                        window.location.replace("/Coins");
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
});

// Hàm chỉ cho phép nhập ký tự là số
function isNumberKey(evt) {
    var charCode = evt.which ? evt.which : evt.keyCode;
    // Kiểm tra mã phím, chỉ cho phép các phím số từ 0-9
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function formatNumberInput(inputSoDiemVaSoTien) {
    let value = inputSoDiemVaSoTien.value.replace(/,/g, ""); // Loại bỏ dấu phẩy cũ nếu có
    if (!isNaN(value) && value !== "") {
        inputSoDiemVaSoTien.value = parseFloat(value).toLocaleString("en"); // Định dạng số có dấu phẩy
    }
}

//----------format dấu phẩy cho chuổi đưa vào--------------------
function formatNumberWithCommas(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function handleCancel() {
    boxCard.hidden = true;
}
