using Kkj.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void ValidTaskParsed()
        {
            // Arrange.
            const string taskName = "Task #1";
            const string serializedTask = "[ ] " + taskName;
            var parser = new TaskParser();
            var store = new MemoryTaskStore();
            var factory = new TaskFactory(parser, store);

            // Act.
            var task = factory.Create(new DateTime(), serializedTask);

            // Assert.
            Assert.IsNotNull(task);
            var tasks = store.Tasks;
            Assert.AreEqual(1, tasks.Count());
            Assert.AreEqual(taskName, tasks.First().Name);
        }

        [TestMethod]
        public void InvalidTaskParsed()
        {
            // Arrange.
            const string serializedTask = "";
            var parser = new TaskParser();
            var store = new MemoryTaskStore();
            var factory = new TaskFactory(parser, store);

            // Act.
            var task = factory.Create(new DateTime(), serializedTask);

            // Assert.
            Assert.IsNull(task);
        }
    }
}
