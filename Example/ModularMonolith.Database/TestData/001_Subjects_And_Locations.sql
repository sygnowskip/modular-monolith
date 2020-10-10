IF 
EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'read' AND  TABLE_NAME = 'Subject') AND
NOT EXISTS(SELECT * FROM [read].[Subject] WHERE Id = 1)
BEGIN
	SET IDENTITY_INSERT [read].[Subject] ON 
    INSERT [read].[Subject] ([Id], [Name]) VALUES (1, N'Mathematics')
    INSERT [read].[Subject] ([Id], [Name]) VALUES (2, N'Physics')
    INSERT [read].[Subject] ([Id], [Name]) VALUES (3, N'Chemistry')
    INSERT [read].[Subject] ([Id], [Name]) VALUES (4, N'History')
    INSERT [read].[Subject] ([Id], [Name]) VALUES (5, N'Biology')
    SET IDENTITY_INSERT [read].[Subject] OFF
END

IF 
EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'read' AND  TABLE_NAME = 'Location') AND
NOT EXISTS(SELECT * FROM [read].[Location] WHERE Id = 1)
BEGIN
	SET IDENTITY_INSERT [read].[Location] ON 
    INSERT [read].[Location] ([Id], [Name]) VALUES (1, N'Warsaw')
    INSERT [read].[Location] ([Id], [Name]) VALUES (2, N'Madrid')
    INSERT [read].[Location] ([Id], [Name]) VALUES (3, N'London')
    INSERT [read].[Location] ([Id], [Name]) VALUES (4, N'Munich')
    INSERT [read].[Location] ([Id], [Name]) VALUES (5, N'Paris')
    SET IDENTITY_INSERT [read].[Location] OFF
END