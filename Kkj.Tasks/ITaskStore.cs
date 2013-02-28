using System.Collections.Generic;

namespace Kkj.Tasks
{
    public interface ITaskStore
    {
        IDictionary<string, Task> Tasks { get; }
    }
}