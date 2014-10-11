namespace Gu.Units.Tests
{
    public interface IPowerUnit<out T> : IUnit where T : IUnit
    {
        T Unit { get; }
        int Power { get; }
    }
}