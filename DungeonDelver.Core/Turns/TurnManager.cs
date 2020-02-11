using DungeonDelver.Core.Actions;
using System;
using System.Collections.Generic;

namespace DungeonDelver.Core.Turns
{
    public class TurnManager<T> where T : ITurnable
    {
        private readonly TurnResult _turnResult = new TurnResult();

        private readonly IReadOnlyList<T> _turnables;
        private int _currentIndex;

        public bool IsRunning { get; set; }

        public TurnManager(IReadOnlyList<T> turnables)
        {
            _turnables = turnables ?? throw new ArgumentNullException(nameof(turnables));
            IsRunning = true;
        }

        public TurnResult Process()
        {
            _turnResult.MadeProgress = false;
            _turnResult.ClearEvents();

            if (_turnables.Count == 0)
            {
                return _turnResult;
            }

            while (IsRunning)
            {
                T turnable = CurrentTurnable();

                if (turnable.CanTakeTurn() && turnable.IsWaitingForInput())
                {
                    return _turnResult;
                }

                _turnResult.MadeProgress = true;

                BaseAction action = GetNextAction(ref turnable);

                if (action == null)
                {
                    return _turnResult;
                }

                ActionResult result = PerformAction(action, turnable);
                if (result.Succeeded)
                {
                    turnable.FinishTurn();
                    AdvanceIndex();
                }
            }

            return _turnResult;
        }

        private ActionResult PerformAction(BaseAction action, T turnable)
        {
            ActionResult result = action.Perform(turnable, _turnResult);
            while (result.Alternative != null)
            {
                action = result.Alternative;
                result = action.Perform(turnable, _turnResult);
            }

            return result;
        }

        private BaseAction GetNextAction(ref T turnable)
        {
            BaseAction action = null;
            while (action == null)
            {
                turnable = CurrentTurnable();

                if (EqualityComparer<T>.Default.Equals(default(T), turnable))
                {
                    AdvanceIndex();
                    continue;
                }

                if (turnable.CanTakeTurn() || turnable.GainEnergy())
                {
                    if (turnable.IsWaitingForInput())
                    {
                        return null;
                    }

                    action = turnable.GetAction();
                }
                else
                {
                    if (!turnable.IsTurning())
                    {
                        AdvanceIndex();
                    }
                }
            }

            return action;
        }

        private void AdvanceIndex()
        {
            _currentIndex = (_currentIndex + 1) % _turnables.Count;
        }

        private T CurrentTurnable()
        {
            if (_currentIndex < 0 || _currentIndex >= _turnables.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return _turnables[_currentIndex];
        }
    }
}
