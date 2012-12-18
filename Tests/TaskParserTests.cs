using Kkj.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;

namespace Tests
{
    [TestClass]
    public class TaskParserTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Parse_SerializedTaskNull_Exception()
        {
            // Arrange.
            var parser = new TaskParser();

            // Act and assert.
            parser.Parse(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Parse_SerializedTaskEmpty_Exception()
        {
            // Arrange.
            var parser = new TaskParser();

            // Act and assert.
            parser.Parse("");
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Parse_SerializedTaskWhiteSpace_Exception()
        {
            // Arrange.
            var parser = new TaskParser();

            // Act and assert.
            parser.Parse(" ");
        }

        [TestMethod]
        public void Parse_Delegates()
        {
            // Arrange.
            var task = "task";
            var parser = MockRepository.GeneratePartialMock<TaskParser>();
            parser
                .Expect(p => p.IsSerializedTask(task))
                .Return(true);

            // Act.
            var res = parser.Parse(task);

            // Assert.
            parser.VerifyAllExpectations();
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Parse_InvalidSerializedTask_Exception()
        {
            // Arrange.
            var task = "task";
            var parser = MockRepository.GeneratePartialMock<TaskParser>();
            parser
                .Expect(p => p.IsSerializedTask(task))
                .Return(false);

            // Act and assert.
            parser.Parse(task);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ParseStatus_NullStatus_Exception()
        {
            new TaskParser().ParseStatus(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ParseStatus_UnknownStatus_Exception()
        {
            new TaskParser().ParseStatus("unknown status");
        }

        [TestMethod]
        public void ParseStatus_Works()
        {
            var status1 = new TaskParser().ParseStatus("[ ]");
            Assert.AreEqual(status1, TaskStatus.Pending);
            var status2 = new TaskParser().ParseStatus("[X]");
            Assert.AreEqual(status2, TaskStatus.Done);
        }
    }
}
