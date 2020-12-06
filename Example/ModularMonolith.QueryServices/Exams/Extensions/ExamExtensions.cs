using ModularMonolith.Contracts.Exams;
using ModularMonolith.ReadModels.Planning;

namespace ModularMonolith.QueryServices.Exams.Extensions
{
    public static class ExamExtensions
    {
        public static ExamDto ToExamDto(this Exam exam) => new ExamDto()
        {
            Id = exam.Id,
            Capacity = exam.Capacity,
            Status = exam.Status,
            LocationId = exam.LocationId,
            LocationName = exam.LocationName,
            SubjectId = exam.SubjectId,
            SubjectName = exam.SubjectName,
            ExamDateTime = exam.ExamDateTime,
            RegistrationEndDate = exam.RegistrationEndDate,
            RegistrationStartDate = exam.RegistrationStartDate
        };
    }
}