document.addEventListener("DOMContentLoaded", function () {
    let DiemTieuDungHeader = document.getElementById("DiemTieuDungHeader");

    // console.log(DiemTieuDungHeader);
    if (DiemTieuDungHeader) {

        //thế này sẽ không bị gi đè
        window.addEventListener("load", function () {
            formatNumbersInSpecificParagraphs(DiemTieuDungHeader); // Định dạng giá trị ban đầu trong các thẻ p có class 'format-number'
        });

        function formatNumbersInSpecificParagraphs(paragraphs) {
            // Lấy tất cả các thẻ p có class 'format-number'
            let value = paragraphs.innerText.replace(/,/g, ""); // Loại bỏ dấu phẩy cũ nếu có

            if (value !== "" && !isNaN(value)) {
                paragraphs.innerText = parseFloat(value).toLocaleString("en"); // Định dạng số có dấu phẩy
            }
        }

    }
   
});
