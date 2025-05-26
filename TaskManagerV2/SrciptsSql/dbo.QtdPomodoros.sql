CREATE TABLE [dbo].[QtdPomodoros] (
    [Id]           INT            NULL,
    [UserId]       NVARCHAR (MAX) NOT NULL,
    [Data]         DATE           NOT NULL,
    [QtdPomodoros] INT            NULL 
	CONSTRAINT QtdPomodoros_key PRIMARY KEY (UserId, Data)
);

