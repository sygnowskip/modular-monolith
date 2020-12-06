IF EXISTS (
  SELECT *
  FROM INFORMATION_SCHEMA.VIEWS
  WHERE TABLE_NAME = 'Exam' AND TABLE_SCHEMA = 'read'
)
BEGIN
	DROP VIEW [read].[Exam]
END
GO

CREATE VIEW [read].[Exam]
AS
SELECT [ex].[Id]
      ,[ex].[LocationId]
	  ,[loc].[Name] as 'LocationName'
      ,[ex].[SubjectId]
	  ,[sbj].[Name] as 'SubjectName'
      ,[ex].[Capacity]
      ,[ex].[RegistrationStartDate]
      ,[ex].[RegistrationEndDate]
      ,[ex].[ExamDateTime]
      ,[ex].[Status]
  FROM [exams].[Exam] ex
  JOIN [read].[Location] loc ON loc.Id = ex.LocationId
  JOIN [read].[Subject] sbj on sbj.Id = ex.SubjectId


