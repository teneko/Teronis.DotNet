// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;
using Teronis.Mvc.Case;

namespace Teronis.Mvc.ApplicationModels
{
    /// <summary>
    /// A route template case formatter.
    /// </summary>
    public class RouteTemplateCaseFormatter : CaseFormatter
    {
        public RouteTemplateCaseFormatter(CaseType caseType)
            : base(caseType) { }

        /// <summary>
        /// Formats the case of <paramref name="routeTemplate"/>.
        /// </summary>
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public override string Format(string routeTemplate) =>
            Regex.Replace(routeTemplate, @"(?>[\w ]+)(?!\]|\})", 
                match => match.Value.ToCase(CaseType));
    }
}
