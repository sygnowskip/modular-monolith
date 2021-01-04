using System;

namespace ModularMonolith.Contracts.Exams
{
    public class EditExamRequest
    {
        public int Capacity { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
    }
}