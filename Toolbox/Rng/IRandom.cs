namespace Toolbox.Rng
{
    public interface IRandom
    {
        int Next();
        int Next(int max);
        int Next(int min, int max);
        double NextDouble();
        double NextDouble(double min, double max);
    }
}
