var inputChangeUserName = document.getElementById("idUserName");
var btnChangeUserName = document.getElementById("btnEditUserName");

btnChangeUserName.addEventListener("click", function () {
    if (inputChangeUserName.hasAttribute("disabled")) {
        inputChangeUserName.removeAttribute("disabled");
        btnChangeUserName.textContent = "Xác nhận";
        console.log('Thuộc tính "disabled" đã thêm.');
    } else {
        inputChangeUserName.setAttribute("disabled", true);
        console.log('Thuộc tính "disabled" đã bị xóa.');
        btnChangeUserName.textContent = "Cập nhật tên người dùng";
    }
});

var inputChangePassword = document.getElementById("inputpassword");
var btnChangePassword = document.getElementById("btnChangePassword");
var lbnewPassword = document.getElementById("lbnewPassword");
var inputNewPassword = document.getElementById("inputpasswordNew");

btnChangePassword.addEventListener("click", function () {
    if (inputChangePassword.hasAttribute("disabled")) {
        inputChangePassword.removeAttribute("disabled");
        btnChangePassword.textContent = "Xác nhận";
        console.log('Thuộc tính "disabled" đã thêm.');
        lbnewPassword.removeAttribute("hidden");
        inputNewPassword.removeAttribute("hidden");
    } else {
        inputChangePassword.setAttribute("disabled", true);
        console.log('Thuộc tính "disabled" đã bị xóa.');
        btnChangePassword.textContent = "Cập nhật tên người dùng";
        lbnewPassword.setAttribute("hidden", true);
        inputNewPassword.setAttribute("hidden", true);
    }
});
