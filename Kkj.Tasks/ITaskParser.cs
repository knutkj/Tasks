using JetBrains.Annotations;

namespace Kkj.Tasks
{
    public interface ITaskParser
    {
        /// <summary>
        /// Initializes a new <see cref="ParserResult"/> based on the
        /// serialized task.
        /// </summary>
        /// <remarks>
        /// Only one serialized task must be provided.
        /// </remarks>
        /// <param name="serializedTask">
        /// A serialized task.
        /// </param>
        /// <returns>
        /// A parser result or <c>null</c> if no task.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If serializedTask is <c>null</c>.
        /// </exception>
        [CanBeNull]
        ParserResult Parse(string serializedTask);
    }
}