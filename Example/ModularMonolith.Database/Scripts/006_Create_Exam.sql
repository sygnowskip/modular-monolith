IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'exams' AND  TABLE_NAME = 'Exam')
BEGIN
	CREATE TABLE [exams].[Exam](
        [Id] [bigint] IDENTITY(1,1) NOT NULL,
        [LocationId] [bigint] NOT NULL,
        [SubjectId] [bigint] NOT NULL,
        [Capacity] [int] NOT NULL,
        [RegistrationStartDate] [date] NOT NULL,
        [RegistrationEndDate] [date] NOT NULL,
        [ExamDateTime] [datetime2](7) NOT NULL,
        [Status] [nvarchar](30) NOT NULL,
     CONSTRAINT [PK_Exam] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    CONSTRAINT [FK_Exam_Subject] FOREIGN KEY ([SubjectId]) REFERENCES [read].[Subject]([Id]),
    CONSTRAINT [FK_Exam_Location] FOREIGN KEY ([LocationId]) REFERENCES [read].[Location]([Id])
    ) ON [PRIMARY]
END