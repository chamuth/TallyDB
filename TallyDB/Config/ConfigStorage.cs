namespace TallyDB.Config
{
  public abstract class ConfigStorage<T>
  {
    public abstract string Path { get; set; }
  
    public void Insert(T doc)
    {
      
    }
  }
}
