using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a store for tasks.
    /// </summary>
    public interface ITaskStore
    {
        /// <summary>
        /// Get all tasks.
        /// </summary>
        IEnumerable<Task> Tasks { get; }

        /// <summary>
        /// Saves the specified task version.
        /// </summary>
        /// <param name="taskName">
        /// The name of the task that this task version belongs to.
        /// </param>
        /// <param name="taskVersion">
        /// The task version to save.
        /// </param>
        /// <returns>
        /// The task with the specified name.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <c>taskName</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <c>taskName</c> is white space.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If <c>taskVersion</c> is <c>null</c>.
        /// </exception>
        [NotNull]
        Task Save([NotNull] string taskName, [NotNull] TaskVersion taskVersion);
    }
}