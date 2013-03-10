namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a store for tasks.
    /// </summary>
    public interface ITaskStore
    {
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
        Task Save(string taskName, TaskVersion taskVersion);
    }
}