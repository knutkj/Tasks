using Kkj.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;

namespace Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void Test1()
        {
            // Arrange.
            var date = new DateTime(2000, 1, 1);
            const string serializedTask = "[ ] Task #1";
            var parser = new TaskParser();
            var store = MockRepository.GenerateStub<ITaskStore>();
            var factory = new TaskFactory(parser, store);

            // Act.
            var task = factory.Create(date, serializedTask);

            // Assert.

        }
    }
}
