-- Database Creation
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TaskManager')
BEGIN
    CREATE DATABASE TaskManager;
END
GO

USE TaskManager;
GO

-- Users Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users] (
        [UserId] INT PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [IsManager] BIT NOT NULL DEFAULT 0
    )
END
GO

-- Projects Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Projects] (
        [ProjectId] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500),
        [CreatedBy] INT NOT NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Projects_Users FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
    )
END
GO

-- Tasks Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tasks]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Tasks] (
        [TaskId] INT IDENTITY(1,1) PRIMARY KEY,
        [ProjectId] INT NOT NULL,
        [Title] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500),
        [DueDate] DATETIME2,
        [Status] NVARCHAR(20) NOT NULL CHECK (Status IN ('Pending', 'InProgress', 'Completed')),
        [Priority] NVARCHAR(10) NOT NULL CHECK (Priority IN ('Low', 'Medium', 'High')),
        [CreatedBy] INT NOT NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Tasks_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId),
        CONSTRAINT FK_Tasks_Users FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
    )
END
GO

-- Task History Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaskHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[TaskHistory] (
        [HistoryId] INT IDENTITY(1,1) PRIMARY KEY,
        [TaskId] INT NOT NULL,
        [FieldChanged] NVARCHAR(50) NOT NULL,
        [ChangedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [ChangedBy] INT NOT NULL,
        [Comment] NVARCHAR(500),
        CONSTRAINT FK_TaskHistory_Tasks FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId),
        CONSTRAINT FK_TaskHistory_Users FOREIGN KEY (ChangedBy) REFERENCES Users(UserId)
    )
END
GO

-- Insert default users
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users])
BEGIN
    INSERT INTO [dbo].[Users] (UserId, Name, IsManager) VALUES 
    (1, 'Ivan Prado', 1),
    (2, 'Gisele Tejada', 0),
    (3, 'Vitor de Oliveira', 0),
    (4, 'Cibele Marcondes', 1);
END
GO

-- Create index for better performance
CREATE INDEX IX_Tasks_ProjectId ON Tasks(ProjectId);
CREATE INDEX IX_TaskHistory_TaskId ON TaskHistory(TaskId);
GO