namespace DungeonDelver.Core.Turns
{
    public class TurnResult
    {
        public bool MadeProgress { get; set; }

        public bool NeedsRefresh()
        {
            return MadeProgress;
        }

        public void ClearEvents()
        {

        }
    }
}
