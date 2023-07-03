namespace TallyDB.Tests.Config
{
  [TestClass]
  public class ConfigStorageTests
  {
    private MockConfigStorage configStorage;

    public ConfigStorageTests()
    {
      var mockFileOps = new MockFileOperable();
      configStorage = new MockConfigStorage(mockFileOps);
    }

    [TestInitialize]
    public void BeforeEach()
    {
      var mockFileOps = new MockFileOperable();
      configStorage = new MockConfigStorage(mockFileOps);
    }

    [TestMethod("Should insert new record into config storage")]
    public void ShouldReadAndWriteToConfigStorage()
    {
      var input = new MockConfigType("Sample");
      configStorage.Insert(input);
      var doc = configStorage.GetAll().First();

      Assert.AreEqual(doc.Name, input.Name);
    }

    [TestMethod("Should save already inserted doc")]
    public void ShouldSaveAlreadyInserted()
    {
      var input = new MockConfigType("Hello");
      configStorage.Insert(input);

      input.Name = "World";
      configStorage.Save(input, d => d.Name == "Hello");

      var output = configStorage.GetAll().First();

      Assert.AreEqual(input.Name, output.Name);
    }

    [TestMethod("Should handle multiple records")]
    public void ShouldHandleMultipleRecords()
    {
      var recordsAdded = new List<MockConfigType>();
      for (var i = 0; i < new Random().Next(5, 15); i++) {
        var record = new MockConfigType("Record" + i.ToString());
        configStorage.Insert(record);
        recordsAdded.Add(record);
      }

      // Verify each added
      var allAdded = configStorage.GetAll();

      foreach(var record in recordsAdded)
      {
        if (allAdded.FirstOrDefault(u => u.Name == record.Name) == null)
        {
          Assert.Fail();
          return;
        }
      }
    }
  }
}
