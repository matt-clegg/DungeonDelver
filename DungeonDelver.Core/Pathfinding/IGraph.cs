using System.Collections.Generic;

namespace DungeonDelver.Core.Pathfinding
{
    public interface IGraph<T>
    {
        int Area { get; }
        IEnumerable<T> GetNeighbors(T id);
    }
}
