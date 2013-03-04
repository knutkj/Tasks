namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a store for tasks.
    /// </summary>
    public interface ITaskStore
    {
        /// <summary>
        /// Saves and updates the specified task.
        /// </summary>
        /// <param name="task">
        /// The task to save.
        /// </param>
        /// <returns>
        /// The same but updated task.
        /// </returns>
        Task Save(Task task);
    }
}