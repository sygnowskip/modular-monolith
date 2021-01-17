using System;
using Hexure.Events;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Events
{
    public class ExamDeleted : IEvent
    {
        public ExamDeleted(ExamId examId, DateTime publishedOn)
        {
            ExamId = examId;
            PublishedOn = publishedOn;
        }

        public ExamId ExamId { get; }
        public DateTime PublishedOn { get; }
    }
}