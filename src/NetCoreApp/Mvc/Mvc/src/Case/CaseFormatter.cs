using System;

namespace Teronis.Mvc.Case
{
    public class CaseFormatter : IStringFormatter
    {
        public CaseType CaseType { get; }

        public CaseFormatter(CaseType caseType)
        {
            if (!Enum.IsDefined(typeof(CaseType), caseType)) {
                throw new ArgumentException("Bad case type.");
            }

            CaseType = caseType;
        }

        public virtual string Format(string source) =>
            source.ToCase(CaseType);

    }
}
