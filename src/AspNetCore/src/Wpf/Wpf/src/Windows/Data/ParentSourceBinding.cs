using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Teronis.Windows.Data
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
