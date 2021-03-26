// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Data;

namespace Teronis.Windows.PresentationFoundation
{
    public class StaticResource : StaticResourceExtension
    {
        public PropertyPath Path { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // First get the value from StaticResource
            object value = base.ProvideValue(serviceProvider);
            return Path == null ? value : PathEvaluator.Evaluate(value, Path);
        }

        private class PathEvaluator : DependencyObject
        {
            /// <summary>
            /// This dummy will hold the end result.
            /// </summary>
            private static readonly DependencyProperty DummyProperty =
                DependencyProperty.Register("Dummy", typeof(object),
                typeof(PathEvaluator), new UIPropertyMetadata(null));

            public static object Evaluate(object source, PropertyPath path)
            {
                var pathEvaluator = new PathEvaluator();
                BindingOperations.SetBinding(pathEvaluator, DummyProperty, new Binding(path.Path) { Source = source });

                // Force binding to give us the desired value defined in path.
                var result = pathEvaluator.GetValue(DummyProperty);

                // Clear the binding to leave nice memory footprints
                BindingOperations.ClearBinding(pathEvaluator, DummyProperty);

                return result;
            }
        }
    }
}
