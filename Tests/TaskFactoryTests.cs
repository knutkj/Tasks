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
            var task = new Task();
            var parser = MockRepository.GenerateStrictMock<ITaskParser>();
            var store = MockRepository.GenerateStrictMock<ITaskStore>();
            var dictionary =
                MockRepository.GenerateStrictMock<IDictionary<string, Task>>();

            var factory = new TaskFactory(parser, store);

            parser
                .Expect(p => p.Parse(serializedTask))
                .Return(new ParserResult { Name = taskName });
            store
                .Expect(s => s.Tasks)
                .Return(dictionary);
            dictionary
                .Expect(d => d[taskName])
                .Return(task);

            // Act.
            var res = factory.Create(date, serializedTask);

            // Assert.
            parser.VerifyAllExpectations();
            store.VerifyAllExpectations();
            dictionary.VerifyAllExpectations();
            Assert.AreEqual(task, res);
        }
    }
}
