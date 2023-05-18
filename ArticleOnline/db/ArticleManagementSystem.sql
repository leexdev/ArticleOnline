CREATE DATABASE ArticleManagementSystem;
GO

USE ArticleManagementSystem;
GO

CREATE TABLE [USER] 
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  Email NVARCHAR(50) NOT NULL,
  Password NVARCHAR(50) NOT NULL,
  DisplayName NVARCHAR(50) DEFAULT N'',
  JoinDate DATETIME NOT NULL DEFAULT GETDATE(),
  Avatar NVARCHAR(MAX) NOT NULL DEFAULT N'/Content/img/defaultuser.png',
  Role NVARCHAR(50) NOT NULL DEFAULT 'user',
  Deleted BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE [Category] 
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  Name NVARCHAR(50) NOT NULL,
  Deleted BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE [Article]
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  Avatar NVARCHAR(MAX) NOT NULL,
  Title NVARCHAR(200) NOT NULL,
  Description NVARCHAR(MAX) NOT NULL,
  Content NVARCHAR(MAX) NOT NULL,
  AuthorId UNIQUEIDENTIFIER NOT NULL,
  CategoryId UNIQUEIDENTIFIER NOT NULL,
  PublishDate DATETIME NOT NULL DEFAULT GETDATE(),
  ViewCount INT NOT NULL DEFAULT 0,
  Deleted BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (AuthorId) REFERENCES [USER](Id),
  FOREIGN KEY (CategoryId) REFERENCES [Category](Id)
);
GO

CREATE TABLE [Comment] 
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  Content NVARCHAR(100) NOT NULL,
  AuthorId UNIQUEIDENTIFIER NOT NULL,
  ArticleId UNIQUEIDENTIFIER NOT NULL,
  PublishDate DATETIME NOT NULL DEFAULT GETDATE(),
  FOREIGN KEY (AuthorId) REFERENCES [USER](Id),
  FOREIGN KEY (ArticleId) REFERENCES [Article](Id)
);


CREATE TABLE Advertisement
(
	Id INT IDENTITY (1, 1) NOT NULL,
	Title NVARCHAR(100),
	ImageUrl NVARCHAR(MAX) NOT NULL,
	TargetUrl NVARCHAR(MAX),
	CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
	ExpiredDate DATETIME
)


INSERT INTO dbo.[USER] (Id, Email, Password, DisplayName, JoinDate, Avatar, Role, Deleted) 
VALUES ('4A7A0469-EA5E-4EB3-8071-79744228D184', N'leex.dev@gmail.com', N'admin', N'Dương Lễ', GETDATE(), DEFAULT, N'admin', 0)
INSERT INTO dbo.[USER] (Id, Email, Password, DisplayName, JoinDate, Avatar, Role, Deleted)
VALUES ('5B5ACC91-0B15-4F64-9F30-92F21941A24D', N'tronglo@gmail.com', N'user', N'Trọng Fat', GETDATE(), DEFAULT, DEFAULT, 0)


INSERT INTO dbo.[CATEGORY] (Id, Name, Deleted)
VALUES (N'509EF075-F814-4C26-B690-ADF3398ACCEC', N'Thể thao', 0)
INSERT INTO dbo.[CATEGORY] (Id, Name, Deleted)
VALUES (N'015BB949-E724-47C3-9EC8-9581F4530816', N'Thời sự', 0)
INSERT INTO dbo.[CATEGORY] (Id, Name, Deleted)
VALUES (N'076AE839-816C-4BBA-997E-9148E22FA274', N'Công nghệ', 0)
INSERT INTO dbo.[CATEGORY] (Id, Name, Deleted)
VALUES (N'1D054DDC-8650-4E2A-9510-E4079F8846A3', N'Pháp luật', 0)
INSERT INTO dbo.[CATEGORY] (Id, Name, Deleted)
VALUES (N'25A1917E-9584-4408-8EA3-C6C0CDD27C4A', N'Giải trí', 0)


