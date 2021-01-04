using System;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.ReadModels.Planning
{
    public class Exam
    {
        public long Id { get; protected set; }
        public long LocationId { get; protected set; }
        public string LocationName { get; protected set; }
        public long SubjectId { get; protected set; }
        public string SubjectName { get; protected set; }
        public int Capacity { get; protected set; }
        public DateTime RegistrationStartDate { get; protected set; }
        public DateTime RegistrationEndDate { get; protected set; }
        public DateTime ExamDateTime { get; protected set; }
        public ExamStatus Status { get; protected set; }
    }
}