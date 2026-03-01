using Kkj.Tasks;
using NSubstitute;
using Task = Kkj.Tasks.Task;
using TaskFactory = Kkj.Tasks.TaskFactory;
using TaskStatus = Kkj.Tasks.TaskStatus;

namespace Tests;

[TestClass]
public class TaskFactoryTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CtorNoParserThrowsArgumentNullException()
    {
        // Arrange.
        ITaskParser parser = null!;
        var store = Substitute.For<ITaskStore>();

        // Act.
        new TaskFactory(parser, store);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CtorNoStoreThrowsArgumentNullException()
    {
        // Arrange.
        var parser = Substitute.For<ITaskParser>();
        ITaskStore store = null!;

        // Act.
        new TaskFactory(parser, store);
    }

    [TestMethod]
    public void CtorSavesArgs()
    {
        // Arrange.
        var parser = Substitute.For<ITaskParser>();
        var store = Substitute.For<ITaskStore>();

        // Act.
        var factory = new TaskFactory(parser, store);

        // Assert.
        Assert.AreEqual(parser, factory.Parser);
        Assert.AreEqual(store, factory.Store);
    }

    [TestMethod]
    public void CreateDelegates()
    {
        // Arrange.
        var date = new DateTime(2000, 1, 1);
        const string serializedTask = "serialized task";
        const string taskName = "task name";
        var parserResult = new ParserResult { Name = taskName };
        var task = new Task(taskName);
        var parser = Substitute.For<ITaskParser>();
        var store = Substitute.For<ITaskStore>();

        parser.Parse(serializedTask).Returns(parserResult);
        store.Save(taskName, Arg.Any<TaskVersion>()).Returns(task);

        var factory = new TaskFactory(parser, store);

        // Act.
        var res = factory.Create(date, serializedTask);

        // Assert.
        parser.Received().Parse(serializedTask);
        store.Received().Save(taskName, Arg.Any<TaskVersion>());
        Assert.AreEqual(task, res);
    }

    [TestMethod]
    public void CreateUnableToParseNull()
    {
        // Arrange.
        const string serializedTask = "serialized task";
        var parser = Substitute.For<ITaskParser>();
        var store = Substitute.For<ITaskStore>();
        var factory = new TaskFactory(parser, store);
        parser.Parse(serializedTask).Returns((ParserResult?)null);

        // Act.
        var res = factory.Create(new DateTime(), serializedTask);

        // Assert.
        Assert.IsNull(res);
    }

    [TestMethod]
    public void NewBasedOnParserResult()
    {
        // Arrange.
        var date = new DateTime();
        const string name = "Task name";
        const TaskStatus status = TaskStatus.Done;
        const TaskPriority priority = TaskPriority.Normal;
        var parserResult = new ParserResult
        {
            Name = name,
            Status = status,
            Priority = priority
        };

        var factory = new TaskFactory(
            Substitute.For<ITaskParser>(),
            Substitute.For<ITaskStore>()
        );

        // Act.
        var res = factory.New(date, parserResult);

        // Assert.
        Assert.AreEqual(priority, res.Priority);
        Assert.AreEqual(status, res.Status);
    }

    [TestMethod]
    public void CurrentStore()
    {
        // Act.
        var store = TaskFactory.CurrentStore;

        // Assert.
        Assert.IsInstanceOfType(store, typeof(MemoryTaskStore));
    }

    [TestMethod]
    public void CurrentFactory()
    {
        // Act.
        var factory = TaskFactory.CurrentFactory;

        // Assert.
        Assert.IsInstanceOfType(factory, typeof(TaskFactory));
        Assert.AreEqual(
            TaskFactory.CurrentStore,
            ((TaskFactory)factory).Store
        );
    }
}
