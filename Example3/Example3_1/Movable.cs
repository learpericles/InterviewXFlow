namespace Movable;

public interface IMovable
{
    UpdatePathToEnemy();
    Stop();
}

public class Movable : IMovable
{
    private List<Vector2> activeWalkPath = null;
    private bool isMoving;
    private Vector2 currentPosition;

    public void UpdatePathToEnemy(ITarget target)
    {
        if (target == null)
        {
            return;
        }

        activeWalkPath =  <много строк кода по созданию пути >;
        isMoving = activeWalkPath != null;
    }

    public void Stop()
    {
        if (!isMoving)
        {
            return;
        }

        isMoving = false;
    }
}