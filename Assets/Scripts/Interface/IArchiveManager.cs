
public interface IArchiveManager : IManager
{
    void Save(string key, object value);

    object Load(string key, object instance);
}
