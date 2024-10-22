var btnDels = document.querySelectorAll(".btndel");
document.addEventListener("DOMContentLoaded", function () {
    btnDels.forEach((btnDel) => {
        btnDel.addEventListener("click", function (e) {
            e.preventDefault;

            var bookIDremove = btnDel.getAttribute("data-idBookRemove");
            console.log("1::: " + bookIDremove);

            let XacNhan = confirm(
                "Bạn chắc chắn muốn xóa tài liệu này đúng không?"
            );
            if (!XacNhan) {
                return;
            }

            ////Account/RemoveBookinAccount${bookIDremove}
            ///Account/ManageUploadedDocuments/RemoveBookinAccount

            fetch("/Account/ManageUploadedDocuments/RemoveBookinAccount", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    BookID_remove: parseInt(bookIDremove),
                }),
            })
                .then((response) => response.text())
                .then((result) => {
                    if (result.success) {
                        console.log(`thanfh coong xpas:${result.message}`);
                    }
                    if (result.success == false) {
                        console.log(`thanfh coong lỗi :${result.message}`);
                    }
                    // window.history.back();
                    // location.reload();
                })
                .catch((error) => {
                    console.error("Error:", error);
                });
        });
    });
});
