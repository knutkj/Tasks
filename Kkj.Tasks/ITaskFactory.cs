using System;

namespace Kkj.Tasks
{
    public interface ITaskFactory
    {
        Task Create(DateTime date, string serializedTask);
    }
}