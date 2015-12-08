namespace Gu.Units.Generator.Views
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UnitViewModel
    {
        public string UnitName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private static string CreateName(IReadOnlyCollection<UnitAndPower> unitAndPowers)
        {
            var builder = new StringBuilder();
            int sign = 1;
            foreach (var up in unitAndPowers)
            {
                if (sign == 1 && up.Power < 0)
                {
                    builder.Append("Per");
                    sign = -1;
                }

                var p = Math.Abs(up.Power);
                switch (p)
                {
                    case 1:
                        break;
                    case 2:
                        builder.Append("Square");
                        break;
                    case 3:
                        builder.Append("Cubic");
                        break;
                    default:
                        throw new NotImplementedException("message");
                }

                if (up.Power > 0)
                {
                    builder.Append(up.Unit.Name);
                }
                else
                {
                    builder.Append(up.Unit.Name.TrimEnd('s'));
                }
            }

            return builder.ToString();
        }
    }
}
