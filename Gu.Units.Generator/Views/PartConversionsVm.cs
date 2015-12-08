﻿namespace Gu.Units.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class PartConversionsVm : INotifyPropertyChanged
    {
        private readonly ObservableCollection<PartConversionVm[]> conversions = new ObservableCollection<PartConversionVm[]>();
        private readonly Settings settings;
        private Unit unit;

        public PartConversionsVm(Settings settings)
        {
            this.settings = settings;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PartConversionVm[]> Conversions => this.conversions;

        public void SetBaseUnit(Unit value)
        {
            this.unit = value;
            this.conversions.Clear();

            if (this.unit == null ||
                this.unit.Parts.BaseParts.Count != 2)
            {
                return;
            }

            var unitParts = this.unit.Parts.BaseParts.ToArray();
            var p0s = CreatePowerParts(unitParts, 0);
            var p1s = CreatePowerParts(unitParts, 1);
            foreach (var c1 in p0s)
            {
                var cs = new List<PartConversionVm>();

                foreach (var c2 in p1s)
                {
                    cs.Add(new PartConversionVm(this.unit.PartConversions, PartConversion.Create(c1, c2)));
                }

                this.conversions.Add(cs.ToArray());
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static IReadOnlyList<PartConversion.PowerPart> CreatePowerParts(IReadOnlyList<UnitAndPower> parts, int index)
        {
            var powerParts = new List<PartConversion.PowerPart>();
            var unitAndPower = parts[index];
            powerParts.Add(new PartConversion.PowerPart(unitAndPower.Power, new PartConversion.IdentityConversion(unitAndPower.Unit)));
            powerParts.AddRange(unitAndPower.Unit.AllConversions.Select(x => new PartConversion.PowerPart(unitAndPower.Power, x)));
            return powerParts;
        }
    }
}