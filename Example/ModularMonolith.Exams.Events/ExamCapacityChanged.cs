using System;
using Hexure.Events;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Events
{
    public class ExamCapacityChanged : IEvent
    {
        public ExamCapacityChanged(ExamId examId, int capacity, DateTime publishedOn)
        {
            ExamId = examId;
            Capacity = capacity;
            PublishedOn = publishedOn;
        }

        public ExamId ExamId { get; }
        public int Capacity { get; }
        public DateTime PublishedOn { get; }
    }
}