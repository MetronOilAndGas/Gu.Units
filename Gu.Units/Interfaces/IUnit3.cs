namespace Gu.Units
{
    interface IUnit3<T1, T2, T3> : IUNitN
        where T1 : IPowerUnit<IUnit>
        where T2 : IPowerUnit<IUnit>
        where T3 : IPowerUnit<IUnit>
    {
    }
}