namespace Kkj.Tasks
{
    /// <summary>
    /// Represents a task tag.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Get or set the tag name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Get or set the tasks that has this tag applied.
        /// </summary>
        public IEnumerable<Task> Tasks { get; set; } = [];
    }
}
