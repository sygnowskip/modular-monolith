using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Hexure.EntityFrameworkCore.SqlServer.Hints
{
    public class HintInterceptor : IObserver<KeyValuePair<string, object>>
    {
        private static readonly Regex TableAliasRegex = new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(*HINT*\)))", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        [ThreadStatic]
#pragma warning disable S1104 // Fields should not have public accessibility
        public static string HintValue;
#pragma warning restore S1104 // Fields should not have public accessibility

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key == RelationalEventId.CommandExecuting.Name)
            {
                var command = ((CommandEventData)value.Value).Command;
                command.CommandText = ApplyHint(command.CommandText);
            }
        }

        private static string ApplyHint(string commandText)
        {
            if (!string.IsNullOrWhiteSpace(HintValue))
            {
                if (!TableAliasRegex.IsMatch(commandText))
                {
                    throw new InvalidProgramException("Failed to match table alias", new Exception(commandText));
                }
                commandText = TableAliasRegex.Replace(commandText, "${tableAlias} WITH (*HINT*)");
                commandText = commandText.Replace("*HINT*", HintValue);
            }

            HintValue = string.Empty;
            return commandText;
        }

    }
}