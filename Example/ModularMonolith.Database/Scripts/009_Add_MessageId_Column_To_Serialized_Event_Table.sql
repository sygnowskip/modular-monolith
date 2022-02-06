IF NOT EXISTS (
  SELECT *
  FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'SerializedEvent' and TABLE_SCHEMA = 'events' AND COLUMN_NAME = 'MessageId'
)
BEGIN
    ALTER TABLE [events].[SerializedEvent]
    ADD MessageId UNIQUEIDENTIFIER NOT NULL 
    CONSTRAINT [DF_MessageId_SerializedEvent] DEFAULT NEWID()
    WITH VALUES
END