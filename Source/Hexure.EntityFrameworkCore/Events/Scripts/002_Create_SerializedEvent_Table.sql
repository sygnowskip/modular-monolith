SET ANSI_NULLS ON
IF NOT EXISTS(
	SELECT * FROM INFORMATION_SCHEMA.TABLES 
	WHERE TABLE_SCHEMA = 'events' AND  TABLE_NAME = 'SerializedEvent')
BEGIN
	CREATE TABLE [events].[SerializedEvent](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[SerializedEventNamespace] [nvarchar](max) NOT NULL,
		[SerializedEventType] [nvarchar](max) NOT NULL,
		[SerializedEventPayload] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_Serialized] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO