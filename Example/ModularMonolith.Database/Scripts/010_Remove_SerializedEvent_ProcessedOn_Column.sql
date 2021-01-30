IF EXISTS (
  SELECT *
  FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'SerializedEvent' and TABLE_SCHEMA = 'events' AND COLUMN_NAME = 'ProcessedOn'
)
BEGIN
    ALTER TABLE [events].[SerializedEvent]
    DROP COLUMN ProcessedOn
END
GO