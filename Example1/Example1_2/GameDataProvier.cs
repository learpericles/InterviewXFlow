namespace GameDataProvider;

public interface IGameDataProvider<T>
{
    public string Path { get; }

    public bool TryGet(out T data)
}

public class PlayerDataProvider: IGameDataProvider<Player>
{
    public string Path { get; } = "NewPlayer.json";

    private readonly IGameDataSerializer _serializer;

    public PlayerDataProvider(IGameDataSerializer serializer)
    {
        _serializer = serializer;
    }

    public bool TryGet(out Player data)
    {
        return _serializer.TryLoad<Player>(Path, out data);
    }
}

public class SettingsDataProvider: IGameDataProvider<Settings>
{
    public string Path { get; } = "Settings.json";

    private readonly IGameDataSerializer _serializer;

    public PlayerDataProvider(IGameDataSerializer serializer)
    {
        _serializer = serializer;
    }

    public bool TryGet(out Settings data)
    {
        return _serializer.TryLoad<Settings>(Path, out data);
    }
}