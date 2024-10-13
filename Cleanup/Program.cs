namespace Cleanup
{
    // Основные цели рефакторинга:
    // Внедрение зависимостей замест использования реализаций напрямую
    // Декомпозиция метода CleanupTest
    // Снижение комплекности состояния класса за счет использования локальных переменных
    internal class Program
    {
        private const double TargetChangeTime = 1;

        // Убраны _lockedCandidateTarget и _lockedTarget, так как, в соответствии с текущим примером, они всегда будут null
        private ITargetable _previousTarget;
        private double _previousTargetSetTime;

        private ITargetFinder _targetInRangeContainer;
        private ITimeProvider _timeProvider;

        private ITargetable CurrentTarget => TargetableEntity.Selected;

        public Program(ITargetFinder targetFinder, ITimeProvider timeProver)
        {
            _targetInRangeContainer = targetFinder;
            _timeProvider = timeProver;
        }

        public void CleanupTest(Frame frame)
        {
            ITargetable target = null;

            try
            {
                TryUpdateTarget(frame, out target);
            }
            catch (Exception ex)
            {
                // Handle exception
            }
            finally
            {
                if (IsTargetValid(target) && _previousTarget != target)
                {
                    _previousTargetSetTime = _timeProvider.GetTimeMilli();
                }

                TargetableEntity.Selected = target;
            }
        }

        private bool TryUpdateTarget(Frame frame, out ITargetable target)
        {
            target = null;

            if (IsTargetValid(CurrentTarget) && !CanChangeTarget())
            {
                return false;
            }

            _previousTarget = CurrentTarget;

            if (TryGetActiveTargetFromQuantum(frame, out var activateTargetCandidate) && IsTargetValid(activateTargetCandidate))
            {
                target = activateTargetCandidate;
                return true;
            }

            if (TryFindTarget(out var targetCandidate))
            {
                target = targetCandidate;
                return true;
            }

            return false;
        }

        private bool CanChangeTarget()
        {
            return (_timeProvider.GetTimeMilli() - _previousTargetSetTime) >= TargetChangeTime;
        }

        private bool IsTargetValid(ITargetable target)
        {
            return target && target.CanBeTarget;
        }

        private bool TryFindTarget(out ITargetable target)
        {
            target = _targetInRangeContainer.GetTarget();
            return IsTargetValid(target);
        }

        // MORE CLASS CODE
    }
}

public interface ITargetable
{
    public bool CanBeTarget { get; set; }
}

public interface ITargetFinder
{
    public ITargetable GetTarget();
}

public interface ITimeProvider
{
    public double GetTimeMilli();
}