// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Teronis.Windows.PresentationFoundation.Data
{
    public class ParentSourceBinding : MarkupExtension
    {
        public string Path { get; private set; }

        public ParentSourceBinding(string path)
        {
            Path = path;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding(Path).ProvideValue(serviceProvider);
            return binding;
        }
    }
}
