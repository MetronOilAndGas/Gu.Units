namespace Gu.Units
{
    public interface I3<out T> : IPowerUnit<IUnit>
        where T : IUnit
    {
    }
}