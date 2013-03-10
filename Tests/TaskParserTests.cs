using Kkj.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;

namespace Tests
{
    [TestClass]
    public class TaskParserTests
    {
        private const string TaskPri = "!";
        private const string TaskStat = "X";
        private const string TaskName = "Name";
        private const string ValidSimpleSerializedTask =
            TaskPri + "[" + TaskStat + "]" + TaskName;

        [TestMethod]
        public void Pattern_Works()
        {
            var res1 = TaskParser.SingleLinePattern.Match(" ! [ ] t1");
            Assert.IsTrue(res1.Groups[1].Success);
            Assert.AreEqual("!", res1.Groups[1].Value);

            var res11 = TaskParser.SingleLinePattern.Match("?[X] t11");
            Assert.AreEqual("?", res11.Groups[1].Value);

            var res12 = TaskParser.SingleLinePattern.Match(" [ ] t12");
            Assert.IsFalse(res12.Groups[1].Success);
            Assert.AreEqual("", res12.Groups[1].Value);

            var res2 = TaskParser.SingleLinePattern.Match("[X]t2");
            Assert.AreEqual("X", res2.Groups[2].Value);

            var res3 = TaskParser.SingleLinePattern.Match(" [ ] Task name");
            Assert.AreEqual("Task name", res3.Groups[3].Value);

            var res4 = TaskParser.SingleLinePattern.Match(
                "[ ] Name { tag1, tag2 }"
            );
            Assert.AreEqual("tag1, tag2 ", res4.Groups[4].Value);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Parse_SerializedTaskNull_Exception()
        {
            // Arrange.
            var parser = new TaskParser();

            // Act and assert.
            parser.Parse(null);
        }

        [TestMethod]
        public void Parse_SerializedTaskEmpty_Exception()
        {
            // Arrange.
            var parser = new TaskParser();

            // Act.
            var res = parser.Parse("");

            // Assert.
            Assert.IsNull(res);
        }

        [TestMethod]
        public void Parse_SerializedTaskWhiteSpace_Null()
        {
            // Arrange.
            var parser = new TaskParser();

            // Act.
            var res = parser.Parse(" ");

            // Assert.
            Assert.IsNull(res);
        }

        [TestMethod]
        public void Parse_Delegates()
        {
            // Arrange.
            const string task = "task";
            
            var tokens = Tuple.Create<string, string, string>(
                "priority",
                "status",
                "name"
            );

            var expectedPriority = TaskPriority.High;
            var expectedStatus = TaskStatus.Done;
            const string expectedName = "name";

            var parser = MockRepository.GeneratePartialMock<TaskParser>();
            parser
                .Expect(p => p.GetTokens(task))
                .Return(tokens);
            parser
                .Expect(p => p.ParsePriority(tokens.Item1))
                .Return(expectedPriority);
            parser
                .Expect(p => p.ParseStatus(tokens.Item2))
                .Return(expectedStatus);
            parser
                .Expect(p => p.ParseName(tokens.Item3))
                .Return(expectedName);

            // Act.
            var res = parser.Parse(task);

            // Assert.
            parser.VerifyAllExpectations();
            Assert.AreEqual(expectedPriority, res.Priority);
            Assert.AreEqual(expectedStatus, res.Status);
            Assert.AreEqual(expectedName, res.Name);
        }

        [TestMethod]
        public void Parse_NoTokens_Null()
        {
            // Arrange.
            var task = "task";

            var parser = MockRepository.GeneratePartialMock<TaskParser>();
            parser
                .Expect(p => p.GetTokens(task))
                .Return(null);

            // Act.
            var res = parser.Parse(task);

            // Assert.
            parser.VerifyAllExpectations();
            Assert.IsNull(res);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ParsePriority_Null_Exception()
        {
            new TaskParser().ParsePriority(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ParsePriority_Unknown_Exception()
        {
            new TaskParser().ParsePriority("unknown priority");
        }

        [TestMethod]
        public void ParsePriority_Works()
        {
            var pri1 = new TaskParser().ParsePriority("");
            Assert.AreEqual(pri1, TaskPriority.Normal);

            var pri2 = new TaskParser().ParsePriority("!");
            Assert.AreEqual(pri2, TaskPriority.High);

            var pri3 = new TaskParser().ParsePriority("?");
            Assert.AreEqual(pri3, TaskPriority.Low);
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
            var status1 = new TaskParser().ParseStatus(" ");
            Assert.AreEqual(status1, TaskStatus.Pending);

            var status2 = new TaskParser().ParseStatus("X");
            Assert.AreEqual(status2, TaskStatus.Done);
        }

        [TestMethod]
        public void GetTokensMatchReturnsTokens()
        {
            // Arrange.
            var parser = new TaskParser();

            // Act.
            var tokens = parser.GetTokens(ValidSimpleSerializedTask);

            // Assert.
            Assert.AreEqual(TaskPri, tokens.Item1);
            Assert.AreEqual(TaskStat, tokens.Item2);
            Assert.AreEqual(TaskName, tokens.Item3);
        }

        [TestMethod]
        public void GetTokensNotMatchReturnsNull()
        {
            // Arrange.
            var parser = new TaskParser();

            // Act.
            var tokens = parser.GetTokens("");

            // Assert.
            Assert.IsNull(tokens);
        }

        [TestMethod]
        public void ParseNameReturnsVanillaName()
        {
            // Arrange.
            const string expectedName = "name";
            var parser = new TaskParser();

            // Act.
            var name = parser.ParseName(expectedName);

            // Assert.
            Assert.AreEqual(expectedName, name);
        }

        [TestMethod]
        public void ParseNameReturnsTrimmedName()
        {
            // Arrange.
            const string nameString = " name ";
            var expectedName = nameString.Trim();
            var parser = new TaskParser();

            // Act.
            var name = parser.ParseName(nameString);

            // Assert.
            Assert.AreEqual(expectedName, name);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ParseNameNullException()
        {
            // Arrange.
            const string nameString = null;
            var parser = new TaskParser();

            // Act and assert.
            var name = parser.ParseName(nameString);
        }
    }
}
