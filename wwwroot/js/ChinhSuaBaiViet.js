const btnChinhSua = document.getElementById("SubmitEditBlog");

btnChinhSua.addEventListener("click", function (e) {
    // e.preventDefault;

    const idblog = this.getAttribute("data-idBlog");
    const BlogName = document.getElementById("title").value;
    const BlogContent = document.getElementById("content").value;

    fetch(`/account/BaiVietDaDang/XacNhanChinhSuaBaiViet`, {
        method: "Post",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            BlogID: parseInt(idblog),
            BlogName: BlogName,
            BlogContent: BlogContent,
        }),
    }).then(function (response) {
        if (response.ok) {
            window.location.href = "/account/BaiVietDaDang";
        } else {
        }
    });
});
