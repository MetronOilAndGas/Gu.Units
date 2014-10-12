namespace Gu.Units
{
    public interface IPowerUnit<out T> : IUnit where T : IUnit
    {
        T Unit { get; }
        int Power { get; }
    }
}