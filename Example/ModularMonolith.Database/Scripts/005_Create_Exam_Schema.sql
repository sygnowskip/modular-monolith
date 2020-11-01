IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'exams')
BEGIN
	EXEC('CREATE SCHEMA exams')
END