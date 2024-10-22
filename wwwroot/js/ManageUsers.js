var btndel = document.querySelectorAll(".btn-delete");

// Gán sự kiện click cho tất cả các button
btndel.forEach((button, index) => {
  button.addEventListener("click", function (event) {
    var userId = button.getAttribute("data-Userid");

    console.log("book id:", userId);

    fetch("/ManageUsers/RemoveUser", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ BookId: -1, Userid: userId }),
    })
      .then((response) => response.json())
      .then((data) => {
        if (data.success) {
          window.history.back();
          location.reload();
          alert("Người dùng đã được xóa!");
          window.location.replace("/ManageUsers");
        } else {
          alert(`Có lỗi xảy ra: ${data.message}. Vui lòng thử lại.`);
        }
      })
      .catch((error) => {
        console.error("Error:", error);
        alert("Có lỗi xảy ra. Vui lòng thử lại. Mana=>" + error);
      });
  });
});
