using Hexure.Results;

namespace ModularMonolith.Exams.Domain
{
    public static class ExamErrors
    {
        private static string Combine(string prefix, string key) => $"{prefix}_{key}";
        
        public static class Actions
        {
            public static readonly Error.ErrorType UnableToOpenForRegistration =
                new Error.ErrorType(nameof(UnableToOpenForRegistration), "Unable to open for registration");

            public static readonly Error.ErrorType UnableToCloseRegistration =
                new Error.ErrorType(nameof(UnableToCloseRegistration), "Unable to close registration");

            public static readonly Error.ErrorType UnableToMarkAsDone =
                new Error.ErrorType(nameof(UnableToMarkAsDone), "Unable to mark as done");

            public static readonly Error.ErrorType UnableToDelete =
                new Error.ErrorType(nameof(UnableToDelete), "Unable to delete");

            public static readonly Error.ErrorType UnableToChangeCapacity =
                new Error.ErrorType(nameof(UnableToChangeCapacity), "Unable to change capacity");
        }

        public static class RegistrationStartDate
        {
            public static readonly Error.ErrorType UnableToChange =
                new Error.ErrorType(Combine(nameof(RegistrationStartDate), nameof(UnableToChange)),
                    "Unable to change registration start date");

            public static readonly Error.ErrorType HasToBeSetInTheFuture =
                new Error.ErrorType(Combine(nameof(RegistrationStartDate), nameof(HasToBeSetInTheFuture)),
                    "Registration start date has to be set for today or the future date");

            public static readonly Error.ErrorType HasToBeSetBeforeRegistrationEndDate =
                new Error.ErrorType(Combine(nameof(RegistrationStartDate), nameof(HasToBeSetBeforeRegistrationEndDate)),
                    "Registration start date has to be set before registration end date");
        }

        public static class RegistrationEndDate
        {
            public static readonly Error.ErrorType UnableToChange =
                new Error.ErrorType(Combine(nameof(RegistrationEndDate), nameof(UnableToChange)),
                    "Unable to change registration end date");

            public static readonly Error.ErrorType HasToBeSetInTheFuture =
                new Error.ErrorType(Combine(nameof(RegistrationEndDate), nameof(HasToBeSetInTheFuture)),
                    "Registration end date has to be set for today or the future date");

            public static readonly Error.ErrorType HasToBeSetBeforeExamDate =
                new Error.ErrorType(Combine(nameof(RegistrationEndDate), nameof(HasToBeSetBeforeExamDate)),
                    "Registration end date has to be set before exam date");
        }
    }
}