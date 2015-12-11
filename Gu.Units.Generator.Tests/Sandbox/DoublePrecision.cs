namespace Gu.Units.Generator.Tests.Sandbox
{
    using System;
    using NUnit.Framework;

    public class DoublePrecision
    {
        [Test]
        public void TestName()
        {
            Console.WriteLine($"1E3 {RoundtripsMultiplication(1E3, 1E-3)} {RoundtripsDivision(1E3)}");
            Console.WriteLine($"1E6 {RoundtripsMultiplication(1E6, 1E-6)} {RoundtripsDivision(1E6)}");
            Console.WriteLine($"1E-3 {RoundtripsMultiplication(1E-3, 1E3)} {RoundtripsDivision(1E-3)}");
            Console.WriteLine($"1E-6 {RoundtripsMultiplication(1E-6, 1E6)} {RoundtripsDivision(1E-6)}");
        }

        private static bool RoundtripsMultiplication(double f1, double f2)
        {
            for (int i = 0; i < 100; i++)
            {
                var temp = f1 * i;
                var rt = f2 * temp;
                if (i != rt)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool RoundtripsDivision(double f1)
        {
            for (int i = 0; i < 100; i++)
            {
                var temp = f1 * i;
                var rt = temp / f1;
                if (i != rt)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
