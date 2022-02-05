IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'orders')
BEGIN
	EXEC('CREATE SCHEMA orders')
END