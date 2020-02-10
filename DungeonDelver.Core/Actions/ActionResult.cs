namespace DungeonDelver.Core.Actions
{
    public class ActionResult
    {
        public static readonly ActionResult Success = new ActionResult(true);
        public static readonly ActionResult Failure = new ActionResult(false);

        public bool Succeeded { get; }
        public BaseAction Alternative { get; }

        public ActionResult(bool success)
        {
            Succeeded = success;
            Alternative = null;
        }

        public ActionResult(BaseAction alternative)
        {
            Succeeded = false;
            Alternative = alternative;
        }
    }
}
