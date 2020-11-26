using System;
using Hexure.Events;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Events
{
    public class ExamRegistrationEndDateChanged : IEvent
    {
        public ExamRegistrationEndDateChanged(ExamId examId, DateTime registrationEndDate, DateTime publishedOn)
        {
            ExamId = examId;
            RegistrationEndDate = registrationEndDate;
            PublishedOn = publishedOn;
        }

        public ExamId ExamId { get; }
        public DateTime RegistrationEndDate { get; }
        public DateTime PublishedOn { get; }
    }
}