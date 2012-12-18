namespace Kkj.Tasks
{
    public interface ITaskFactory
    {
        Task Create(string serializedTask);
    }
}