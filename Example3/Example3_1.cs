public class Player
{
	// - Извлек механизм поиска врага и передвижения в отдельные компоненты
	private readonly IMovable _movable = new Movable();
	private readonly ITargetFinder _targetFinder = new TargetFinder;

	public void Update()
	{
		if (_targetFinder.TryFindTarget(out var enemy))
		{
			_movable.UpdatePathToEnemy(enemy);
		}
		else
		{
			_movable.Stop();
		}
	}
}

