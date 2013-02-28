using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a factory that creates tasks.
    /// </summary>
    public class TaskFactory : ITaskFactory
    {
        private readonly ITaskParser _taskParser;
        private readonly ITaskStore _taskStore;

        /// <summary>
        /// Get the task parser.
        /// </summary>
        internal ITaskParser Parser { get { return _taskParser; } }

        /// <summary>
        /// Get the store.
        /// </summary>
        internal ITaskStore Store { get { return _taskStore; } }

        /// <summary>
        /// Initializes a new task factory with the specified task parser.
        /// </summary>
        /// <param name="taskParser">
        /// The task parser to use.
        /// </param>
        /// <exception cref="ArgumenNullException">
        /// If <c>taskParser</c> is <c>null</c>.
        /// </exception>
        public TaskFactory(
            ITaskParser taskParser,
            ITaskStore taskStore
        )
        {
            if (taskParser == null)
            {
                throw new ArgumentNullException("taskParser");
            }
            if (taskStore == null)
            {
                throw new ArgumentNullException("taskStore");
            }
            _taskParser = taskParser;
            _taskStore = taskStore;
        }

        public Task Create(DateTime date, string serializedTask)
        {
            return Store.Tasks[Parser.Parse(serializedTask).Name];
        }
    }
}
