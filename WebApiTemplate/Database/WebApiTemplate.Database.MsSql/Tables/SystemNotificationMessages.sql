CREATE TABLE [dbo].[SystemNotificationMessages]
(
	[Id] INT NOT NULL IDENTITY (1000, 1),
    [SystemNotificationId] INT NOT NULL,
    [LanguageCode] CHAR(2) NOT NULL, 
    [Message] NVARCHAR(2000) NOT NULL,
    CONSTRAINT [PK_SystemNotificationMessages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SystemNotificationMessage_SystemNotification] FOREIGN KEY ([SystemNotificationId]) REFERENCES [dbo].[SystemNotifications] ([Id]) ON DELETE CASCADE
)
GO

CREATE NONCLUSTERED INDEX [IDX_SystemNotificationMessages_SystemNotification]
    ON [dbo].[SystemNotificationMessages]([SystemNotificationId] ASC);
