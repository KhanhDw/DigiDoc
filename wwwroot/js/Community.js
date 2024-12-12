const links = document.querySelectorAll(".item-blog-post");
const blogPostShow = document.getElementById("blog-post-show");
const BtnBack = document.getElementById("BtnBack");
const allDivs = document.querySelectorAll(".blog-post");
const article_FormPostBlog = document.getElementById("article_FormPostBlog");
const btnPostBlog = document.getElementById("btnPostBlog");
const isAuthenticated = btnPostBlog.getAttribute("data-authenticated") === "True";
const btnBackViewPost = document.getElementById("btnBackViewPost");
const itemBlogXemNhieu = document.querySelectorAll(".itemBlogXemNhieu");
const title_name = document.getElementById("title_name");
const btnNotification = document.getElementById("btnNotification");
const Notification_show = document.getElementById("Notification-show");

var fetch_Flag = 0;

document.addEventListener("DOMContentLoaded", function () {
    // Kiểm tra xem thẻ a có tồn tại không
    links.forEach((link) => {
        link.addEventListener("click", function (event) {
            event.preventDefault(); // Ngăn chặn hành động mặc định của thẻ a

            var data_post_id = link.getAttribute("data-post-id");
            var data_numberOfView = parseInt(
                link.getAttribute("data-numberOfView"),
                10
            );

            fetch(`/Community/increaseView/${data_post_id}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    BlogID: data_post_id,
                    BlogViews: data_numberOfView,
                }),
            })
                .then((response) => {
                    if (response.ok) {
                        console.log("View count updated successfully!");
                        // Cập nhật lại thuộc tính data-numberOfView trên thẻ <a>
                    } else {
                        console.error("Failed to update view count.");
                    }
                })
                .catch((error) => {
                    console.error("Error:", error);
                });

            // Ẩn tất cả các thẻ div

            blogPostShow.classList.remove("hidden-pre-animation");

            const parent = link.parentElement;

            var postId = parent.getAttribute("data-post-id");
            console.log(postId);
            // Ẩn tất cả các thẻ div
            allDivs.forEach((div) => {
                // Thêm class hidden-animation để bắt đầu animation
                div.classList.remove("show-pre-animation"); // Thêm class để chạy animation
                div.classList.add("hidden-animation");
                // Sau khi animation hoàn tất (0.5 giây), ẩn hoàn toàn thẻ div
                div.addEventListener(
                    "animationend",
                    function () {
                        div.hidden = true; // Ẩn thẻ div sau khi animation kết thúc
                        div.classList.remove("hidden-animation");

                        // Kiểm tra nếu tất cả các div blog-post đã bị ẩn, thì hiển thị blogPostShow
                        const allHidden = Array.from(allDivs).every(
                            (d) => d.hidden
                        );

                        if (allHidden && blogPostShow) {
                            // Thêm class show-animation để blogPostShow xuất hiện từ phải qua trái
                            blogPostShow.classList.add("show-animation");
                            blogPostShow.hidden = false;
                            blogPostShow.classList.remove("show-animation");
                        }
                    },
                    { once: true }
                ); // Lắng nghe sự kiện một lần
            });
            fetch_Flag = 1;
            if (fetch_Flag === 1) {
                // Xây dựng URL cho yêu cầu Fetch
                const url = `/Community/Blog/${postId}`;
                // Gửi yêu cầu GET với ID bài viết
                fetch(url, {
                    method: "GET",
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
                    .then((post) => {
                        // Hiển thị dữ liệu bài viết

                        document.getElementById("NameBlog").textContent =
                            post.BlogName;
                        document.getElementById("NumberOfView").textContent =
                            post.BlogViews;

                        const date = new Date(post.CreatedDate); // Chuyển chuỗi thành đối tượng Date
                        const formattedDate = date.toLocaleDateString("vi-VN"); // Định dạng theo chuẩn Việt Nam

                        document.getElementById("dateCreateBlog").textContent =
                            formattedDate;
                        document.getElementById(
                            "blog-content-Center"
                        ).textContent = post.BlogContent;

                        console.log(typeof post.CreatedDate);
                    })
                    .catch((error) => {
                        alert("Có lỗi xảy ra: 111::: " + error.message);
                    });

                fetch_Flag = 0;
            }
        });
    });
});
//------------------------------hiển thị bài viết của lượt xem nhiều-----------------------------------
itemBlogXemNhieu.forEach((item) => {
    item.addEventListener("click", function (event) {
        event.preventDefault();

        blogPostShow.classList.remove("hidden-pre-animation");
        Notification_show.classList.remove("hidden-pre-animation");

        // Ẩn tất cả các thẻ div
        allDivs.forEach((div) => {
            // Thêm class hidden-animation để bắt đầu animation
            // div.classList.remove("show-pre-animation"); // Thêm class để chạy animation
            div.classList.add("hidden-animation");
            Notification_show.hidden = true;
            blogPostShow.classList.add("show-animation");
            blogPostShow.hidden = false;
            blogPostShow.classList.remove("show-animation");
            // Sau khi animation hoàn tất (0.5 giây), ẩn hoàn toàn thẻ div
            div.addEventListener(
                "animationend",
                function () {
                    div.classList.remove("hidden-animation");
                    div.hidden = true; // Ẩn thẻ div sau khi animation kết thúc
                    blogPostShow.classList.remove("show-animation");
                },
                { once: true }
            ); // Lắng nghe sự kiện một lần
        });

        var postId = item.getAttribute("data_id_itemXemnhieu");

        fetch_Flag = 2;
        if (fetch_Flag === 2) {
            // Xây dựng URL cho yêu cầu Fetch
            const url = `/Community/Blog/${postId}`;
            // Gửi yêu cầu GET với ID bài viết
            fetch(url, {
                method: "GET",
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
                .then((post) => {
                    // Hiển thị dữ liệu bài viết

                    document.getElementById("NameBlog").textContent =
                        post.BlogName;
                    document.getElementById("NumberOfView").textContent =
                        post.BlogViews;

                    const date = new Date(post.CreatedDate); // Chuyển chuỗi thành đối tượng Date
                    const formattedDate = date.toLocaleDateString("vi-VN"); // Định dạng theo chuẩn Việt Nam

                    document.getElementById("dateCreateBlog").textContent =
                        formattedDate;
                    document.getElementById("blog-content-Center").textContent =
                        post.BlogContent;
                })
                .catch((error) => {
                    alert("Có lỗi xảy ra: 111::: " + error.message);
                });
            fetch_Flag = 0;
        }
    });
});

//------------------------------------------------------------------

BtnBack.addEventListener("click", function (event) {
    event.preventDefault(); // Ngăn chặn hành động mặc định của thẻ a
    blogPostShow.classList.remove("show-animation"); // Thêm class để chạy animation
    blogPostShow.classList.add("hidden-pre-animation"); // Thêm class để chạy animation

    // Sau khi animation kết thúc, ẩn thẻ div
    blogPostShow.addEventListener(
        "animationend",
        function () {
            blogPostShow.hidden = true;

            allDivs.forEach((div) => {
                // Thêm class hidden-animation để bắt đầu animation
                div.classList.remove("hidden-animation");
                div.classList.add("show-pre-animation");
                div.hidden = false;
                div.classList.remove("show-pre-animation");
                blogPostShow.classList.remove("hidden-pre-animation");
            });
        },
        { once: true }
    );
});

btnPostBlog.addEventListener("click", function (event) {
    if (isAuthenticated == false) {
        window.location.href = "/login";
    } else {
        event.preventDefault(); // Ngăn chặn hành động mặc định của thẻ a
    }
    title_name.textContent = "Đăng bài viết";

    // article_FormPostBlog.hidden = false;
    if (!Notification_show.hidden) {
        Notification_show.hidden = true; // Ẩn thẻ div sau khi animation kết thúc
        article_FormPostBlog.classList.add("show-animation"); // Thêm class để chạy animation
        article_FormPostBlog.classList.remove("hidden-pre-animation"); // Thêm class để chạy animation

        blogPostShow.classList.remove("show-pre-animation"); // Thêm class để chạy animation
        blogPostShow.classList.add("hidden-animation");

        Notification_show.classList.add("show-animation"); // Thêm class để chạy animation
        article_FormPostBlog.hidden = false;
        blogPostShow.classList.remove("hidden-animation");
        article_FormPostBlog.classList.remove("show-animation");
        Notification_show.classList.remove("show-animation"); // Thêm class để chạy animation
    }

    allDivs.forEach((div) => {
        if (!div.hasAttribute("hidden")) {
            article_FormPostBlog.classList.add("show-animation"); // Thêm class để chạy animation
            article_FormPostBlog.classList.remove("hidden-pre-animation"); // Thêm class để chạy animation

            div.classList.remove("show-pre-animation"); // Thêm class để chạy animation
            div.classList.add("hidden-animation");

            // Sau khi animation hoàn tất (0.5 giây), ẩn hoàn toàn thẻ div
            div.addEventListener(
                "animationend",
                function () {
                    div.classList.remove("hidden-animation");
                    div.hidden = true; // Ẩn thẻ div sau khi animation kết thúc
                    // Kiểm tra nếu tất cả các div blog-post đã bị ẩn, thì hiển thị blogPostShow
                    const allHidden = Array.from(allDivs).every(
                        (d) => d.hidden
                    );

                    if (allHidden && blogPostShow) {
                        // Thêm class show-animation để blogPostShow xuất hiện từ phải qua trái
                        article_FormPostBlog.hidden = false;
                        article_FormPostBlog.classList.add("show-animation");
                        // article_FormPostBlog.classList.remove("show-animation");
                    }
                },
                { once: true }
            ); // Lắng nghe sự kiện một lần
        }
    });
    if (!blogPostShow.hasAttribute("hidden")) {
        article_FormPostBlog.classList.add("show-animation"); // Thêm class để chạy animation
        article_FormPostBlog.classList.remove("hidden-pre-animation"); // Thêm class để chạy animation

        blogPostShow.classList.remove("show-pre-animation"); // Thêm class để chạy animation
        blogPostShow.classList.add("hidden-animation");

        // // Notification_show.classList.remove("show-pre-animation"); // Thêm class để chạy animation
        Notification_show.classList.add("show-animation"); // Thêm class để chạy animation
        // Notification_show.classList.add("hidden-animation");

        blogPostShow.addEventListener(
            "animationend",
            function () {
                blogPostShow.hidden = true; // Ẩn thẻ div sau khi animation kết thúc
                Notification_show.hidden = true; // Ẩn thẻ div sau khi animation kết thúc
                // Kiểm tra nếu tất cả các div blog-post đã bị ẩn, thì hiển thị blogPostShow

                if (blogPostShow && Notification_show) {
                    // Thêm class show-animation để blogPostShow xuất hiện từ phải qua trái
                    article_FormPostBlog.hidden = false;

                    article_FormPostBlog.classList.add("show-animation");
                    article_FormPostBlog.classList.remove("show-animation");

                    // Notification_show.classList.add("show-animation");
                    Notification_show.classList.remove("show-animation");

                    blogPostShow.classList.remove("hidden-animation");
                }
            },
            { once: true }
        ); // Lắng nghe sự kiện một lần
    }
});
btnBackViewPost.addEventListener("click", function (event) {
    event.preventDefault(); // Ngăn chặn hành động mặc định của thẻ a

    title_name.textContent = "Bài viết cộng đồng";

    article_FormPostBlog.classList.remove("show-animation"); // Thêm class để chạy animation
    article_FormPostBlog.classList.add("hidden-pre-animation"); // Thêm class để chạy animation

    // Sau khi animation kết thúc, ẩn thẻ div
    article_FormPostBlog.addEventListener(
        "animationend",
        function () {
            article_FormPostBlog.hidden = true;

            allDivs.forEach((div) => {
                // Thêm class hidden-animation để bắt đầu animation
                div.classList.remove("hidden-animation");
                div.classList.add("show-pre-animation");
                div.hidden = false;
                article_FormPostBlog.classList.remove("hidden-pre-animation");
                div.classList.remove("show-pre-animation");
            });
        },
        { once: true }
    );
});

btnNotification.addEventListener("click", function (event) {
    event.preventDefault();

    if (title_name.textContent != "Đăng bài viết")
        title_name.textContent = "Thông báo";

    allDivs.forEach((div) => {
        if (!div.hasAttribute("hidden")) {
            Notification_show.classList.add("show-animation"); // Thêm class để chạy animation
            Notification_show.classList.remove("hidden-pre-animation"); // Thêm class để chạy animation

            div.classList.remove("show-pre-animation"); // Thêm class để chạy animation
            div.classList.add("hidden-animation");

            // Sau khi animation hoàn tất (0.5 giây), ẩn hoàn toàn thẻ div
            div.addEventListener(
                "animationend",
                function () {
                    div.classList.remove("hidden-animation");
                    div.hidden = true; // Ẩn thẻ div sau khi animation kết thúc
                    // Kiểm tra nếu tất cả các div blog-post đã bị ẩn, thì hiển thị blogPostShow
                    const allHidden = Array.from(allDivs).every(
                        (d) => d.hidden
                    );

                    if (allHidden && Notification_show) {
                        // Thêm class show-animation để blogPostShow xuất hiện từ phải qua trái
                        Notification_show.hidden = false;
                        Notification_show.classList.remove("show-animation");
                        // article_FormPostBlog.classList.remove("show-animation");
                        Notification_show.classList.remove("show-animation"); // Thêm class để chạy animation
                    }
                },
                { once: true }
            ); // Lắng nghe sự kiện một lần
        }
    });

    if (!blogPostShow.hasAttribute("hidden")) {
        article_FormPostBlog.classList.add("show-animation"); // Thêm class để chạy animation
        article_FormPostBlog.classList.remove("hidden-pre-animation"); // Thêm class để chạy animation

        blogPostShow.classList.remove("show-pre-animation"); // Thêm class để chạy animation
        blogPostShow.classList.add("hidden-animation");

        blogPostShow.addEventListener(
            "animationend",
            function () {
                blogPostShow.hidden = true; // Ẩn thẻ div sau khi animation kết thúc
                article_FormPostBlog.hidden = true; // Ẩn thẻ div sau khi animation kết thúc
                // Kiểm tra nếu tất cả các div blog-post đã bị ẩn, thì hiển thị blogPostShow

                if (Notification_show) {
                    // Thêm class show-animation để blogPostShow xuất hiện từ phải qua trái
                    Notification_show.classList.add("show-animation");
                    Notification_show.hidden = false;
                    Notification_show.classList.remove("show-animation");
                    blogPostShow.classList.remove("hidden-animation");
                    article_FormPostBlog.classList.remove("hidden-animation");
                }
            },
            { once: true }
        ); // Lắng nghe sự kiện một lần
    }
});
