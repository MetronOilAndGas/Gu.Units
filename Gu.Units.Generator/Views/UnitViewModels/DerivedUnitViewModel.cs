namespace Gu.Units.Generator
{
    using System;
    using Reactive;

    public class DerivedUnitViewModel : UnitViewModel<DerivedUnit>
    {
        public DerivedUnitViewModel()
            : this(new DerivedUnit(UnknownName, UnknownSymbol, UnknownName, new UnitAndPower[0]))
        {
            Unit.ObservePropertyChangedSlim().Subscribe(_ =>
            {
                if (Settings.Instance.DerivedUnits.Contains(Unit))
                {
                    return;
                }

                if (!IsUnknown)
                {
                    Settings.Instance.DerivedUnits.Add(Unit);
                }
            });
        }

        public DerivedUnitViewModel(DerivedUnit unit)
            : base(unit)
        {
        }
    }
}