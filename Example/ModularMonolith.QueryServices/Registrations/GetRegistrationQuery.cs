using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hexure.Results;
using MediatR;
using ModularMonolith.Contracts.Registrations;
using ModularMonolith.Errors;

namespace ModularMonolith.QueryServices.Registrations
{
    public class GetRegistrationQuery : IRequest<Result<RegistrationDto>>
    {
        public GetRegistrationQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
        
        public static Result<GetRegistrationQuery> Create(long examId)
        {
            return Result.Ok(new GetRegistrationQuery(examId));
        }
    }
    
    public class GetRegistrationQueryHandler : IRequestHandler<GetRegistrationQuery, Result<RegistrationDto>>
    {
        private readonly IDbConnection _dbConnection;

        public GetRegistrationQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Result<RegistrationDto>> Handle(GetRegistrationQuery request, CancellationToken cancellationToken)
        {
            var registration =
                await _dbConnection.QuerySingleAsync<RegistrationDto>(BuildQuery(), new { id = request.Id });
            
            
            return registration != null
                ? Result.Ok(registration)
                : Result.Fail<RegistrationDto>(DomainErrors.BuildNotFound("Registration", request.Id));
        }

        private string BuildQuery()
        {
            return @"SELECT [Id]
      ,[ExamId]
      ,[OrderId]
      ,[ExternalId]
      ,[Status]
      ,[CandidateFirstName]
      ,[CandidateLastName]
      ,[CandidateDateOfBirth]
  FROM [registrations].[Registration]
  WHERE [Id] = @id";
        }
    }
}