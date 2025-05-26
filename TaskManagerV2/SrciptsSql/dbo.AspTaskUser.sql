CREATE TABLE [dbo].[AspTaskUser]
(
	[TaskId] NVARCHAR(MAX) NOT NULL PRIMARY KEY, 
    [TaskName] VARCHAR(MAX) NOT NULL, 
    [TaskDesc] VARCHAR(MAX) NOT NULL, 
    [StartDate] DATETIME NOT NULL, 
    [FinalDate] DATETIME NOT NULL, 
    [Local] VARCHAR(MAX) NOT NULL, 
    [UserNameTask] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_Tasks_ToUsers] FOREIGN KEY ([UserNameTask]) REFERENCES [AspNetUsers]([UserName])
)
