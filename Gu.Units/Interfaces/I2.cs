namespace Gu.Units
{
    public interface I2<out T> : IPowerUnit<IUnit>
        where T : IUnit
    {
    }
}