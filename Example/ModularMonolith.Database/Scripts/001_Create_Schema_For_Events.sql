IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'events')) 
BEGIN
    EXEC ('CREATE SCHEMA [events] AUTHORIZATION [dbo]')
END