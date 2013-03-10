using JetBrains.Annotations;
using System;

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
        internal TaskFactory(
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
            var parserResult = Parser.Parse(serializedTask);
            var taskVersion = New(new DateTime(), parserResult);
            return Store.Save(parserResult.Name, taskVersion);
        }

        /// <summary>
        /// Initializes a new <see cref="TaskVersion"/> with the specified date
        /// and values from the specified parser result.
        /// </summary>
        /// <param name="date">
        /// The date of the task version.
        /// </param>
        /// <param name="parserResult">
        /// The parser result.
        /// </param>
        /// <returns>
        /// The new task version.
        /// </returns>
        [NotNull]
        internal virtual TaskVersion New(
            DateTime date, 
            ParserResult parserResult
        )
        {
            return new TaskVersion(date)
            {
                Status = parserResult.Status,
                Priority = parserResult.Priority
            };
        }
    }
}
