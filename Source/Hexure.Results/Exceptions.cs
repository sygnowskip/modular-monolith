using System;

namespace Hexure.Results
{

    internal static class ResultMessages
    {
        public static readonly string ErrorIsInaccessibleForSuccess = "There is no Error for a successful result.";

        public static readonly string ValueIsInaccessibleForFailure = "There is no Value for a failed result.";

        public static readonly string ErrorObjectIsNotProvidedForFailure =
            "You have tried to create a failure result, but error object appeared to be null, please review the code, generating error object.";

        public static readonly string ErrorObjectIsProvidedForSuccess =
            "You have tried to create a success result, but error object was also passed to the constructor, please try to review the code, creating a success result.";

        public static readonly string ErrorMessageIsNotProvidedForFailure = "There must be error message for failure.";

        public static readonly string ErrorMessageIsProvidedForSuccess = "There should be no error message for success.";
    }

    public class ResultSuccessException : Exception
    {
        internal ResultSuccessException() : base(ResultMessages.ErrorIsInaccessibleForSuccess)
        {
        }
    }

    public class ResultFailureException : Exception
    {
        public string Error { get; }

        internal ResultFailureException(string error) : base(ResultMessages.ValueIsInaccessibleForFailure)
        {
            Error = error;
        }
    }

    public class ResultFailureException<TError> : ResultFailureException
    {
        public new TError Error { get; }

        internal ResultFailureException(TError error) : base(ResultMessages.ValueIsInaccessibleForFailure)
        {
            Error = error;
        }
    }
}