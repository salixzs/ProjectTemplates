CREATE TABLE [dbo].[SystemNotifications]
(
	[Id] INT NOT NULL IDENTITY (1000, 1),
    [StartTime] DATETIME NOT NULL, 
    [EndTime] DATETIME NOT NULL, 
    [Type] TINYINT NOT NULL DEFAULT 0, 
    [EmphasizeSince] DATETIME NOT NULL, 
    [EmphasizeType] TINYINT NOT NULL DEFAULT 0, 
    [CountdownSince] DATETIME NOT NULL, 
    CONSTRAINT [PK_SystemNotifications] PRIMARY KEY CLUSTERED ([Id] ASC),
)
