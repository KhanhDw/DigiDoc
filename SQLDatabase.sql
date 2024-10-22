CREATE DATABASE QLSach;
GO

USE QLSach;
GO

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
	UserLevel INT, -- 1: member 2:Admin 3: AdminALL
	PointsDownloadFile int default 0,
	AnhBia NText,
	AnhDaiDien NText,
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO
CREATE TABLE Books (
    BookID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    Price DECIMAL(15, 0) NOT NULL,	
    FileDoc Text NOT NULL,
    ImgThumbnail Text NOT NULL,
	DownloadFile int not null,
	IdUserUploadedFile int not null,
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO
CREATE TABLE Genres (
    GenreID Int primary key IDENTITY(1,1),
	TenTheLoai Nvarchar(255),
);
GO
CREATE TABLE GenresAndBooks(
	GenresAndBookID int primary key IDENTITY(1,1),
	GenreID int,
	BookID int,
);
GO
-- giỏ hàng nơi lưu sách 
CREATE TABLE Cart (
    CartID INT PRIMARY KEY IDENTITY(1,1),
	BookID INT, --key
	UserID  INT, --key
    CreatedDate DATETIME DEFAULT GETDATE(),
);
GO

CREATE TABLE Blog (
    BlogID INT PRIMARY KEY IDENTITY(1,1),  -- ID tự động tăng
    BlogName NVARCHAR(255) NOT NULL,       -- Tên blog
    BlogContent NTEXT NOT NULL,             -- Nội dung blog
    BlogViews INT DEFAULT 0,               -- Lượt xem, mặc định là 0
	UserIDPost int not null,
    CreatedDate DATETIME DEFAULT GETDATE() -- Ngày tạo, mặc định là ngày hiện tại
);

GO


CREATE TABLE Comments (
    CommentID INT PRIMARY KEY IDENTITY(1,1),   -- ID của comment
    UserID INT NOT NULL,                        -- ID người dùng viết comment
    BookID INT NOT NULL,                        -- ID bài viết mà comment thuộc về
    Content NTEXT NOT NULL,                       -- Nội dung của comment
    CreatedDate DATETIME DEFAULT GETDATE()  -- Thời gian tạo comment
);

CREATE TABLE LichSuNapDiem (
    LichSuNapDiemID INT PRIMARY KEY IDENTITY(1,1),
	UserID INT NOT NULL,      
    DiemNap int not null,
	SotienNap int not null,
	TrangThai Nvarchar(100) not null, --đã xác nhận và chưa xác nhận
	CreatedDate DATETIME DEFAULT GETDATE(),
);

CREATE TABLE HeThongDiemNap (
    DiemNapID INT PRIMARY KEY IDENTITY(1,1),
	SoDiem INT NOT NULL,      
    SoTien int not null,
);

insert into HeThongDiemNap (SoDiem, SoTien) values (100000, 200000);

select * from HeThongDiemNap;

TRUNCATE TABLE HeThongDiemNap;

DROP TABLE IF EXISTS HeThongDiemNap;



select * from LichSuNapDiem;

insert into LichSuNapDiem (UserId, DiemNap, SotienNap, TrangThai) 
values (1, 200000, 200000, N'Chưa xác nhận');

DROP TABLE IF EXISTS LichSuNapDiem


insert into Comments (UserId, Bookid, Content) VALUES (1, 1, 'test comment')
Drop Table Comments


-- Đặt Khóa --
--Thiết lập khóa ngoại giữa bảng Books và Users (người dùng tải lên sách):
ALTER TABLE Books
ADD CONSTRAINT FK_Books_Users FOREIGN KEY (IdUserUploadedFile) REFERENCES Users(UserID);
--Thiết lập khóa ngoại giữa bảng GenresAndBooks, Genres, và Books:
ALTER TABLE GenresAndBooks
ADD CONSTRAINT FK_GenresAndBooks_Genres FOREIGN KEY (Genreid) REFERENCES Genres(GenreID);

ALTER TABLE GenresAndBooks
ADD CONSTRAINT FK_GenresAndBooks_Books FOREIGN KEY (Bookid) REFERENCES Books(BookID);
--Thiết lập khóa ngoại giữa bảng Cart, Books, và Users:
ALTER TABLE Cart
ADD CONSTRAINT FK_Cart_Books FOREIGN KEY (Bookid) REFERENCES Books(BookID);

ALTER TABLE Cart
ADD CONSTRAINT FK_Cart_Users FOREIGN KEY (Userid) REFERENCES Users(UserID);
--Thiết lập khóa ngoại giữa bảng Blog và Users (người đăng blog):
ALTER TABLE Blog
ADD CONSTRAINT FK_Blog_Users FOREIGN KEY (UserIDPost) REFERENCES Users(UserID);
--Thiết lập khóa ngoại giữa bảng Comments, Users, và Books:
ALTER TABLE Comments
ADD CONSTRAINT FK_Comments_Users FOREIGN KEY (UserId) REFERENCES Users(UserID);

ALTER TABLE Comments
ADD CONSTRAINT FK_Comments_Books FOREIGN KEY (Bookid) REFERENCES Books(BookID);
--Thiết lập khóa ngoại giữa bảng LichSuNapDiem và Users:
ALTER TABLE LichSuNapDiem
ADD CONSTRAINT FK_LichSuNapDiem_Users FOREIGN KEY (UserId) REFERENCES Users(UserID);
-------------
--xóa khóa ngoại--
--Xóa khóa ngoại giữa bảng Books và Users:
ALTER TABLE Books
DROP CONSTRAINT FK_Books_Users;
--Xóa khóa ngoại giữa bảng GenresAndBooks, Genres, và Books:
ALTER TABLE GenresAndBooks
DROP CONSTRAINT FK_GenresAndBooks_Genres;

ALTER TABLE GenresAndBooks
DROP CONSTRAINT FK_GenresAndBooks_Books;

--Xóa khóa ngoại giữa bảng Cart, Books, và Users:
ALTER TABLE Cart
DROP CONSTRAINT FK_Cart_Books;

ALTER TABLE Cart
DROP CONSTRAINT FK_Cart_Users;

--Xóa khóa ngoại giữa bảng Blog và Users:
ALTER TABLE Blog
DROP CONSTRAINT FK_Blog_Users;

--Xóa khóa ngoại giữa bảng Comments, Users, và Books:
ALTER TABLE Comments
DROP CONSTRAINT FK_Comments_Users;

ALTER TABLE Comments
DROP CONSTRAINT FK_Comments_Books;

--Xóa khóa ngoại giữa bảng LichSuNapDiem và Users:
 ALTER TABLE LichSuNapDiem
DROP CONSTRAINT FK_LichSuNapDiem_Users;
------------------------------------------------






-- hiển thị dữ liệu
Select * From Orders;
Select * From CartItems;
Select * From CartDetail;
Select * From Cart;
Select * From Books;
Select * From Users;
Select * From Genres;


-- xóa bảng
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS CartItems;
DROP TABLE IF EXISTS CartDetail;
DROP TABLE IF EXISTS Cart;
DROP TABLE IF EXISTS Books;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Genres;


-- xóa dữ liệu trong bảng
TRUNCATE TABLE Orders;
TRUNCATE TABLE CartItems;
TRUNCATE TABLE CartDetail;
TRUNCATE TABLE Cart;
TRUNCATE TABLE Books;
TRUNCATE TABLE Users;
TRUNCATE TABLE Genres;
	

-- tạo lệnh xóa dữ liệu trong bảng books
DECLARE @BookID INT;
SET @BookID = 4;

BEGIN TRANSACTION;

-- Xóa dữ liệu từ các bảng liên quan dựa trên BookID
DELETE FROM GenresAndBooks WHERE Bookid = @BookID;
DELETE FROM Cart WHERE Bookid = @BookID;
DELETE FROM Comments WHERE Bookid = @BookID;

-- Sau khi xóa các liên kết, xóa sách từ bảng Books
DELETE FROM Books WHERE BookID = @BookID;

COMMIT;
-------------------------------------------







/* THÊM DỮ LIỆU */

INSERT INTO Books (Title, Author, Price, FileDoc, ImgThumbnail, DownloadFile, IdUserUploadedFile, CreatedDate)
VALUES (N'The Great Gatsby 1', N'F. Scott Fitzgerald', 10000, 'doc1.pdf', 'i1.png', 10, 1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 2', N'F. Scott Fitzgerald', 110000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
		(N'The Great Gatsby 3', N'F. Scott Fitzgerald', 1200000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 4', N'F. Scott Fitzgerald', 130000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 5', N'F. Scott Fitzgerald', 14000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 6', N'F. Scott Fitzgerald', 1000000000, 'doc1.pdf', 'i1.png', 10,1,CAST(GETDATE() AS DATE));
       
	   (N'The Great Gatsby 7', N'F. Scott Fitzgerald', 1600000000000, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
	   (N'The Great Gatsby 8', N'F. Scott Fitzgerald', 17.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 9', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 10', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
		(N'The Great Gatsby 11', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 12', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 13', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 14', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 15', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 16', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 17', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 18', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 19', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 20', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 21', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 22', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 23', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 24', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 25', N'F. Scott Fitzgerald', 18.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE)),
       (N'The Great Gatsby 26', N'F. Scott Fitzgerald', 19.99, 'doc1.pdf', 'i1.png', CAST(GETDATE() AS DATE));




INSERT INTO Blog (BlogName, BlogContent, BlogViews, UserIDPost, CreatedDate)
VALUES 
('Blog 1', N'Nội dung của Blog 1', 100, 1,'2024-10-01'),
('Blog 2', N'Nội dung của Blog 2', 150, 1,'2024-10-02'),
('Blog 3', N'Nội dung của Blog 3', 200, 1,'2024-10-03'),
('Blog 4', N'Nội dung của Blog 4', 250, 1,'2024-10-04'),
('Blog 5', N'Nội dung của Blog 5', 300, 1,'2024-10-05'),
('Blog 6', N'Nội dung của Blog 6', 350, 1,'2024-10-06'),
('Blog 7', N'Nội dung của Blog 7', 400,1, '2024-10-07'),
('Blog 8', N'Nội dung của Blog 8', 450, 1,'2024-10-08'),
('Blog 9', N'Nội dung của Blog 9', 500, 1,'2024-10-09'),
('Blog 10', N'Nội dung của Blog 10', 550, 1,'2024-10-10');



/* thêm dữ liệu cho bảng UESER */
-- Insert sample data into Users table
INSERT INTO Users (UserName, Email, PasswordHash, UserLevel,CreatedDate )
VALUES 
('admin', 'admin@example.com', 'hashed_password_3', 2, GETDATE()),
('user', 'user@example.com', 'hashed_password_4', 1, GETDATE());



/* thêm dữ liệu bản thể loại Genres*/
--INSERT INTO Genres (TenTheLoai, GBookid)
INSERT INTO Genres (TenTheLoai)
VALUES (N'Bán hàng và kinh doanh'),
	(N'Phát triển bản thân'),
	(N'IT và lập trình'),
	(N'Kế toán, Thuế và Tài chính'),
	(N'Kiến trúc và xây dựng'),
	(N'Luật và pháp lý'),
	(N'Marketing và truyền thông'),
	(N'Thiết kế'),
	(N'Khác');


INSERT INTO GenresAndBooks (GenreID, BookID)
VALUES (1, 1), (2, 2), (3, 3), (4, 4), (6, 5), (6, 6);


INSERT INTO GenresAndBooks (GenreID, BookID)
VALUES (1, 7), (2, 8), (3, 9), (4, 10), (6, 11), (6, 12);


INSERT INTO Cart (Bookid, Userid)
VALUES (1, 1), (2, 1), (3, 1);


-- hiển thị dữ liệu
select * from Comments;
select *  from Cart;
select *  from GenresAndBooks;
select *  from Genres;
select *  from Books;
select *  from Users;
select *  from Blog;
select *  from LichSuNapDiem;
select *  from HeThongDiemNap;
-- xóa bảng

DROP TABLE IF EXISTS Cart;
DROP TABLE IF EXISTS Books;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Genres;
DROP TABLE IF EXISTS GenresAndBooks;
DROP TABLE IF EXISTS Comments;
DROP TABLE IF EXISTS LichSuNapDiem;
DROP TABLE IF EXISTS Blog;
DROP TABLE IF EXISTS HeThongDiemNap;

-- xóa dữ liệu trong bảng
TRUNCATE TABLE Comments;
TRUNCATE TABLE Bills;
TRUNCATE TABLE Cart;
TRUNCATE TABLE GenresAndBooks;
TRUNCATE TABLE Genres;
TRUNCATE TABLE Books;
TRUNCATE TABLE Users;




DELETE FROM Cart WHERE Bookid = 5 and Userid = 1;


select * from Books
where BookID In (1,2);


UPDATE Users
SET PointsDownloadFile = 0
WHERE UserID = 1;

Update Users set PointsDownloadFile = 10000000 where UserID = 1 


Update Users set UserLevel = 3 where UserID = 1 


INSERT INTO Users (UserName, Email, PasswordHash, UserLevel, CreatedDate)
VALUES 
('user1', 'user1@domain.com', 'hashed_password_1', 1, GETDATE()),
('user2', 'user2@domain.com', 'hashed_password_2', 1, GETDATE()),
('user3', 'user3@domain.com', 'hashed_password_3', 1, GETDATE()),
('user4', 'user4@domain.com', 'hashed_password_4', 2, GETDATE()),
('user5', 'user5@domain.com', 'hashed_password_5', 2, GETDATE()),
('user6', 'user6@domain.com', 'hashed_password_6', 2, GETDATE()),
('user7', 'user7@domain.com', 'hashed_password_7', 1, GETDATE()),
('user8', 'user8@domain.com', 'hashed_password_8', 1, GETDATE()),
('user9', 'user9@domain.com', 'hashed_password_9', 3, GETDATE());

Select * From ;
Select * From Users;	
Select * From Comments;	

Delete Users where UserID=13;

insert into Comments (UserId, Bookid, Content) VALUES (4, 1, 'test comment111123')


Select * From Comments where Bookid = 2;	

select * from Books inner join GenresAndBooks on Books.BookID = GenresAndBooks.GenresAndBookID

SELECT 
    b.BookID,
    b.Title,
    b.Author,
    b.Price,
    b.FileDoc,
    b.ImgThumbnail,
    b.CreatedDate,
	g.TenTheLoai,
	u.UserName
FROM 
    Books b
INNER JOIN 
    Users u ON b.IdUserUploadedFile = u.UserID
INNER JOIN 
    GenresAndBooks gb ON b.BookID = gb.BookID
INNER JOIN 
    Genres g ON gb.Genreid = g.GenreID

Delete Books where BookID = 1
Delete Books where BookID = 2
Delete Books where BookID = 3
Delete Books where BookID = 4
Delete Books where BookID = 4
Delete Books where BookID = 5
Delete Books where BookID = 6
Delete Books where BookID = 7
Delete Books where BookID = 8
Delete Books where BookID = 18
Delete Books where BookID = 19
Delete Books where BookID = 20
Delete Books where BookID = 21
Delete Books where BookID = 22
Delete Books where BookID = 23
Delete Books where BookID = 24
Delete Books where BookID = 25
Delete Books where BookID = 26


UPDATE Books
SET DownloadFile = 251
WHERE BookID = 1;


Select * From Blog;
insert into Blog (BlogName, BlogContent) values ('blog 1011', 'kiểm tra nội dung')