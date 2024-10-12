namespace DefaultNamespace;

public interface ITargetFinder<T> where T : ITarget
{
    bool TryFindTarget(out T enemy);
}

public class EnemyFinder: ITargetFinder<Player>
{
    private Player _activeEnemy;

    public bool TryFindTarget(out Player enemy)
    {
        if (_activeEnemy != null)
        {
            return _activeEnemy;
        }

        _activeEnemy = new Player();
        return _activeEnemy != null;
    }
}