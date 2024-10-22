const links = document.querySelectorAll(".cometoBlog");
const btndel = document.querySelectorAll(".btndel");

const listDocument = document.getElementById("listDocument");

const blogPostShow = document.getElementById("blog-post-show");

var Blog_data;

document.addEventListener("DOMContentLoaded", function () {
    // Kiểm tra xem thẻ a có tồn tại không
    btndel.forEach((link) => {
        link.addEventListener("click", function (event) {
            event.preventDefault(); // Ngăn chặn hành động mặc định của thẻ a

            let xacNhanXoa = confirm(
                "Bạn có chắc chắn muốn xóa bài viết này không?"
            );
            if (!xacNhanXoa) {
                return;
            }

            const idblog = link.getAttribute("data-idblog");

            const url = `/account/BaiVietDaDang/DeleteBlog/${idblog}`;
            // Gửi yêu cầu GET với ID bài viết
            fetch(url, {
                method: "Post",
                headers: {
                    "Content-Type": "application/json",
                },
            })
                .then((response) => {
                    if (!response.ok) {
                        throw new Error("Không tìm thấy bài viết 111");
                    }
                    return response.json();
                })
                .then((data) => {
                    if (data.success) {
                        window.history.back();
                        location.reload();
                        // alert("Bài blog đã được xóa!");
                        window.location.replace("/account/BaiVietDaDang");
                    } else {
                        alert("Có lỗi xảy ra: " + data.message);
                    }
                })
                .catch((error) => {
                    alert("Có lỗi xảy ra: 111::: " + error.message);
                });
        });
    });
});
