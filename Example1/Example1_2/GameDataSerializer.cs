namespace GameDataSerializer;

public interface IDataSerializer: IDataLoader { }

public interface IDataLoader
{
    bool TryLoad<T>(string path, out T data)
}

public class FileDataSerializer: IDataSerializer
{
    public bool TryLoad<T>(string path, out T data)
    {
        try
        {
            data = Serializer.LoadFromFile<T>(path);
        }
        catch (Exception e)
        {
            // Handle exception
            return false;
        }

        return data != default;
    }
}