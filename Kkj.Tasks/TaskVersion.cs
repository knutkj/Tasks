using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Kkj.Tasks
{
    public class TaskVersion
    {
        private readonly DateTime _date;

        /// <summary>
        /// Initializes a new task version with the specified date.
        /// </summary>
        /// <param name="date">
        /// The date of the task.
        /// </param>
        public TaskVersion(DateTime date)
        {
            _date = date;
        }

        /// <summary>
        /// Get the date.
        /// </summary>
        public DateTime Date { get { return _date; } }

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
