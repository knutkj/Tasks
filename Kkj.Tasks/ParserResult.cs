namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a task parser result.
    /// </summary>
    public class ParserResult
    {
        /// <summary>
        /// Get or set the status.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Get or set the priority.
        /// </summary>
        public TaskPriority Priority { get; set; }
    }
}
