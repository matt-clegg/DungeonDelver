using DungeonDelver.Core.Events;
using System.Collections.Generic;

namespace DungeonDelver.Core.Turns
{
    public class TurnResult
    {
        private readonly List<GameEvent> _events = new List<GameEvent>();

        public IReadOnlyCollection<GameEvent> Events => _events;

        public bool MadeProgress { get; set; }

        public bool NeedsRefresh() => MadeProgress;

        public void AddEvent(GameEvent gameEvent) => _events.Add(gameEvent);

        public void ClearEvents() => _events.Clear();
    }
}
