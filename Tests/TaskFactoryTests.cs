﻿using Kkj.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class TaskFactoryTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CtorNoParserThrowsArgumentNullException()
        {
            // Arrange.
            ITaskParser parser = null;
            var store = MockRepository.GenerateStub<ITaskStore>();

            // Act.
            new TaskFactory(parser, store);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CtorNoStoreThrowsArgumentNullException()
        {
            // Arrange.
            var parser = MockRepository.GenerateStub<ITaskParser>();
            ITaskStore store = null;

            // Act.
            new TaskFactory(parser, store);
        }

        [TestMethod]
        public void CtorSavesArgs()
        {
            // Arrange.
            var parser = MockRepository.GenerateStub<ITaskParser>();
            var store = MockRepository.GenerateStub<ITaskStore>();

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
            var taskVersion = new TaskVersion(date);
            var task = new Task(taskName);
            var parser = MockRepository.GenerateStrictMock<ITaskParser>();
            var store = MockRepository.GenerateStrictMock<ITaskStore>();

            var factory =
                MockRepository.GeneratePartialMock<TaskFactory>(parser, store);

            parser
                .Expect(p => p.Parse(serializedTask))
                .Return(parserResult);
            factory
                .Expect(f => f.New(date, parserResult))
                .Return(taskVersion);
            store
                .Expect(s => s.Save(taskName, taskVersion))
                .Return(task);

            // Act.
            var res = factory.Create(date, serializedTask);

            // Assert.
            parser.VerifyAllExpectations();
            factory.VerifyAllExpectations();
            store.VerifyAllExpectations();
            Assert.AreEqual(task, res);
        }

        [TestMethod]
        public void CreateUnableToParseNull()
        {
            // Arrange.
            const string serializedTask = "serialized task";
            var parser = MockRepository.GenerateStrictMock<ITaskParser>();
            var store = MockRepository.GenerateStrictMock<ITaskStore>();
            var factory = new TaskFactory(parser, store);
            parser.Expect(p => p.Parse(serializedTask)).Return(null);

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
                MockRepository.GenerateStrictMock<ITaskParser>(),
                MockRepository.GenerateStrictMock<ITaskStore>()
            );

            // Act.
            var res = factory.New(date, parserResult);

            // Assert.
            Assert.AreEqual(priority, res.Priority);
            Assert.AreEqual(priority, res.Priority);
        }

        [TestMethod]
        public void CurrentStore()
        {
            // Arrange.

            // Act.
            var store = TaskFactory.CurrentStore;

            // Assert.
            Assert.IsInstanceOfType(store, typeof(MemoryTaskStore));
        }

        [TestMethod]
        public void CurrentFactory()
        {
            // Arrange.

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
}
