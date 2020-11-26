using System;
using Hexure.Events;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Events
{
    public class ExamRegistrationStartDateChanged : IEvent
    {
        public ExamRegistrationStartDateChanged(ExamId examId, DateTime registrationStartDate, DateTime publishedOn)
        {
            ExamId = examId;
            RegistrationStartDate = registrationStartDate;
            PublishedOn = publishedOn;
        }

        public ExamId ExamId { get; }
        public DateTime RegistrationStartDate { get; }
        public DateTime PublishedOn { get; }
    }
}