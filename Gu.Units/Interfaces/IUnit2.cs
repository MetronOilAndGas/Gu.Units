namespace Gu.Units
{
    interface IUnit2<T1, T2> : IUNitN
        where T1 : IPowerUnit<IUnit>
        where T2 : IPowerUnit<IUnit>
    {
    }
}