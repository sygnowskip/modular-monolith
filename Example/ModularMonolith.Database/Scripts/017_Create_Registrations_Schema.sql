IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'registrations')
BEGIN
	EXEC('CREATE SCHEMA registrations')
END