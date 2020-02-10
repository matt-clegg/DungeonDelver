using DungeonDelver.Core.Turns;

namespace DungeonDelver.Core.Actions
{
    public abstract class BaseAction
    {
        public abstract ActionResult Perform(ITurnable turnable);

        protected virtual ActionResult Fail()
        {
            return ActionResult.Failure;
        }

        protected virtual ActionResult Succeed()
        {
            return ActionResult.Success;
        }

        protected virtual ActionResult Alternative(BaseAction action)
        {
            return new ActionResult(action);
        }
    }
}
