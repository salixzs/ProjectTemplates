CREATE TABLE [dbo].[SystemFeedbackComments]
(
    [Id] INT NOT NULL IDENTITY (10000, 1),
    [Content] NVARCHAR(MAX) NOT NULL,
    [CreatedAt] DATETIMEOFFSET(0) NOT NULL,
    [SystemFeedbackId] INT NOT NULL,
    CONSTRAINT [PK_SystemFeedbackComments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SystemFeedbackComment_SystemFeedback] FOREIGN KEY ([SystemFeedbackId]) REFERENCES [dbo].[SystemFeedbacks] ([Id]) ON DELETE CASCADE
)
GO

CREATE NONCLUSTERED INDEX [IDX_SystemFeedbackComments_SystemFeedbacks]
    ON [dbo].[SystemFeedbackComments]([SystemFeedbackId] ASC);
