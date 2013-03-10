using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a task.
    /// </summary>
    public class Task
    {
        private string _name;

        /// <summary>
        /// Initializes a new task with the specified task name.
        /// </summary>
        /// <param name="name">
        /// The task name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <c>name</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <c>name</c> is white space.
        /// </exception>
        public Task(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException("name");
            }
            _name = name;
        }

        /// <summary>
        /// Get the date.
        /// </summary>
        public DateTime Date
        {
            get { return Versions.Last().Date; }
        }

        /// <summary>
        /// Get the priority.
        /// </summary>
        public TaskPriority Priority
        {
            get { return Versions.Last().Priority; }
        }

        /// <summary>
        /// Get the status.
        /// </summary>
        public TaskStatus Status
        {
            get { return Versions.Last().Status; }
        }

        /// <summary>
        /// Get or set the task name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Get the task versions.
        /// </summary>
        [NotNull]
        public virtual IEnumerable<TaskVersion> Versions { get; internal set; }
    }
}
