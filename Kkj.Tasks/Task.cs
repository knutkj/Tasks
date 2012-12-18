using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a task.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Get or set the task status.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Get the tags that has been applied to this task.
        /// </summary>
        [NotNull]
        public IEnumerable<Tag> Tags
        {
            get { throw new NotImplementedException(); }
        }
    }
}
