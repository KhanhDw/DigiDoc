var password_user_input = document.getElementById("inputpassword");
var repassword_user_input = document.getElementById("reinputpassword");
var icon = document.getElementById("hiddenpassword");
var reicon = document.getElementById("rehiddenpassword");

document
    .getElementById("hiddenpassword")
    .addEventListener("click", function () {
        ShowAndHiddenPassword(icon, password_user_input);
    });
document
    .getElementById("rehiddenpassword")
    .addEventListener("click", function () {
        ShowAndHiddenPassword(reicon, repassword_user_input);
    });

function ShowAndHiddenPassword(icon, repassword_user_input) {
    if (icon.classList.contains("fa-eye")) {
        icon.classList.remove("fa-eye");
        icon.classList.add("fa-eye-slash");
        repassword_user_input.type = "password";
    } else {
        icon.classList.remove("fa-eye-slash");
        icon.classList.add("fa-eye");
        repassword_user_input.type = "text";
    }
}
