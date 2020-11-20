using System;
using CaseExtensions;

namespace Teronis.Mvc.Case
{
    public static class CaseExtensions
    {
        public static string ToCase(this string source, CaseType caseType)
        {
            return caseType switch
            {
                CaseType.CamelCase => source.ToCamelCase(),
                CaseType.KebabCase => source.ToKebabCase(),
                CaseType.PascalCase => source.ToPascalCase(),
                CaseType.SnakeCase => source.ToSnakeCase(),
                CaseType.TrainCase => source.ToTrainCase(),
                _ => throw new ArgumentException("Bad case type.")
            };
        }
    }
}
