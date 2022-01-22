using System;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Contracts.Registrations
{
    public class RegistrationDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long ExamId { get; set; }
        public RegistrationStatus Status { get; set; }
        public string CandidateFirstName { get; set; }
        public string CandidateLastName { get; set; }
        public DateTime CandidateDateOfBirth { get; set; }
    }
}