namespace ModularMonolith.QueryServices.Exams
{
    public interface IQueryBuilder
    {
        string SingleExamQuery();
        string MultipleExamsQuery();
    }
    
    public class QueryBuilder : IQueryBuilder
    {
        private readonly string _baseQuery = @"
SELECT [ex].[Id]
      ,[ex].[LocationId]
	  ,[loc].[Name] as 'LocationName'
      ,[ex].[SubjectId]
	  ,[sbj].[Name] as 'SubjectName'
      ,[ex].[Capacity]
      ,[ex].[Booked]
      ,[ex].[RegistrationStartDate]
      ,[ex].[RegistrationEndDate]
      ,[ex].[ExamDateTime]
      ,[ex].[Status]
  FROM [exams].[Exam] ex
  JOIN [read].[Location] loc ON loc.Id = ex.LocationId
  JOIN [read].[Subject] sbj on sbj.Id = ex.SubjectId";
        
        public string SingleExamQuery()
        {
            return @$"
{_baseQuery}
WHERE [ex].[Id] = @id";
        }

        public string MultipleExamsQuery()
        {
            return _baseQuery;
        }
    }
}