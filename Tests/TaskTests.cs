using Kkj.Tasks;
using Task = Kkj.Tasks.Task;
using TaskStatus = Kkj.Tasks.TaskStatus;

namespace Tests;

[TestClass]
public class TaskTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CtorNoNameThrows()
    {
        // Arrange.
        string name = null!;

        // Act and assert.
        new Task(name);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void CtorEmptyNameThrows()
    {
        // Arrange.
        const string name = " ";

        // Act and assert.
        new Task(name);
    }

    [TestMethod]
    public void GetName()
    {
        // Arrange.
        const string name = "task name";
        var task = new Task(name);

        // Act.
        var res = task.Name;

        // Assert.
        Assert.AreEqual(name, res);
    }

    [TestMethod]
    public void DateDelegates()
    {
        // Arrange.
        var date = new DateTime(2010, 1, 1);
        var task = new Task("task name")
        {
            Versions = [new TaskVersion(date)]
        };

        // Act.
        var res = task.Date;

        // Assert.
        Assert.AreEqual(date, res);
    }

    [TestMethod]
    public void StatusDelegates()
    {
        // Arrange.
        var status = TaskStatus.Done;
        var task = new Task("task name")
        {
            Versions = [new TaskVersion(new DateTime()) { Status = status }]
        };

        // Act.
        var res = task.Status;

        // Assert.
        Assert.AreEqual(status, res);
    }

    [TestMethod]
    public void PriorityDelegates()
    {
        // Arrange.
        var priority = TaskPriority.High;
        var task = new Task("task name")
        {
            Versions = [new TaskVersion(new DateTime()) { Priority = priority }]
        };

        // Act.
        var res = task.Priority;

        // Assert.
        Assert.AreEqual(priority, res);
    }
}
