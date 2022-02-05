namespace ModularMonolith.Tests.Scenarios
{
    public class Scenarios
    {
        public Scenarios(LocationScenarios locations, MessageScenarios messages, SubjectScenarios subjects,
            ExamScenarios exams, RegistrationScenarios registrations, OrderScenarios orders)
        {
            Locations = locations;
            Messages = messages;
            Subjects = subjects;
            Exams = exams;
            Registrations = registrations;
            Orders = orders;
        }

        public LocationScenarios Locations { get; }
        public SubjectScenarios Subjects { get; }
        public ExamScenarios Exams { get; }
        public RegistrationScenarios Registrations { get; }
        public OrderScenarios Orders { get; }
        public MessageScenarios Messages { get; }
    }
}