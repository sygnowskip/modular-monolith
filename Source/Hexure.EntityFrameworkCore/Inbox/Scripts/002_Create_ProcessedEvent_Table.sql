IF NOT EXISTS(
	SELECT * FROM INFORMATION_SCHEMA.TABLES 
	WHERE TABLE_SCHEMA = 'events' AND  TABLE_NAME = 'ProcessedEvent')
BEGIN
	CREATE TABLE [events].[ProcessedEvent](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[MessageId] [uniqueidentifier] NOT NULL,
		[Consumer] [nvarchar](32) NOT NULL,
		[ProcessedOn] [datetime2](7) NOT NULL,
	 CONSTRAINT [PK_Serialized] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

    CREATE UNIQUE INDEX [UQ_ProcessedEvent_MessageId_Consumer]   
    ON [events].[ProcessedEvent] (MessageId, Consumer);
END
GO