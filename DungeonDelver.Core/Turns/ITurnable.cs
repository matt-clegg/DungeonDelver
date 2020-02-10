using DungeonDelver.Core.Actions;

namespace DungeonDelver.Core.Turns
{
    public interface ITurnable
    {
        bool CanTakeTurn();
        bool IsWaitingForInput();
        bool GainEnergy();
        void FinishTurn();

        BaseAction GetAction();
    }
}
