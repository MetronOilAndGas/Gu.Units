namespace Gu.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CompositeUnit : IUnit
    {
        private readonly IPowerUnit<IUnit>[] _parts;
        protected CompositeUnit()
        {
        }
        public CompositeUnit(IPowerUnit<IUnit> part)
        {
            _parts = new[] { part };
        }

        public CompositeUnit(params IPowerUnit<IUnit>[] parts)
        {
            if (parts.Length == 0)
            {
                throw new ArgumentException("Parts cannot be empty", "parts");
            }
            if (parts.Select(x => x.Unit).Distinct().Count() == parts.Length)
            {
                throw new ArgumentException("parts must be distinct units", "parts");
            }
            _parts = parts;
        }

        public virtual IEnumerable<IPowerUnit<IUnit>> Parts { get { return _parts; } }

        public bool IsMatch(IEnumerable<IPowerUnit<IUnit>> other)
        {
            throw new NotImplementedException("message");
        }
        public string Symbol
        {
            get
            {
                throw new NotImplementedException("message");
            }
        }

        public double ToSiUnit(double value)
        {
            throw new NotImplementedException();
        }
    }
}
