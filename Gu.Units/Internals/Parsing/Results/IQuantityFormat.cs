namespace Gu.Units
{
    internal interface IQuantityFormat
    {
        string Format { get; }

        IUnit Unit { get; }
    }
}