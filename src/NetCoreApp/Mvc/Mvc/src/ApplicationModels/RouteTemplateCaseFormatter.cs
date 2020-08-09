using System.Text.RegularExpressions;
using Teronis.Mvc.Case;

namespace Teronis.Mvc.ApplicationModels
{
    public class RouteTemplateCaseFormatter : CaseFormatter
    {
        public RouteTemplateCaseFormatter(CaseType caseType)
            : base(caseType) { }

        public override string Format(string source) =>
            Regex.Replace(source, @"(?>[\w ]+)(?!\]|\})", 
                match => match.Value.ToCase(CaseType));
    }
}
