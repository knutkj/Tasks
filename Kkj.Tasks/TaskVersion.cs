using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Kkj.Tasks
{
    public abstract class TaskVersion
    {
        protected string _name;

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
        public TaskVersion(string name, DateTime date)
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
        /// Get or set the name.
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// Get or set the status.
        /// </summary>
        public virtual TaskStatus Status { get; set; }

        /// <summary>
        /// Get or set the priority.
        /// </summary>
        public virtual TaskPriority Priority { get; set; }

        /// <summary>
        /// Get the tags that has been applied.
        /// </summary>
        [NotNull]
        public virtual IEnumerable<Tag> Tags
        {
            get { throw new NotImplementedException(); }
        }
    }
}
