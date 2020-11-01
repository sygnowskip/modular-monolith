using System;
using Hexure.Events;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Events
{
    public class ExamAvailable : IEvent
    {
        public ExamAvailable(ExamId examId, DateTime publishedOn)
        {
            ExamId = examId;
            PublishedOn = publishedOn;
        }

        public ExamId ExamId { get; }
        public DateTime PublishedOn { get; }
    }
}