var password_user_input = document.getElementById("inputpassword");

document
    .getElementById("hiddenpassword")
    .addEventListener("click", function () {
        var icon = document.getElementById("hiddenpassword");
        if (icon.classList.contains("fa-eye")) {
            icon.classList.remove("fa-eye");
            icon.classList.add("fa-eye-slash");
            password_user_input.type = "password";
        } else {
            icon.classList.remove("fa-eye-slash");
            icon.classList.add("fa-eye");

            password_user_input.type = "text";
        }
    });
