using System.Collections.Generic;
using System.Linq;

namespace DungeonDelver.Core.Events
{
    public class GameEventManager
    {
        private readonly List<GameEvent> _events = new List<GameEvent>();
        private readonly List<GameEvent> _toRemove = new List<GameEvent>();

        private readonly HashSet<int> _eventIds = new HashSet<int>();

        public bool HasEventsToProcess()
        {
            return _events.Count > 0;
        }

        public void AddEvents(IReadOnlyCollection<GameEvent> events)
        {
            _events.AddRange(events);
        }

        public void Update(float delta)
        {
            if (_events.Count == 0)
            {
                return;
            }

            bool hasBlock = _events.Any(e => e.IsBlocking);
            _eventIds.Clear();
            _toRemove.Clear();

            foreach (GameEvent gameEvent in _events)
            {
                // TODO: check boolean logic here
                if ((hasBlock && gameEvent.IsBlocking) || !hasBlock)
                {
                    if (gameEvent.Id != null)
                    {
                        if (_eventIds.Contains(gameEvent.Id.Value))
                        {
                            continue;
                        }

                        _eventIds.Add(gameEvent.Id.Value);
                    }

                    if (gameEvent.Update(delta))
                    {
                        System.Console.WriteLine("finished " + gameEvent.Id);
                        _toRemove.Add(gameEvent);
                    }
                }
            }

            foreach (GameEvent removeEvent in _toRemove)
            {
                _events.Remove(removeEvent);
            }
        }
    }
}
