using Kkj.Tasks;

namespace Tests;

[TestClass]
public class MemoryTaskStoreTests
{
    [TestMethod]
    public void GetTaskVersionsReturnsTasksDictionary()
    {
        // Arrange.
        var store = new MemoryTaskStore();

        // Act.
        var res = store.TaskVersions;

        // Assert.
        Assert.IsNotNull(res);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SaveNoTaskNameException()
    {
        // Arrange.
        string taskName = null!;
        var version = new TaskVersion(new DateTime());
        var store = new MemoryTaskStore();

        // Act and assert.
        store.Save(taskName, version);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void SaveEmptyTaskNameException()
    {
        // Arrange.
        const string taskName = " ";
        var version = new TaskVersion(new DateTime());
        var store = new MemoryTaskStore();

        // Act and assert.
        store.Save(taskName, version);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SaveNoTaskVersionException()
    {
        // Arrange.
        const string taskName = "name";
        TaskVersion version = null!;
        var store = new MemoryTaskStore();

        // Act and assert.
        store.Save(taskName, version);
    }

    [TestMethod]
    public void SaveCreatesTaskAndAppendsVersion()
    {
        // Arrange.
        const string name = "task name";
        var date = new DateTime(2000, 1, 1);
        var version = new TaskVersion(date);
        var store = new MemoryTaskStore();

        // Act.
        var res = store.Save(name, version);

        // Assert.
        Assert.IsNotNull(res);
        Assert.AreEqual(name, res.Name);
        Assert.AreEqual(1, store.TaskVersions[name].Count);
        Assert.AreEqual(version, store.TaskVersions[name][date]);
    }

    [TestMethod]
    public void NewCreatesTaskWithSpecifiedName()
    {
        // Arrange.
        const string taskName = "task name";

        var date = new DateTime();
        var version = new TaskVersion(date);

        var versions = new SortedDictionary<DateTime, TaskVersion>
        {
            { date, version }
        };

        var store = new MemoryTaskStore();

        // Act.
        var task = store.New(taskName, versions);

        // Assert.
        Assert.AreEqual(taskName, task.Name);
        Assert.AreEqual(1, task.Versions.Count());
        Assert.AreEqual(version, task.Versions.First());
    }

    [TestMethod]
    public void GetVersionsCreateNewDictionaryIfDoesNotExist()
    {
        // Arrange.
        const string name = "task name";
        var store = new MemoryTaskStore();

        // Act.
        var versions = store.GetVersions(name);

        // Assert.
        Assert.IsNotNull(versions);
    }

    [TestMethod]
    public void GetVersionsReturnsExisting()
    {
        // Arrange.
        const string name = "task name";
        var existingVersions =
            new SortedDictionary<DateTime, TaskVersion>();
        var store = new MemoryTaskStore();
        store.TaskVersions.Add(name, existingVersions);

        // Act.
        var versions = store.GetVersions(name);

        // Assert.
        Assert.AreEqual(existingVersions, versions);
    }

    [TestMethod]
    public void GetTasks()
    {
        // Arrange.
        const string taskName = "task name";
        var date = new DateTime();
        var taskVersion = new TaskVersion(date);
        var store = new MemoryTaskStore();
        store.Save(taskName, taskVersion);

        // Act.
        var res = store.Tasks.ToList();

        // Assert.
        Assert.AreEqual(1, res.Count);
        Assert.AreEqual(taskName, res.First().Name);
    }

    [TestMethod]
    public void ClearDelegate()
    {
        // Arrange.
        var store = new MemoryTaskStore();
        store.Save("task", new TaskVersion(new DateTime()));
        Assert.AreEqual(1, store.TaskVersions.Count);

        // Act.
        store.Clear();

        // Assert.
        Assert.AreEqual(0, store.TaskVersions.Count);
    }
}