INSERT INTO dbo.[ARTICLE] (Id, Avatar, Title, Description, Content, AuthorId, CategoryId, PublishDate, ViewCount, Deleted)
VALUES (N'954C843F-C66D-4459-AFE4-1DAB951628C3', N'/Content/img/ronaldo-1660008562387368647921.jpg', N'Khốn khổ đời xin việc, CR7 cũng thành… CV7!', N'Người hâm mộ Ronaldo mới đây lại phải thêm một phen cám cảnh cho số phận thần tượng của mình. Sau khi bị 6 đội bóng hàng đầu châu Âu từ chối, Ronaldo đã phải nộp CV xin việc đến lần thứ 7.', N'Nội dung của bài viết', N'4A7A0469-EA5E-4EB3-8071-79744228D184', N'509EF075-F814-4C26-B690-ADF3398ACCEC', GETDATE(), 0, 0)
INSERT INTO dbo.[ARTICLE] (Id, Avatar, Title, Description, Content, AuthorId, CategoryId, PublishDate, ViewCount, Deleted)
VALUES (N'E0966C39-3684-48B3-AB73-69E25C007A0A', N'/Content/img/106574692-1591910886557ps5-3-16828249832511139165394.jpg', N'Sony đạt doanh thu kỷ lục nhờ bán chip và máy PlayStation5', N'Theo công bố của Sony về doanh thu hằng năm, phần lớn lợi nhuận của hãng điện tử này đã đạt mức kỷ lục trong năm tài khóa vừa qua.', N'Nội dung của bài viết', N'4A7A0469-EA5E-4EB3-8071-79744228D184', N'076AE839-816C-4BBA-997E-9148E22FA274', GETDATE(), 0, 0)
INSERT INTO dbo.[ARTICLE] (Id, Avatar, Title, Description, Content, AuthorId, CategoryId, PublishDate, ViewCount, Deleted)
VALUES (N'F52DD9BB-5840-4B23-84CC-6C342C5C1556', N'/Content/img/duoi-nuoc-chet-duoi-dap-nuoc-binh-khuong-binh-son-quang-ngai-tran-mai-29-4-2023-16827723632771433182563.jpg', N'Về quê nghỉ lễ, ba cháu bé chết đuối ở đập nước', N'Ba cháu bé là anh em chết đuối ở đập nước khi đi dạo cùng người lớn. Vụ việc thương tâm xảy ra ở xã Bình Khương, huyện Bình Sơn, Quảng Ngãi.', N'Nội dung của bài viết', N'5B5ACC91-0B15-4F64-9F30-92F21941A24D', N'015BB949-E724-47C3-9EC8-9581F4530816', GETDATE(), 0, 0)
INSERT INTO dbo.[ARTICLE] (Id, Avatar, Title, Description, Content, AuthorId, CategoryId, PublishDate, ViewCount, Deleted)
VALUES (N'CE6DDE2B-1281-4773-B65F-95BC96730042', N'/Content/img/ronaldo-16823874969711708280080.jpg', N'Cổ động viên Ả Rập khuyên Ronaldo giải nghệ vì đá quá tệ', N'Cổ động viên Ả Rập khuyên Ronaldo nên giải nghệ sau màn trình diễn tệ hại trong trận Al Nassr thua Al Wehda 0-1 ngày 25-4 ở bán kết Cúp nhà vua Saudi Arabia.', N'Nội dung của bài viết', N'5B5ACC91-0B15-4F64-9F30-92F21941A24D', N'509EF075-F814-4C26-B690-ADF3398ACCEC', GETDATE(), 0, 0)
INSERT INTO dbo.[ARTICLE] (Id, Avatar, Title, Description, Content, AuthorId, CategoryId, PublishDate, ViewCount, Deleted)
VALUES (N'E7D48413-3CAD-4293-B490-C4B1BEEA92DA', N'/Content/img/1610-1682676345801763363356.jpg', N'Tin tức giải trí ngày 28-4: Báo Trung Quốc khen Chi Pu là mỹ nhân số 1 Việt Nam', N'Một số tin tức đáng chú ý: Chi Pu xuất hiện trong trailer chương trình Trung Quốc; AMEE hội ngộ Katy Perry; Đêm nhạc Hiền Hồ xin rút bị hủy.', N'Nội dung của bài viết', N'5B5ACC91-0B15-4F64-9F30-92F21941A24D', N'25A1917E-9584-4408-8EA3-C6C0CDD27C4A', GETDATE(), 0, 0)


INSERT INTO dbo.Comment (Id, Content, AuthorId, ArticleId, PublishDate) VALUES (NEWID(), N'Ri con khóc thét', N'4A7A0469-EA5E-4EB3-8071-79744228D184', N'954C843F-C66D-4459-AFE4-1DAB951628C3', DEFAULT)
INSERT INTO dbo.Comment (Id, Content, AuthorId, ArticleId, PublishDate) VALUES (NEWID(), N'Tin chuẩn chưa anh', N'5B5ACC91-0B15-4F64-9F30-92F21941A24D', N'954C843F-C66D-4459-AFE4-1DAB951628C3', DEFAULT)
