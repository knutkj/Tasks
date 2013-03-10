using Kkj.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
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

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void SaveNoTaskNameException()
        {
            // Arrange.
            const string taskName = null;
            var version = new TaskVersion(new DateTime());
            var store = new MemoryTaskStore();

            // Act and assert.
            store.Save(taskName, version);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SaveEmptyTaskNameException()
        {
            // Arrange.
            const string taskName = " ";
            var version = new TaskVersion(new DateTime());
            var store = new MemoryTaskStore();

            // Act and assert.
            store.Save(taskName, version);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void SaveNoTaskVersionException()
        {
            // Arrange.
            const string taskName = "name";
            TaskVersion version = null;
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
            var task = new Task(name);
            var versions = new SortedDictionary<DateTime, TaskVersion>();
            var store = MockRepository.GeneratePartialMock<MemoryTaskStore>();

            store.Expect(s => s.GetVersions(name)).Return(versions);
            store.Expect(s => s.New(name, versions)).Return(task);

            // Act.
            var res = store.Save(name, version);

            // Assert.
            store.VerifyAllExpectations();
            Assert.AreEqual(task, res);
            Assert.AreEqual(1, versions.Count());
            Assert.AreEqual(version, versions[date]);
        }

        [TestMethod]
        public void NewCreatesTaskWithSpecifiedName()
        {
            // Arrange.
            const string taskName = "task name";

            var date = new DateTime();
            var version = new TaskVersion(date);

            var versions = new SortedDictionary<DateTime, TaskVersion>();
            versions.Add(date, version);

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
            var task = new Task(taskName);

            var date = new DateTime();
            var taskVersion = new TaskVersion(date);

            var versions = new SortedDictionary<DateTime, TaskVersion>();
            versions.Add(date, taskVersion);

            var taskVersions = new Dictionary<
                string, SortedDictionary<DateTime, TaskVersion>
            >();
            taskVersions.Add(taskName, versions);

            var store = MockRepository.GeneratePartialMock<MemoryTaskStore>();

            store.Expect(s => s.TaskVersions).Return(taskVersions);
            store.Expect(s => s.New(taskName, versions)).Return(task);

            // Act.
            var res = store.Tasks.ToList();

            // Assert.
            store.VerifyAllExpectations();
            Assert.AreEqual(1, res.Count());
            Assert.AreEqual(task, res.First());
        }

        [TestMethod]
        public void ClearDelegate()
        {
            // Arrange.
            var store = MockRepository.GeneratePartialMock<MemoryTaskStore>();
            store.Expect(s => s.TaskVersions.Clear());

            // Act.
            store.Clear();

            // Assert.
            store.VerifyAllExpectations();
        }
    }
}