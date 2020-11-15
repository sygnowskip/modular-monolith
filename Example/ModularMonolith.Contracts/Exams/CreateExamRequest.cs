using System;

namespace ModularMonolith.Contracts.Exams
{
    public class CreateExamRequest
    {
        public long SubjectId { get; set; }
        public long LocationId { get; set; }
        public DateTime ExamDateTime { get; set; }
        public int Capacity { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
    }
}