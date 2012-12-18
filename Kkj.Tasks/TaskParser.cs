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
            if (!IsSerializedTask(serializedTask))
            {
                throw new ArgumentException(
                    "Specified argument is not a serialized task.",
                    "serializedTask"
                );
            }
            return null;
        }

        /// <summary>
        /// Checks if the specified serialized task is valid.
        /// </summary>
        /// <param name="serializedTask">
        /// The serialized task to check.
        /// </param>
        /// <returns>
        /// True if valid serialized task or false.
        /// </returns>
        public virtual bool IsSerializedTask([CanBeNull] string serializedTask)
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