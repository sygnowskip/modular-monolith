DROP VIEW IF EXISTS [read].[Exam]

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


