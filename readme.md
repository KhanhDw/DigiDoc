//---------------------------------
dotnet watch run : tự động reset server
dotnet run watch : phải reset mỗi lần thay đổi

//---------------------------------
để cài scss

npm install

npm install -g sass

(trong package.json) trong script{}

"sass": "sass --watch wwwroot/scss:wwwroot/css"

npm run sass : run để chạy trình biên dịch scss

//---------------------------------
_các thư viện cần thiết cho code sql vào web_

1. Microsoft.AspNetCore.Session
2. Microsoft.EntityFrameworkCore
3. Microsoft.EntityFrameworkCore.SqlServer
4. Microsoft.EntityFrameworkCore.Tools
5. Microsoft.Extensions.Caching.Memory
6.

//----------------------------------
_Migration để cập nhật cơ sở dữ liệu sql server dùng trong visual studio 2022_

để có thể add table từ sql vào project cần tạo các đối tượng trong Model, mỗi đối tượng là 1 bảng dữ liệu. sau đó cần vào Service/ApplicationBbContext.css để tạo đối tượng bảng

quy chuẩn đặt tên khóa chính để Migration có thể nhận diện: <Tên bảng>ID
ví dụ: bảng User -> Key -> UserID

// tên biến đặt trong file databaseContext cần phải giống với tên bảng trong sql server

Add-Migration <NameFile>
Add-Migration CreateDB

Update-Database

Remove-Migration

nếu Migration build faile thì làm các bước sau

1. fix tất cả lổi trong hệ thống
2. clearn project
3. rebuild project
4. thông báo không còn lỗi -> chạy được lệnh migration

_Lệnh cập nhật migration trong visual studio code_
cài đặt: dotnet tool install --global dotnet-ef
kiểm tra: dotnet ef
cập nhật: dotnet ef database update
tạo mới: dotnet ef migrations add <TênMigration>
tạo mới: dotnet ef Remove-Migration

//--------------------------------------------------
_mẹo quay lại trang trước đó_
<button onclick="window.history.back();"></button>
_tải lại trang hiện tại_
<button onclick="location.reload();"></button>

//-------------------------------------------------

nếu lỗi không truy cập được vào sql server thì sai đường dẫn trong package.json

//-------------------------------------------------
Hiển thị dữ liệu ra console trong vs 2022

System.Diagnostics.Debug.WriteLine("--->>>>>--->>--->" + userIdClaim);

//-------------------------------------------------

giao diện quản lý
tài liệu
người dùng
hóa đơn thanh toán

giao diện
tải sách
sửa sách
sửa người dùng - vai trò - cấp lại mật khẩu gửi đến email người dùng - địa chỉ email

giao diện tải sách
tên sách
tác giả
giá
hình ảnh
thể loại

add QR code thanh toán
