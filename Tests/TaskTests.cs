using Kkj.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass]
    public class TaskTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CtorNoNameThrows()
        {
            // Arrange.
            const string name = null;

            // Act and assert.
            new Task(name);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
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

            Assert.AreEqual(task.Name, res);
        }
    }
}
