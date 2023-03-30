CREATE TABLE [dbo].[SystemFeedbacks]
(
    [Id] INT NOT NULL IDENTITY (1000, 1),
    [Title] NVARCHAR(200) NOT NULL,
    [Content] NVARCHAR(MAX) NULL,
    [SystemInfo] NVARCHAR(MAX) NULL,
    [Category] INT NOT NULL,
    [Status] INT NOT NULL,
    [Priority] INT NOT NULL,
    [CreatedAt] DATETIMEOFFSET(0) NOT NULL,
    [ModifiedAt] DATETIMEOFFSET(0) NOT NULL,
    [CompletedAt] DATETIMEOFFSET(0) NULL,
    CONSTRAINT [PK_SystemFeedbacks] PRIMARY KEY CLUSTERED ([Id] ASC),
)
