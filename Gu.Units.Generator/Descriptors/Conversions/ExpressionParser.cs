namespace Gu.Units.Generator
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis.CSharp.Scripting;

    internal class ExpressionParser
    {
        internal static double Evaluate(double value, string expression)
        {
            var parameter = FindParameter(expression);
            expression = $"double {parameter} = {value.ToString("G17", CultureInfo.InvariantCulture)};\r\n" +
                         $"return {expression};";
           return CSharpScript.EvaluateAsync<double>(expression).Result; // don't feel like async all the way :)
        }

        private static string FindParameter(string expression)
        {
            var match = Regex.Match(expression, @"[A-z]{2,}");
            if (!match.Success)
            {
                throw new InvalidOperationException($"Did not find a parameter in {expression}");
            }

            return match.Value;
        }
    }
}
