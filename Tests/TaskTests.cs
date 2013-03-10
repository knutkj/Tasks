using Kkj.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Linq;

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

        [TestMethod]
        public void DateDelegates()
        {
            // Arrange.
            var date = new DateTime(2010, 1, 1);
            var task = MockRepository.GeneratePartialMock<Task>("task name");
            var versions = new[] { new TaskVersion(date) };
            task.Expect(t => t.Versions).Return(versions);

            // Act.
            var res = task.Date;

            // Assert.
            task.VerifyAllExpectations();
            Assert.AreEqual(date, res);
        }

        [TestMethod]
        public void StatusDelegates()
        {
            // Arrange.
            var status = TaskStatus.Done;
            var task = MockRepository.GeneratePartialMock<Task>("task name");
            var versions = new[] {
                new TaskVersion(new DateTime()) { Status = status }
            };
            task.Expect(t => t.Versions).Return(versions);

            // Act.
            var res = task.Status;

            // Assert.
            task.VerifyAllExpectations();
            Assert.AreEqual(status, res);
        }

        [TestMethod]
        public void PriorityDelegates()
        {
            // Arrange.
            var priority = TaskPriority.High;
            var task = MockRepository.GeneratePartialMock<Task>("task name");
            var versions = new[] {
                new TaskVersion(new DateTime()) { Priority = priority }
            };
            task.Expect(t => t.Versions).Return(versions);

            // Act.
            var res = task.Priority;

            // Assert.
            task.VerifyAllExpectations();
            Assert.AreEqual(priority, res);
        }
    }
}
