using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a task.
    /// </summary>
    public class Task : TaskVersion
    {
        /// <summary>
        /// Initializes a new task version with the specified task name and
        /// date.
        /// </summary>
        /// <param name="name">
        /// The task name.
        /// </param>
        /// <param name="date">
        /// The date of the task.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <c>name</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <c>date</c> is <c>null</c>.
        /// </exception>
        public Task(string name, DateTime date)
            : base(name, date)
        { }

        /// <summary>
        /// Get or set the task name.
        /// </summary>
        public override string Name
        {
            get { return _name; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Get the task versions.
        /// </summary>
        [NotNull]
        public IEnumerable<TaskVersion> Versions { get; internal set; }
    }
}
