using Kkj.Tasks;

namespace Tests;

[TestClass]
public class TaskVersionTests
{
    [TestMethod]
    public void CtorSavesDateRef()
    {
        // Arrange.
        var date = new DateTime(2000, 1, 1);

        // Act.
        var version = new TaskVersion(date);

        // Assert.
        Assert.AreEqual(date, version.Date);
    }
}
