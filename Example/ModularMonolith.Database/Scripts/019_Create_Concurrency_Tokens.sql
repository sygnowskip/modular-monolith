IF NOT EXISTS (
  SELECT *
  FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'Registration' and TABLE_SCHEMA = 'registrations' AND COLUMN_NAME = 'DomainTimestamp'
)
BEGIN
    ALTER TABLE [registrations].[Registration]
    ADD DomainTimestamp DATETIME2 NOT NULL
    CONSTRAINT [DF_Registration_DomainTimestamp] DEFAULT GETUTCDATE()

    ALTER TABLE [registrations].[Registration]
    DROP CONSTRAINT IF EXISTS [DF_Registration_DomainTimestamp]
END

IF NOT EXISTS (
  SELECT *
  FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'Order' and TABLE_SCHEMA = 'orders' AND COLUMN_NAME = 'DomainTimestamp'
)
BEGIN
    ALTER TABLE [orders].[Order]
    ADD DomainTimestamp DATETIME2 NOT NULL
    CONSTRAINT [DF_Order_DomainTimestamp] DEFAULT GETUTCDATE()

    ALTER TABLE [orders].[Order]
    DROP CONSTRAINT IF EXISTS [DF_Order_DomainTimestamp]
END

IF NOT EXISTS (
  SELECT *
  FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'Exam' and TABLE_SCHEMA = 'exams' AND COLUMN_NAME = 'DomainTimestamp'
)
BEGIN
    ALTER TABLE [exams].[Exam]
    ADD DomainTimestamp DATETIME2 NOT NULL
    CONSTRAINT [DF_Exam_DomainTimestamp] DEFAULT GETUTCDATE()

    ALTER TABLE [exams].[Exam]
    DROP CONSTRAINT IF EXISTS [DF_Exam_DomainTimestamp]
END