IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'read')
BEGIN
	EXEC('CREATE SCHEMA [read]')
END