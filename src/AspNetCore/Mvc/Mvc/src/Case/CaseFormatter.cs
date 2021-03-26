// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Mvc.Case
{
    public class CaseFormatter : IStringFormatter
    {
        public CaseType CaseType { get; }

        public CaseFormatter(CaseType caseType)
        {
            if (!Enum.IsDefined(typeof(CaseType), caseType)) {
                throw new ArgumentException("Case type does not exist.");
            }

            CaseType = caseType;
        }

        public virtual string Format(string source) =>
            source.ToCase(CaseType);
    }
}
