USE [movies]
GO
/****** Object:  StoredProcedure [dbo].[ClearTables]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClearTables]
	/*
		Löscht alle Inhalte der Tabellen und 
		stellt Auslieferungs-Zustand her.
	*/
AS
	
	/* 
		Alle Tabellen löschen
	*/
	
	DELETE FROM LogApplication;
	DELETE FROM LogSets;
	DELETE FROM tblAccounts WHERE ID > 1;
	DELETE FROM tblDocs;
	DELETE FROM tblImages;
	DELETE FROM tblJobOrders;
	DELETE FROM tblLandingPages;
	DELETE FROM tblProdutionServer;
	DELETE FROM tblLandingPages;
	DELETE FROM tblRoles WHERE ID > 1;
	DELETE FROM tblLandingPages;
	DELETE FROM tblUsers;
	
	/* 
		Neuen User anlegen
	*/
	
	INSERT INTO tblUsers
                         (Name, Password, AccountID, RoleID, Email, Description, Firstname, Lastname)
	VALUES        ('su','su',1,1,'su@localhost','Default user, generated while installation','Admin','DirectSmile');  
	
RETURN

GO
/****** Object:  StoredProcedure [dbo].[InsertAccount]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertAccount]
	(
	@Name nvarchar(50),
	@Description nvarchar(255),
	@ID_NEW int OUTPUT
	)
AS
	INSERT INTO tblAccounts([Name],Description)VALUES (@Name,@Description)
    SELECT @ID_NEW = @@IDENTITY
    RETURN (1)
        

GO
/****** Object:  StoredProcedure [dbo].[procUpdateDocument]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[procUpdateDocument] 
	(
	@GUID nvarchar(255),
	@LastErrorMsg nvarchar(255),
	@LastErrorXMLUrl nvarchar(255),
	@LastProgressMsg nvarchar(255),
	@LastProgressPercent nvarchar(255),
	@PDFUrl nvarchar(255),
	@ElapsedTimeInMilliSeconds Bigint,
	@State int,
	@Updated datetime
	)
	
AS
	UPDATE tblDocs
	SET LastErrorMsg = @LastErrorMsg, 
		LastErrorXMLUrl = @LastErrorXMLUrl,
		LastProgressMsg = @LastProgressMsg,
		LastProgressPercent = @LastProgressPercent,
		PDFUrl = @PDFUrl,
		ElapsedTimeInMilliSeconds = @ElapsedTimeInMilliSeconds,
		State = @State,
		Updated = @Updated
	WHERE  id = @GUID;  
	RETURN(1)

GO
/****** Object:  StoredProcedure [dbo].[procUpdateImage]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[procUpdateImage] 
	(
	@GUID nvarchar(255),
	@ErrorMessage nvarchar(255),
	@ElapsedTimeInMilliSeconds Bigint,
	@JPGFilename nvarchar(255),
	@Updated datetime,
	@Done int
	)
AS
	UPDATE tblImages
	SET ErrorMessage = @ErrorMessage, 
		ElapsedTimeInMilliSeconds = @ElapsedTimeInMilliSeconds,
		JPGFilename = @JPGFilename,
		Updated = @Updated,
		Done = @Done
	WHERE  id = @GUID;  
	RETURN (1)

GO
/****** Object:  StoredProcedure [dbo].[procUpdateJobOrder]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[procUpdateJobOrder]
	(
	@GUID nvarchar(255),
	@ElapsedTimeInSeconds Bigint
	)
AS
	UPDATE tblOrders
	SET JobNeededSeconds = @ElapsedTimeInSeconds
	WHERE  id = @GUID;  
	RETURN (1)

GO
/****** Object:  Table [dbo].[tblGenres]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblGenres](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[de-DE] [nvarchar](50) NULL,
	[en-EN] [nvarchar](50) NULL,
 CONSTRAINT [PK__tblGenres__7C8480AE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblImages]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblImages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[SmallImage] [image] NULL,
	[MediumImage] [image] NULL,
	[LargeImage] [image] NULL,
 CONSTRAINT [PK_tblImages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblItems]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[MediaTypeID] [int] NOT NULL,
	[MediaFormatID] [int] NOT NULL,
	[GenreID] [int] NOT NULL,
	[AuthorID] [int] NULL,
	[DirectorID] [int] NULL,
	[Actor1ID] [int] NULL,
	[Actor2ID] [int] NULL,
	[PublishDate] [nvarchar](50) NULL,
	[EAN] [nvarchar](13) NULL,
	[OwnerID] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Actor3ID] [int] NULL,
	[AmazonSalesRank] [int] NULL,
	[AudienceRank] [nvarchar](255) NULL,
	[BorrowedByID] [int] NULL,
	[BorrowedSince] [datetime] NULL,
	[ASIN] [nvarchar](50) NULL,
	[SmallImageUrl] [nvarchar](255) NULL,
	[MediumImageUrl] [nvarchar](255) NULL,
	[LargeImageUrl] [nvarchar](255) NULL,
	[BorrowCount] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[ImageID] [bigint] NULL,
	[GenreIDs] [nvarchar](200) NULL,
 CONSTRAINT [PK__tblItems__164452B1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblLents]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblLents](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NULL,
	[LentDate] [datetime] NULL,
	[LenderID] [int] NULL,
 CONSTRAINT [PK__tblLents__1273C1CD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblMediaFormat]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMediaFormat](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](1024) NULL,
 CONSTRAINT [PK__tblMediaFormat__0425A276] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblMediaType]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMediaType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](1024) NULL,
 CONSTRAINT [PK__tblMediaType__023D5A04] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblMovieItems]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMovieItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[MediaTypeID] [int] NOT NULL,
	[MediaFormatID] [int] NOT NULL,
	[GenreID] [int] NOT NULL,
	[AuthorID] [int] NULL,
	[DirectorID] [int] NULL,
	[Actor1ID] [int] NULL,
	[Actor2ID] [int] NULL,
	[PublishDate] [nvarchar](50) NULL,
	[EAN] [nvarchar](13) NULL,
	[OwnerID] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Actor3ID] [int] NULL,
	[AmazonSalesRank] [int] NULL,
	[AudienceRank] [nvarchar](255) NULL,
	[BorrowedByID] [int] NULL,
	[BorrowedSince] [datetime] NULL,
	[ASIN] [nvarchar](50) NULL,
	[SmallImageUrl] [nvarchar](255) NULL,
	[MediumImageUrl] [nvarchar](255) NULL,
	[LargeImageUrl] [nvarchar](255) NULL,
	[BorrowCount] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[ImageID] [bigint] NULL,
 CONSTRAINT [PK__tblMovieItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblParticipants]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblParticipants](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParticipantTypeID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](1024) NULL,
 CONSTRAINT [PK__tblParticipants__145C0A3F] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblParticipantType]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblParticipantType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](1024) NULL,
 CONSTRAINT [PK__tblParticipantTy__00551192] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblRatings]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRatings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Comment] [nvarchar](500) NULL,
	[Rating] [int] NOT NULL,
	[Subject] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblRatings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblRoles]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Rolename] [nvarchar](40) NOT NULL,
	[ApplicationName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblSettings]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Updated] [datetime] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Value] [nvarchar](2048) NULL,
 CONSTRAINT [PK_tblSettings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblUsers]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Password] [nvarchar](128) NOT NULL,
	[Username] [nvarchar](50) NULL,
	[Fon] [nvarchar](25) NULL,
	[ApplicationName] [nvarchar](255) NULL,
	[PasswordQuestion] [nvarchar](255) NULL,
	[PasswordAnswer] [nvarchar](255) NULL,
	[IsApproved] [bit] NULL,
	[IsOnLine] [bit] NULL,
	[IsLockedOut] [bit] NULL,
	[LastActivityDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
	[LastPasswordChangedDate] [datetime] NULL,
	[CreationDate] [datetime] NULL,
	[LastLockedOutDate] [datetime] NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL,
	[FailureCount] [int] NULL,
	[FailedPasswordAttemptCount] [int] NULL,
	[FailedPasswordAnswerAttemptCount] [int] NULL,
 CONSTRAINT [PK_tblUsers_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblUsersInRoles]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUsersInRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[RoleName] [nvarchar](40) NOT NULL,
	[ApplicationName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_UsersInRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[GetASingleItem]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[GetASingleItem]
AS
SELECT     dbo.tblItems.Name AS Title, dbo.tblMediaFormat.Name AS [Media Format], dbo.tblUsers.Firstname, dbo.tblUsers.Lastname, dbo.tblMediaType.Name AS [Media Type], 
                      dbo.tblGenres.Name AS Genre, tblParticipants_2.Name AS [Actor 2], tblParticipants_3.Name AS [Actor 3], tblParticipants_1.Name AS Director, 
                      dbo.tblParticipants.Name AS [Actor 1], dbo.tblItems.CoverUrl, tblParticipants_4.Name
FROM         dbo.tblGenres INNER JOIN
                      dbo.tblItems ON dbo.tblGenres.ID = dbo.tblItems.GenreID INNER JOIN
                      dbo.tblMediaFormat ON dbo.tblItems.MediaFormatID = dbo.tblMediaFormat.ID INNER JOIN
                      dbo.tblMediaType ON dbo.tblItems.MediaTypeID = dbo.tblMediaType.ID INNER JOIN
                      dbo.tblUsers ON dbo.tblItems.OwnerID = dbo.tblUsers.ID INNER JOIN
                      dbo.tblParticipants AS tblParticipants_1 ON dbo.tblItems.DirectorID = tblParticipants_1.ID LEFT OUTER JOIN
                      dbo.tblParticipants AS tblParticipants_2 ON dbo.tblItems.Actor2ID = tblParticipants_2.ID LEFT OUTER JOIN
                      dbo.tblParticipants AS tblParticipants_3 ON dbo.tblItems.Actor3ID = tblParticipants_3.ID LEFT OUTER JOIN
                      dbo.tblParticipants ON dbo.tblItems.Actor1ID = dbo.tblParticipants.ID LEFT OUTER JOIN
                      dbo.tblParticipants AS tblParticipants_4 ON dbo.tblItems.AuthorID = tblParticipants_4.ID
WHERE     (dbo.tblItems.ID = 8)

GO
/****** Object:  View [dbo].[viewItems]    Script Date: 11.01.2013 18:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[viewItems]
AS
SELECT     dbo.tblParticipants.Name AS Actor1, dbo.tblGenres.Name AS Title, dbo.tblUsers.Username, dbo.tblMediaFormat.Name AS MediaFormat, 
                      dbo.tblMediaType.Name AS MediaType, dbo.tblImages.SmallImage, dbo.tblImages.MediumImage, dbo.tblImages.LargeImage, dbo.tblItems.Description, 
                      dbo.tblItems.AmazonSalesRank, dbo.tblItems.AudienceRank, dbo.tblItems.PublishDate, dbo.tblItems.EAN, dbo.tblItems.BorrowCount, dbo.tblItems.DateAdded, 
                      dbo.tblItems.ASIN, dbo.tblItems.BorrowedSince, dbo.tblItems.BorrowedByID, tblParticipants_1.Name AS Director, tblParticipants_2.Name AS Actor3, dbo.tblItems.ID, 
                      dbo.tblItems.Name
FROM         dbo.tblGenres INNER JOIN
                      dbo.tblItems ON dbo.tblGenres.ID = dbo.tblItems.GenreID INNER JOIN
                      dbo.tblImages ON dbo.tblItems.ID = dbo.tblImages.ItemID INNER JOIN
                      dbo.tblMediaFormat ON dbo.tblItems.MediaFormatID = dbo.tblMediaFormat.ID INNER JOIN
                      dbo.tblMediaType ON dbo.tblItems.MediaTypeID = dbo.tblMediaType.ID INNER JOIN
                      dbo.tblParticipants ON dbo.tblItems.Actor1ID = dbo.tblParticipants.ID INNER JOIN
                      dbo.tblUsers ON dbo.tblItems.OwnerID = dbo.tblUsers.ID INNER JOIN
                      dbo.tblParticipants AS tblParticipants_1 ON dbo.tblItems.DirectorID = tblParticipants_1.ID INNER JOIN
                      dbo.tblParticipants AS tblParticipants_2 ON dbo.tblItems.Actor3ID = tblParticipants_2.ID

GO
ALTER TABLE [dbo].[tblItems] ADD  CONSTRAINT [DF_tblItems_BorrowedSince]  DEFAULT (((1)/(1))/(1970)) FOR [BorrowedSince]
GO
ALTER TABLE [dbo].[tblItems] ADD  CONSTRAINT [DF_tblItems_BorrowCount]  DEFAULT ((0)) FOR [BorrowCount]
GO
ALTER TABLE [dbo].[tblItems] ADD  CONSTRAINT [DF_tblItems_DateAdded]  DEFAULT (((1)/(1))/(1970)) FOR [DateAdded]
GO
ALTER TABLE [dbo].[tblItems] ADD  CONSTRAINT [DF_tblItems_ImageID]  DEFAULT ((0)) FOR [ImageID]
GO
ALTER TABLE [dbo].[tblMovieItems] ADD  CONSTRAINT [DF_tblMovieItems_BorrowedSince]  DEFAULT (((1)/(1))/(1970)) FOR [BorrowedSince]
GO
ALTER TABLE [dbo].[tblMovieItems] ADD  CONSTRAINT [DF_tblMovieItems_BorrowCount]  DEFAULT ((0)) FOR [BorrowCount]
GO
ALTER TABLE [dbo].[tblMovieItems] ADD  CONSTRAINT [DF_tblMovieItems_DateAdded]  DEFAULT (((1)/(1))/(1970)) FOR [DateAdded]
GO
ALTER TABLE [dbo].[tblMovieItems] ADD  CONSTRAINT [DF_tblMovieItems_ImageID]  DEFAULT ((0)) FOR [ImageID]
GO
ALTER TABLE [dbo].[tblRatings] ADD  CONSTRAINT [DF_tblRatings_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[tblSettings] ADD  CONSTRAINT [DF_tblSettings_Updated]  DEFAULT (getdate()) FOR [Updated]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblUsers_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblUsers_IsOnLine]  DEFAULT ((0)) FOR [IsOnLine]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblUsers_IsLockedOut]  DEFAULT ((0)) FOR [IsLockedOut]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblUsers_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblUsers_FailureCount]  DEFAULT ((0)) FOR [FailureCount]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblUsers_FailedPasswordAttemptCount]  DEFAULT ((0)) FOR [FailedPasswordAttemptCount]
GO
ALTER TABLE [dbo].[tblUsers] ADD  CONSTRAINT [DF_tblUsers_FailedPasswordAnswerAttemptCount]  DEFAULT ((0)) FOR [FailedPasswordAnswerAttemptCount]
GO
ALTER TABLE [dbo].[tblItems]  WITH CHECK ADD  CONSTRAINT [FK_tblItems_tblGenres] FOREIGN KEY([GenreID])
REFERENCES [dbo].[tblGenres] ([ID])
GO
ALTER TABLE [dbo].[tblItems] CHECK CONSTRAINT [FK_tblItems_tblGenres]
GO
ALTER TABLE [dbo].[tblItems]  WITH CHECK ADD  CONSTRAINT [FK_tblItems_tblMediaFormat] FOREIGN KEY([MediaFormatID])
REFERENCES [dbo].[tblMediaFormat] ([ID])
GO
ALTER TABLE [dbo].[tblItems] CHECK CONSTRAINT [FK_tblItems_tblMediaFormat]
GO
ALTER TABLE [dbo].[tblItems]  WITH CHECK ADD  CONSTRAINT [FK_tblItems_tblMediaType] FOREIGN KEY([MediaTypeID])
REFERENCES [dbo].[tblMediaType] ([ID])
GO
ALTER TABLE [dbo].[tblItems] CHECK CONSTRAINT [FK_tblItems_tblMediaType]
GO
ALTER TABLE [dbo].[tblItems]  WITH CHECK ADD  CONSTRAINT [FK_tblItems_tblParticipants] FOREIGN KEY([Actor1ID])
REFERENCES [dbo].[tblParticipants] ([ID])
GO
ALTER TABLE [dbo].[tblItems] CHECK CONSTRAINT [FK_tblItems_tblParticipants]
GO
ALTER TABLE [dbo].[tblItems]  WITH CHECK ADD  CONSTRAINT [FK_tblItems_tblParticipants1] FOREIGN KEY([Actor2ID])
REFERENCES [dbo].[tblParticipants] ([ID])
GO
ALTER TABLE [dbo].[tblItems] CHECK CONSTRAINT [FK_tblItems_tblParticipants1]
GO
ALTER TABLE [dbo].[tblItems]  WITH CHECK ADD  CONSTRAINT [FK_tblItems_tblParticipants2] FOREIGN KEY([Actor3ID])
REFERENCES [dbo].[tblParticipants] ([ID])
GO
ALTER TABLE [dbo].[tblItems] CHECK CONSTRAINT [FK_tblItems_tblParticipants2]
GO
ALTER TABLE [dbo].[tblItems]  WITH CHECK ADD  CONSTRAINT [FK_tblItems_tblParticipants3] FOREIGN KEY([DirectorID])
REFERENCES [dbo].[tblParticipants] ([ID])
GO
ALTER TABLE [dbo].[tblItems] CHECK CONSTRAINT [FK_tblItems_tblParticipants3]
GO
ALTER TABLE [dbo].[tblItems]  WITH CHECK ADD  CONSTRAINT [FK_tblItems_tblParticipants4] FOREIGN KEY([AuthorID])
REFERENCES [dbo].[tblParticipants] ([ID])
GO
ALTER TABLE [dbo].[tblItems] CHECK CONSTRAINT [FK_tblItems_tblParticipants4]
GO
ALTER TABLE [dbo].[tblItems]  WITH CHECK ADD  CONSTRAINT [FK_tblItems_tblUsers] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblItems] CHECK CONSTRAINT [FK_tblItems_tblUsers]
GO
ALTER TABLE [dbo].[tblLents]  WITH CHECK ADD  CONSTRAINT [FK_tblLents_tblItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[tblItems] ([ID])
GO
ALTER TABLE [dbo].[tblLents] CHECK CONSTRAINT [FK_tblLents_tblItems]
GO
ALTER TABLE [dbo].[tblMovieItems]  WITH CHECK ADD  CONSTRAINT [FK_tblMovieItems_tblGenres] FOREIGN KEY([GenreID])
REFERENCES [dbo].[tblGenres] ([ID])
GO
ALTER TABLE [dbo].[tblMovieItems] CHECK CONSTRAINT [FK_tblMovieItems_tblGenres]
GO
ALTER TABLE [dbo].[tblMovieItems]  WITH CHECK ADD  CONSTRAINT [FK_tblMovieItems_tblMediaFormat] FOREIGN KEY([MediaFormatID])
REFERENCES [dbo].[tblMediaFormat] ([ID])
GO
ALTER TABLE [dbo].[tblMovieItems] CHECK CONSTRAINT [FK_tblMovieItems_tblMediaFormat]
GO
ALTER TABLE [dbo].[tblMovieItems]  WITH CHECK ADD  CONSTRAINT [FK_tblMovieItems_tblMediaType] FOREIGN KEY([MediaTypeID])
REFERENCES [dbo].[tblMediaType] ([ID])
GO
ALTER TABLE [dbo].[tblMovieItems] CHECK CONSTRAINT [FK_tblMovieItems_tblMediaType]
GO
ALTER TABLE [dbo].[tblMovieItems]  WITH CHECK ADD  CONSTRAINT [FK_tblMovieItems_tblParticipants] FOREIGN KEY([Actor1ID])
REFERENCES [dbo].[tblParticipants] ([ID])
GO
ALTER TABLE [dbo].[tblMovieItems] CHECK CONSTRAINT [FK_tblMovieItems_tblParticipants]
GO
ALTER TABLE [dbo].[tblMovieItems]  WITH CHECK ADD  CONSTRAINT [FK_tblMovieItems_tblParticipants1] FOREIGN KEY([Actor2ID])
REFERENCES [dbo].[tblParticipants] ([ID])
GO
ALTER TABLE [dbo].[tblMovieItems] CHECK CONSTRAINT [FK_tblMovieItems_tblParticipants1]
GO
ALTER TABLE [dbo].[tblMovieItems]  WITH CHECK ADD  CONSTRAINT [FK_tblMovieItems_tblParticipants2] FOREIGN KEY([Actor3ID])
REFERENCES [dbo].[tblParticipants] ([ID])
GO
ALTER TABLE [dbo].[tblMovieItems] CHECK CONSTRAINT [FK_tblMovieItems_tblParticipants2]
GO
ALTER TABLE [dbo].[tblMovieItems]  WITH CHECK ADD  CONSTRAINT [FK_tblMovieItems_tblParticipants3] FOREIGN KEY([DirectorID])
REFERENCES [dbo].[tblParticipants] ([ID])
GO
ALTER TABLE [dbo].[tblMovieItems] CHECK CONSTRAINT [FK_tblMovieItems_tblParticipants3]
GO
ALTER TABLE [dbo].[tblMovieItems]  WITH CHECK ADD  CONSTRAINT [FK_tblMovieItems_tblParticipants4] FOREIGN KEY([AuthorID])
REFERENCES [dbo].[tblParticipants] ([ID])
GO
ALTER TABLE [dbo].[tblMovieItems] CHECK CONSTRAINT [FK_tblMovieItems_tblParticipants4]
GO
ALTER TABLE [dbo].[tblMovieItems]  WITH CHECK ADD  CONSTRAINT [FK_tblMovieItems_tblUsers] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblMovieItems] CHECK CONSTRAINT [FK_tblMovieItems_tblUsers]
GO
ALTER TABLE [dbo].[tblParticipants]  WITH CHECK ADD  CONSTRAINT [FK_tblParticipants_tblParticipantType] FOREIGN KEY([ParticipantTypeID])
REFERENCES [dbo].[tblParticipantType] ([ID])
GO
ALTER TABLE [dbo].[tblParticipants] CHECK CONSTRAINT [FK_tblParticipants_tblParticipantType]
GO
ALTER TABLE [dbo].[tblRatings]  WITH CHECK ADD  CONSTRAINT [FK_tblRatings_tblItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[tblItems] ([ID])
GO
ALTER TABLE [dbo].[tblRatings] CHECK CONSTRAINT [FK_tblRatings_tblItems]
GO
ALTER TABLE [dbo].[tblRatings]  WITH CHECK ADD  CONSTRAINT [FK_tblRatings_tblUsers] FOREIGN KEY([UserID])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblRatings] CHECK CONSTRAINT [FK_tblRatings_tblUsers]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[50] 4[21] 2[18] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tblGenres"
            Begin Extent = 
               Top = 237
               Left = 946
               Bottom = 339
               Right = 1106
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblItems"
            Begin Extent = 
               Top = 149
               Left = 477
               Bottom = 498
               Right = 653
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblMediaFormat"
            Begin Extent = 
               Top = 121
               Left = 949
               Bottom = 233
               Right = 1109
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblMediaType"
            Begin Extent = 
               Top = 2
               Left = 958
               Bottom = 118
               Right = 1118
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblUsers"
            Begin Extent = 
               Top = 341
               Left = 944
               Bottom = 491
               Right = 1104
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblParticipants_1"
            Begin Extent = 
               Top = 2
               Left = 8
               Bottom = 119
               Right = 183
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblParticipants_2"
            Begin Extent = 
               Top = 130
               Left = 6
               Bottom = 247
               Right = 181
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'GetASingleItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'   End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblParticipants_3"
            Begin Extent = 
               Top = 263
               Left = 5
               Bottom = 380
               Right = 180
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblParticipants"
            Begin Extent = 
               Top = 391
               Left = 10
               Bottom = 508
               Right = 185
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblParticipants_4"
            Begin Extent = 
               Top = 0
               Left = 369
               Bottom = 117
               Right = 544
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 28
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 3060
         Table = 2160
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'GetASingleItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'GetASingleItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[52] 4[14] 2[17] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tblGenres"
            Begin Extent = 
               Top = 13
               Left = 516
               Bottom = 130
               Right = 676
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblItems"
            Begin Extent = 
               Top = 9
               Left = 298
               Bottom = 441
               Right = 474
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblImages"
            Begin Extent = 
               Top = 33
               Left = 8
               Bottom = 174
               Right = 168
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblMediaFormat"
            Begin Extent = 
               Top = 327
               Left = 6
               Bottom = 441
               Right = 166
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblMediaType"
            Begin Extent = 
               Top = 185
               Left = 8
               Bottom = 321
               Right = 168
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblParticipants"
            Begin Extent = 
               Top = 182
               Left = 773
               Bottom = 334
               Right = 948
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblUsers"
            Begin Extent = 
               Top = 342
               Left = 880
               Bottom = 492
               Right = 1048
            End
       ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewItems'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'     DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblParticipants_1"
            Begin Extent = 
               Top = 38
               Left = 813
               Bottom = 157
               Right = 988
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tblParticipants_2"
            Begin Extent = 
               Top = 481
               Left = 722
               Bottom = 600
               Right = 897
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 23
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1590
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewItems'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'viewItems'
GO
