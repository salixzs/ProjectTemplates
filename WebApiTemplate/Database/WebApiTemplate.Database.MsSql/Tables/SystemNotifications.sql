CREATE TABLE [dbo].[SystemNotifications]
(
	[Id] INT NOT NULL IDENTITY (1000, 1),
    [StartTime] DATETIMEOFFSET(0) NOT NULL, 
    [EndTime] DATETIMEOFFSET(0) NOT NULL, 
    [Type] TINYINT NOT NULL DEFAULT 0, 
    [EmphasizeSince] DATETIMEOFFSET(0) NOT NULL, 
    [EmphasizeType] TINYINT NOT NULL DEFAULT 0, 
    [CountdownSince] DATETIMEOFFSET(0) NOT NULL,
    [MoreInfoUrl] VARCHAR(1000) NULL,
    [IsHealthCheck] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_SystemNotifications] PRIMARY KEY CLUSTERED ([Id] ASC),
)
