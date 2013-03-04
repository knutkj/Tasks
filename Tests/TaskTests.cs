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
            var date = new DateTime();

            // Act and assert.
            new Task(name, date);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CtorEmptyNameThrows()
        {
            // Arrange.
            const string name = " ";
            var date = new DateTime();

            // Act and assert.
            new Task(name, date);
        }

        [TestMethod]
        public void GetName()
        {
            // Arrange.
            const string name = "task name";
            var task = new Task(name, new DateTime());

            // Act.
            var res = task.Name;

            // Assert.

            Assert.AreEqual(task.Name, res);
        }
    }
}
