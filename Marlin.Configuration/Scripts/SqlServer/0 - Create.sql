USE [Marlin]
GO
/****** Object:  Table [dbo].[Authorization]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authorization](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Authorization] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Credential]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Credential](
	[UserId] [uniqueidentifier] NOT NULL,
	[Value] [varchar](200) NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Password] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language](
	[Id] [varchar](10) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resource]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resource](
	[Id] [uniqueidentifier] NOT NULL,
	[Url] [varchar](200) NOT NULL,
	[Method] [varchar](20) NOT NULL,
	[IsPublic] [bit] NOT NULL,
	[Order] [int] NULL,
	[Title] [varchar](50) NULL,
	[Label] [varchar](50) NULL,
	[ParentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_Resource] UNIQUE NONCLUSTERED 
(
	[Url] ASC,
	[Method] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceRole]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceRole](
	[RoleId] [uniqueidentifier] NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ResourceRole] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[ResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_Role] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trace]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trace](
	[ClassName] [varchar](200) NOT NULL,
	[ClientIp] [varchar](200) NOT NULL,
	[Hostname] [varchar](200) NOT NULL,
	[Url] [varchar](200) NOT NULL,
	[Method] [varchar](20) NOT NULL,
	[Payload] [varchar](max) NULL,
	[Query] [varchar](max) NULL,
	[Username] [varchar](200) NULL,
	[Created] [datetime] NOT NULL,
	[Millis] [int] NOT NULL,
	[Error] [varchar](max) NULL,
	[Message] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Translation]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Translation](
	[LanguageId] [varchar](10) NOT NULL,
	[Original] [varchar](200) NOT NULL,
	[Translated] [varchar](200) NOT NULL,
 CONSTRAINT [PK_Translation] PRIMARY KEY CLUSTERED 
(
	[LanguageId] ASC,
	[Original] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [varchar](200) NOT NULL,
	[Mobile] [bigint] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Disabled] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_User] UNIQUE NONCLUSTERED 
(
	[Mobile] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_User_1] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProperty]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProperty](
	[UserId] [uniqueidentifier] NOT NULL,
	[Key] [varchar](50) NOT NULL,
	[Value] [varchar](max) NOT NULL,
 CONSTRAINT [PK_UserProperty] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Credential]  WITH CHECK ADD  CONSTRAINT [FK_Password_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Credential] CHECK CONSTRAINT [FK_Password_User]
GO
ALTER TABLE [dbo].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_Resource] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Resource] ([Id])
GO
ALTER TABLE [dbo].[Resource] CHECK CONSTRAINT [FK_Resource_Resource]
GO
ALTER TABLE [dbo].[ResourceRole]  WITH CHECK ADD  CONSTRAINT [FK_ResourceRole_Resource] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[Resource] ([Id])
GO
ALTER TABLE [dbo].[ResourceRole] CHECK CONSTRAINT [FK_ResourceRole_Resource]
GO
ALTER TABLE [dbo].[ResourceRole]  WITH CHECK ADD  CONSTRAINT [FK_ResourceRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[ResourceRole] CHECK CONSTRAINT [FK_ResourceRole_Role]
GO
ALTER TABLE [dbo].[Translation]  WITH CHECK ADD  CONSTRAINT [FK_Translation_Language] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
GO
ALTER TABLE [dbo].[Translation] CHECK CONSTRAINT [FK_Translation_Language]
GO
ALTER TABLE [dbo].[UserProperty]  WITH CHECK ADD  CONSTRAINT [FK_UserProperty_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserProperty] CHECK CONSTRAINT [FK_UserProperty_User]
GO
/****** Object:  StoredProcedure [dbo].[Marlin_CredentialAdd]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_CredentialAdd]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier,
	@value varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

	DELETE FROM dbo.[Credential] WHERE [UserId] = @user;

	INSERT INTO dbo.[Credential] (UserId, [Value], [Created]) VALUES (@user, @value , GETDATE());

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_CredentialGet]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_CredentialGet]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier,
	@value varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @value IS NOT NULL
	SELECT UserId, [Value] , [Created] FROM [dbo].[Credential] WHERE [UserId] = @user AND [Value] = @value
	ELSE
   	SELECT [Value] FROM [dbo].[Credential] WHERE [UserId] = @user

END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_LanguageAdd]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_LanguageAdd]
	-- Add the parameters for the stored procedure here
	@language varchar(10),
	@name varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

    INSERT INTO [dbo].[Language] (Id, [Name]) VALUES (@language, @name);

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_LanguageDelete]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_LanguageDelete]
	-- Add the parameters for the stored procedure here
	@language varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

	DELETE FROM dbo.[Translation] WHERE LanguageId = @language;
	DELETE FROM dbo.[Language] WHERE [Id] = @language;

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_LanguageGet]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_LanguageGet]
	-- Add the parameters for the stored procedure here
	@language varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
	l.Id,
	l.[Name],
	t.Original,
	t.Translated
	FROM dbo.[Language] l
	LEFT JOIN dbo.[Translation] t ON t.LanguageId = l.Id
	WHERE l.Id = @language
	ORDER BY t.Original 
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_LanguageUpdate]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_LanguageUpdate]
	-- Add the parameters for the stored procedure here
	@language varchar(10),
	@name varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

	UPDATE dbo.[Language] SET [Name] = @name WHERE [Id] = @language;

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_ResourceAdd]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_ResourceAdd]
	-- Add the parameters for the stored procedure here
	@url varchar(200),
	@method varchar(20),
	@isPublic bit,
	@order int,
	@title varchar(50),
	@label varchar(50),
	@parent uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
		INSERT INTO [dbo].[Resource]
           ([Id]
           ,[Url]
           ,[Method]
           ,[IsPublic]
           ,[Order]
           ,[Title]
           ,[Label]
           ,[ParentId])
		VALUES
           (NEWID()
           ,@url
           ,@method
           ,@isPublic
           ,@order
           ,@title
           ,@label
           ,@parent)



	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_ResourceDelete]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_ResourceDelete]
	-- Add the parameters for the stored procedure here
	@resource uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
		DELETE FROM dbo.ResourceRole WHERE ResourceId = @resource;
		DELETE FROM dbo.[Resource] WHERE Id = @resource;
	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_ResourceGet]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_ResourceGet]
(
	@url varchar(200),
	@method varchar(20)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SELECT [Id]
      ,[Url]
      ,[Method]
      ,[IsPublic]
	  ,[Order]
      ,[Title]
      ,[Label]
      ,[ParentId]
	FROM [dbo].[Resource]
	WHERE [Url] = @url AND [Method] = @method



END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_ResourceIsPublic]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_ResourceIsPublic]
	-- Add the parameters for the stored procedure here
(
	@url varchar(200),
	@method varchar(20)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT IsPublic FROM dbo.[Resource] WHERE [Url] = @url AND [Method] = @method;
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_ResourceList]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_ResourceList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SELECT [Id]
      ,[Url]
      ,[Method]
      ,[IsPublic]
	  ,[Order]
      ,[Title]
      ,[Label]
      ,[ParentId]
	FROM [dbo].[Resource]
	ORDER BY [Order]



END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_ResourceUpdate]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_ResourceUpdate]
	-- Add the parameters for the stored procedure here
	@resource uniqueidentifier,
	@url varchar(200),
	@method varchar(20),
	@isPublic bit,
	@order int,
	@title varchar(50),
	@label varchar(50),
	@parent uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
		UPDATE [dbo].[Resource] SET [Url] = @url, [Method] = @method, [IsPublic] = @isPublic, [Order] = @order, [Title] = @title, [Label] = @label, ParentId = @parent WHERE Id = @resource
	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_RoleAdd]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_RoleAdd]
	-- Add the parameters for the stored procedure here
	@name varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
	IF EXISTS(SELECT 1 FROM [dbo].[Role] WHERE [Name] = @name)
	   THROW 66666, 'Role already registered', 1;  

    INSERT INTO [dbo].[Role] (Id, [Name]) VALUES (NEWID(), @name);

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_RoleAddAssembly]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_RoleAddAssembly]
	-- Add the parameters for the stored procedure here
	@role uniqueidentifier,
	@assembly uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
	IF NOT EXISTS(SELECT 1 FROM dbo.[AssemblyRole] WHERE RoleId = @role AND AssemblyId = @assembly)
		INSERT INTO [dbo].[AssemblyRole] (RoleId, AssemblyId) VALUES (@role, @assembly);

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_RoleAddResource]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_RoleAddResource]
	-- Add the parameters for the stored procedure here
	@role uniqueidentifier,
	@resource uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
	IF NOT EXISTS(SELECT 1 FROM dbo.[ResourceRole] WHERE RoleId = @role AND ResourceId = @resource)
		INSERT INTO [dbo].[ResourceRole] (RoleId, ResourceId) VALUES (@role, @resource);

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_RoleDelete]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_RoleDelete]
	-- Add the parameters for the stored procedure here
	@role uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
		DELETE FROM dbo.[AssemblyRole] WHERE RoleId = @role;
		DELETE FROM dbo.[ResourceRole] WHERE RoleId = @role;
		DELETE FROM dbo.[Authorization] WHERE RoleId = @role;
		DELETE FROM dbo.[Role] WHERE Id = @role;
	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_RoleGet]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_RoleGet]
	-- Add the parameters for the stored procedure here
	@role varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF TRY_CONVERT(uniqueidentifier, @role) IS NOT NULL
    SELECT 
	r.Id as RoleId, 
	r.[Name] as [Role],
	rs.Id as ResourceId,
	rs.IsPublic,
	rs.[Label],
	rs.[Method],
	rs.ParentId,
	rs.[Title],
	rs.[Url],
	rs.[Order]
	FROM dbo.[Role] r
	LEFT JOIN dbo.[ResourceRole] rr ON rr.RoleId = r.Id
	LEFT JOIN dbo.[Resource] rs ON rs.Id = rr.ResourceId
	WHERE r.[Id] = @role
	ORDER BY rs.[Order]
	ELSE
	    SELECT 
	r.Id as RoleId, 
	r.[Name] as [Role],
	rs.Id as ResourceId,
	rs.IsPublic,
	rs.[Label],
	rs.[Method],
	rs.ParentId,
	rs.[Title],
	rs.[Url],
	rs.[Order]
	FROM dbo.[Role] r
	LEFT JOIN dbo.[ResourceRole] rr ON rr.RoleId = r.Id
	LEFT JOIN dbo.[Resource] rs ON rs.Id = rr.ResourceId
	WHERE r.[Name] = @role
	ORDER BY rs.[Order]
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_RoleList]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_RoleList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
	r.Id as RoleId, 
	r.[Name] as [Role],
	rs.Id as ResourceId,
	rs.IsPublic,
	rs.[Label],
	rs.[Method],
	rs.ParentId,
	rs.[Title],
	rs.[Url],
	rs.[Order]
	FROM dbo.[Role] r
	LEFT JOIN dbo.[ResourceRole] rr ON rr.RoleId = r.Id
	LEFT JOIN dbo.[Resource] rs ON rs.Id = rr.ResourceId
	ORDER BY r.[Name], rs.[Order]
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_RoleUpdate]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_RoleUpdate]
	-- Add the parameters for the stored procedure here
	@role uniqueidentifier,
	@name varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
	UPDATE [dbo].[Role] SET [Name] = @name WHERE [Id] = @role;
	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_TraceWrite]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_TraceWrite]
	-- Add the parameters for the stored procedure here
	@className varchar(200),
	@method varchar(200),
	@hostname varchar(200),
	@clientip varchar(200),
	@username varchar(200),
	@url varchar(200),
	@error varchar(max),
	@message varchar(max),
	@millis int,
	@payload varchar(max),
	@query varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

INSERT INTO [dbo].[Trace]
           ([Created]
           ,[ClassName]
           ,[Method]
           ,[Hostname]
           ,[ClientIP]
           ,[Username]
           ,[Url]
           ,[Error]
           ,[Millis]
		   ,[Payload]
		   ,[Query]
		   ,[Message])
     VALUES
           (GETDATE()
           ,@className
           ,@method
           ,@hostname
           ,@clientip
           ,@username
           ,@url
           ,@error
           ,@millis
		   ,@payload
		   ,@query
		   ,@message);
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_TranslationAdd]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_TranslationAdd]
	-- Add the parameters for the stored procedure here
	@language varchar(10),
	@original varchar(200),
	@translated varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

	INSERT INTO dbo.[Translation] (LanguageId, Original, Translated) VALUES (@language, @original, @translated);

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_TranslationDelete]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_TranslationDelete]
	-- Add the parameters for the stored procedure here
	@language varchar(10),
	@original varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

	DELETE FROM dbo.[Translation] WHERE LanguageId = @language AND Original = @original

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_TranslationGet]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_TranslationGet]
	-- Add the parameters for the stored procedure here
	@language varchar(10),
	@original varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT LanguageId, Original, Translated FROM dbo.Translation WHERE LanguageId = @language AND Original = @original
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_TranslationUpdate]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_TranslationUpdate]
	-- Add the parameters for the stored procedure here
	@language varchar(10),
	@original varchar(200),
	@translated varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

	UPDATE dbo.[Translation] SET Translated = @translated WHERE LanguageId = @language AND Original = @original

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserAdd]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserAdd]
	-- Add the parameters for the stored procedure here
	@username varchar(200),
	@mobile bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

    INSERT INTO [dbo].[User] (Id, Username, Mobile, Created) VALUES (NEWID(), @username, @mobile, GETDATE());

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserAuthorize]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserAuthorize]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier,
	@role uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
	IF NOT EXISTS(SELECT 1 FROM [dbo].[Authorization] WHERE [UserId] = @user AND [RoleId] = @role)
	   INSERT INTO dbo.[Authorization] (UserId, RoleId, Created) VALUES (@user, @role, GETDATE());

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserDelete]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserDelete]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

	DELETE FROM [dbo].[Authorization] WHERE UserId = @user;
	DELETE FROM [dbo].[Credential] WHERE UserId = @user;
	DELETE FROM [dbo].[UserProperty] WHERE UserId = @user;
	DELETE FROM [dbo].[User] WHERE Id = @user;

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserDisable]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserDisable]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

	UPDATE dbo.[User] SET [Disabled] = GETDATE() WHERE Id = @user;

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserEnable]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserEnable]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

	UPDATE dbo.[User] SET [Disabled] = NULL WHERE Id = @user;

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserGet]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserGet]
	-- Add the parameters for the stored procedure here
	@user varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF TRY_CONVERT(UNIQUEIDENTIFIER, @user) IS NOT NULL
	SELECT
	u.Id,
	u.Username,
	u.Mobile,
	u.Created,
	u.[Disabled],
	r.[Name] as [Role],
	up.[Key],
	up.[Value]
	FROM [dbo].[User] u
	JOIN [dbo].[Authorization] a ON a.UserId = u.Id
	JOIN [dbo].[Role] r ON r.Id = a.RoleId
	LEFT JOIN [dbo].UserProperty up ON up.UserId = u.Id
	WHERE u.Id = @user
	ELSE
	SELECT
	u.Id,
	u.Username,
	u.Mobile,
	u.Created,
	u.[Disabled],
	r.[Name] as [Role],
	up.[Key],
	up.[Value]
	FROM [dbo].[User] u
	JOIN [dbo].[Authorization] a ON a.UserId = u.Id
	JOIN [dbo].[Role] r ON r.Id = a.RoleId
	LEFT JOIN [dbo].UserProperty up ON up.UserId = u.Id
	WHERE u.Username = @user
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserPropertyAdd]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserPropertyAdd]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier,
	@key varchar(50),
	@value varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
	 
	IF NOT EXISTS(SELECT 1 FROM dbo.[UserProperty] WHERE UserId = @user AND [Key] = @key)
			INSERT INTO dbo.[UserProperty] (UserId, [Key], [Value]) VALUES (@user, @key, @value)

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserPropertyDelete]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserPropertyDelete]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier,
	@key varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
		DELETE FROM dbo.[UserProperty] WHERE [UserId] = @user AND [Key] = @key 
	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserPropertyGet]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserPropertyGet]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier,
	@key varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Value] FROM dbo.UserProperty WHERE UserId = @user AND [Key] = @key
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserPropertyUpdate]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserPropertyUpdate]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier,
	@key varchar(50),
	@value varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY
	 
	IF EXISTS(SELECT 1 FROM dbo.[UserProperty] WHERE UserId = @user AND [Key] = @key)
			UPDATE dbo.[UserProperty] SET [Value] = @value WHERE [UserId] = @user AND [Key] = @key 
	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserResources]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserResources]
	-- Add the parameters for the stored procedure here
	@user varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF TRY_CONVERT(UNIQUEIDENTIFIER, @user) IS NOT NULL
	SELECT
    rs.Id,
	rs.IsPublic,
	rs.[Label],
	rs.[Method],
	rs.ParentId,
	rs.[Order],
	rs.Title,
	rs.[Url]
	FROM [dbo].[User] u
	JOIN [dbo].[Authorization] a ON a.UserId = u.Id
	JOIN [dbo].[Role] r ON r.Id = a.RoleId
	JOIN [dbo].[ResourceRole] rr ON rr.RoleId = r.Id
	JOIN [dbo].[Resource] rs ON rs.Id = rr.ResourceId
	WHERE u.Id = @user AND u.[Disabled] IS NULL
	ORDER BY rs.[Order]
	ELSE
	SELECT
    rs.Id,
	rs.IsPublic,
	rs.[Label],
	rs.[Method],
	rs.ParentId,
	rs.[Order],
	rs.Title,
	rs.[Url]
	FROM [dbo].[User] u
	JOIN [dbo].[Authorization] a ON a.UserId = u.Id
	JOIN [dbo].[Role] r ON r.Id = a.RoleId
	JOIN [dbo].[ResourceRole] rr ON rr.RoleId = r.Id
	JOIN [dbo].[Resource] rs ON rs.Id = rr.ResourceId
	WHERE u.Username = @user AND u.[Disabled] IS NULL
	ORDER BY rs.[Order]
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserSearch]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserSearch]
	-- Add the parameters for the stored procedure here
	@query varchar(200),
	@page int,
	@limit int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
	u.Id,
	u.Username,
	u.Mobile,
	u.Created,
	u.[Disabled],
	up.[Key],
	up.[Value]
	FROM [dbo].[User] u
	LEFT JOIN [dbo].UserProperty up ON up.UserId = u.Id
	WHERE @query is null OR (u.Username LIKE '%' + @query + '%' OR u.Mobile LIKE '%' + @query + '%' OR up.[Value] LIKE '%' + @query + '%')
	ORDER BY u.Username
	OFFSET (@page-1)*@limit ROWS
	FETCH NEXT @limit ROWS ONLY
END
GO
/****** Object:  StoredProcedure [dbo].[Marlin_UserUnauthorize]    Script Date: 9/23/2021 5:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Marlin_UserUnauthorize]
	-- Add the parameters for the stored procedure here
	@user uniqueidentifier,
	@role uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRANSACTION; 
	BEGIN TRY

	DELETE FROM dbo.[Authorization] WHERE UserId = @user AND RoleId = @role;

	END TRY
	BEGIN CATCH  
		declare @errorNumber int;
		declare @errorMessage nvarchar(max);
		declare @errorState int;

		set @errorNumber = (SELECT ERROR_NUMBER());
		set @errorMessage = (SELECT ERROR_MESSAGE());
		set @errorState = (SELECT ERROR_STATE());

		IF @@TRANCOUNT > 0  
			ROLLBACK TRANSACTION;  
		THROW @errorNumber, @errorMessage, @errorState;
	END CATCH;

IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END
GO
