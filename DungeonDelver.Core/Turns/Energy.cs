namespace DungeonDelver.Core.Turns
{
    public class Energy
    {
        public const int ActionCost = 12;

        public const int MinSpeed = 0;
        public const int NormalSpeed = 3;
        public const int MaxSpeed = 5;

        private static readonly int[] EnergyGains = { 2, 3, 4, 6, 9, 12 };

        public int CurrentEnergy { get; private set; }
        public bool CanTakeTurn => CurrentEnergy >= ActionCost;

        public void Spend()
        {
            if (CanTakeTurn)
            {
                CurrentEnergy = CurrentEnergy % ActionCost;
            }
        }

        public bool Gain(int speed)
        {
            if (speed < 0)
            {
                speed = 0;
            }

            if (speed >= EnergyGains.Length - 1)
            {
                speed = EnergyGains.Length - 1;
            }

            CurrentEnergy += EnergyGains[speed];
            return CanTakeTurn;
        }

        public void Reset() => CurrentEnergy = 0;
    }
}
