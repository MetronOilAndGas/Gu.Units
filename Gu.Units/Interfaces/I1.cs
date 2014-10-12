namespace Gu.Units
{
    public interface I1<out T> : IPowerUnit<IUnit>
        where T : IUnit
    {
    }
}