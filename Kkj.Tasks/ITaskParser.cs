namespace Kkj.Tasks
{
    public interface ITaskParser
    {
        ParserResult Parse(string serializedTask);
    }
}