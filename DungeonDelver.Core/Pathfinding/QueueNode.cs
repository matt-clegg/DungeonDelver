using Priority_Queue;

namespace DungeonDelver.Core.Pathfinding
{
    internal class QueueNode<T> : FastPriorityQueueNode
    {
        public T Item { get; }

        public QueueNode(T item)
        {
            Item = item;
        }
    }
}
