using System;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Contracts.Exams
{
    public class ExamDto
    {
        public long Id { get; set; }
        public long LocationId { get; set; }
        public string LocationName { get; set; }
        public long SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int Capacity { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public DateTime ExamDateTime { get; set; }
        public ExamStatus Status { get; set; }
    }
}