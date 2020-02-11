namespace DungeonDelver.Core.Events
{
    public abstract class GameEvent
    {
        public bool IsBlocking { get; }
        public int? Id { get; set; }

        private bool _isFirstUpdate = true;

        public GameEvent(bool isBlocking = false)
        {
            IsBlocking = isBlocking;
        }

        public bool Update(float delta)
        {
            if (_isFirstUpdate)
            {
                OnStart();
                _isFirstUpdate = false;
            }

            bool result = Process(delta);

            if (result)
            {
                OnEnd();
            }

            return result;
        }

        protected abstract bool Process(float delta);

        protected virtual void OnStart()
        {

        }

        protected virtual void OnEnd()
        {

        }
    }
}
