using System.Collections.Generic;
using System.Linq;

using Narivia.Infrastructure.Extensions;
using Narivia.Logging.Enumerations;

namespace Narivia.Logging
{
    public static class LogBuilder
    {
        /// <summary>
        /// Builds a log message based on key-value pairs.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="status">The status of the operation.</param>
        /// <returns>The log message.</returns>
        public static string BuildKvpMessage(Operation operation, OperationStatus status)
        {
            string message = $"Operation={operation.GetDisplayName()}," +
                             $"OperationStatus={status.GetDisplayName()}";

            return message;
        }

        /// <summary>
        /// Builds a log message based on key-value pairs.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="status">The status of the operation.</param>
        /// <param name="logDetails">The log details as a key-value pair dictionary.</param>
        /// <returns>The log message.</returns>
        public static string BuildKvpMessage(Operation operation, OperationStatus status, IDictionary<LogInfoKey, string> logDetails)
        {
            string message = $"{BuildKvpMessage(operation, status)},";

            logDetails.ToList().ForEach(kvp => message += $"{kvp.Key.GetDisplayName()}={kvp.Value},");

            return message.TrimEnd(',');
        }
    }
}
