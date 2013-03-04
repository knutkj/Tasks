using Kkj.Tasks;
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
            var date = new DateTime();
            const string serializedTask = "serialized task";
            const string taskName = "task name";
            var parserResult = new ParserResult { Name = taskName };
            var task = new Task(taskName, date);
            var parser = MockRepository.GenerateStrictMock<ITaskParser>();
            var store = MockRepository.GenerateStrictMock<ITaskStore>();

            var factory =
                MockRepository.GeneratePartialMock<TaskFactory>(parser, store);

            parser
                .Expect(p => p.Parse(serializedTask))
                .Return(parserResult);
            factory
                .Expect(f => f.New(date, parserResult))
                .Return(task);
            store
                .Expect(s => s.Save(task))
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
            Assert.AreEqual(name, res.Name);
            Assert.AreEqual(priority, res.Priority);
            Assert.AreEqual(priority, res.Priority);
        }
    }
}
