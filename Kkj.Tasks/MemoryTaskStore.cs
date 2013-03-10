using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kkj.Tasks
{
    /// <summary>
    /// A task store that stores the tasks in memory.
    /// </summary>
    public class MemoryTaskStore : ITaskStore
    {
        [NotNull]
        private readonly IDictionary<
            string, SortedDictionary<DateTime, TaskVersion>
        > _taskVersions;

        /// <summary>
        /// Initializes a new memory task store.
        /// </summary>
        public MemoryTaskStore()
        {
            _taskVersions = new Dictionary<
                string,
                SortedDictionary<DateTime, TaskVersion>
            >();
        }

        /// <summary>
        /// Get the stored tasks.
        /// </summary>
        [NotNull]
        internal virtual
            IDictionary<string, SortedDictionary<DateTime, TaskVersion>>
            TaskVersions
        {
            get { return _taskVersions; }
        }

        /// <summary>
        /// Get all tasks.
        /// </summary>
        public IEnumerable<Task> Tasks
        {
            get { return TaskVersions.Select(p => New(p.Key, p.Value)); }
        }

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
        public Task Save(string taskName, TaskVersion taskVersion)
        {
            if (taskName == null)
            {
                throw new ArgumentNullException("taskName");
            }
            if (string.IsNullOrWhiteSpace(taskName))
            {
                throw new ArgumentOutOfRangeException("taskName");
            }
            if (taskVersion == null)
            {
                throw new ArgumentNullException("taskVersion");
            }
            var versions = GetVersions(taskName);
            versions.Add(taskVersion.Date, taskVersion);
            var task = New(taskName, versions);
            return task;
        }

        /// <summary>
        /// Get task versions by name. Creates dictionary for task if necessary.
        /// </summary>
        /// <param name="taskName">
        /// The task name.
        /// </param>
        /// <returns>
        /// The existing version dictionary or a created one.
        /// </returns>
        [NotNull]
        internal virtual SortedDictionary<DateTime, TaskVersion>
            GetVersions([NotNull] string taskName)
        {
            return TaskVersions.ContainsKey(taskName) ?
                TaskVersions[taskName] :
                TaskVersions[taskName] =
                    new SortedDictionary<DateTime, TaskVersion>();
        }

        /// <summary>
        /// Initializes a new task with the specified name.
        /// </summary>
        /// <param name="name">
        /// The task name.
        /// </param>
        /// <returns>
        /// The initialized task.
        /// </returns>
        [NotNull]
        internal virtual Task New(
            [NotNull] string name,
            [NotNull] SortedDictionary<DateTime, TaskVersion> versions
        )
        {
            return new Task(name) { Versions = versions.Select(p => p.Value) };
        }
    }
}
