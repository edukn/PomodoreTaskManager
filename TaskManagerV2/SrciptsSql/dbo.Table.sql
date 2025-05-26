CREATE TABLE [dbo].[AspTaskUser]
(
	[TaskId] NVARCHAR(MAX) NOT NULL PRIMARY KEY, 
    [TaskName] VARCHAR(MAX) NOT NULL, 
    [TaskDesc] VARCHAR(MAX) NOT NULL, 
    [StartDate] DATETIME NOT NULL, 
    [FinalDate] DATETIME NOT NULL, 
    [Local] VARCHAR(MAX) NOT NULL, 
    [UserId] NVARCHAR(128) NOT NULL, 
    CONSTRAINT [FK_Tasks_ToUsers] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id])
)
