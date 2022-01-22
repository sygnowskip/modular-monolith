IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'registrations' AND  TABLE_NAME = 'Registration')
BEGIN
	CREATE TABLE [registrations].[Registration](
		[Id] bigint IDENTITY(1,1) NOT NULL,
		[ExamId] bigint NOT NULL,
		[OrderId] bigint NOT NULL,
        [ExternalId] UNIQUEIDENTIFIER NOT NULL,
		[Status] NVARCHAR(30) NOT NULL,
		[CandidateFirstName] NVARCHAR(MAX) NOT NULL,
		[CandidateLastName] NVARCHAR(MAX) NOT NULL,
		[CandidateDateOfBirth] DATE NOT NULL,
	    
	 CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_Registration_Exam] FOREIGN KEY ([ExamId]) REFERENCES [exams].[Exam]([Id]),
    CONSTRAINT [FK_Registration_Order] FOREIGN KEY ([OrderId]) REFERENCES [orders].[Order]([Id]),
	) ON [PRIMARY]
END