using JetBrains.Annotations;
using System;
using System.Text.RegularExpressions;

namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a task parser that parses serialized tasks.
    /// </summary>
    public class TaskParser : ITaskParser
    {
        internal readonly static Regex Pattern =
            new Regex(@"^\s*\[([ !?X])\]\s*([^{]*)(?:{\s*([^}]+)})?$");

        /// <summary>
        /// Initializes a new Task based on the serialized task.
        /// </summary>
        /// <param name="serializedTask">
        /// A serialized task.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If serializedTask is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If serializedTask is white space.
        /// </exception>
        public ParserResult Parse([NotNull] string serializedTask)
        {
            if (serializedTask == null)
            {
                throw new ArgumentNullException("serializedTask");
            }
            if (string.IsNullOrWhiteSpace(serializedTask))
            {
                throw new ArgumentException("serializedTask");
            }
            var tokens = GetTokens(serializedTask);
            var status = ParseStatus(tokens.Item1);
            return new ParserResult
            {
                Status = status
            };
        }

        /// <summary>
        /// Get the task tokens.
        /// </summary>
        /// <param name="serializedTask">
        /// A serialized task.
        /// </param>
        /// <returns>
        /// The task tokens or <c>null</c> if not a task.
        /// </returns>
        internal virtual Tuple<string> GetTokens(string serializedTask)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses the specified status string and finds the related task
        /// status.
        /// </summary>
        /// <param name="statusString">
        /// The status string to parse.
        /// </param>
        /// <returns>The task status.</returns>
        /// <exception cref="ArgumentNullException">
        /// If statuString is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If not able to parse statuString.
        /// </exception>
        internal virtual TaskStatus ParseStatus([NotNull] string statusString)
        {
            if (statusString == null)
            {
                throw new ArgumentNullException("statusString");
            }
            switch (statusString)
            {
                case "[ ]":
                    return TaskStatus.Pending;
                case "[X]":
                    return TaskStatus.Done;
            }
            throw new ArgumentException("statusString");
        }
    }
}