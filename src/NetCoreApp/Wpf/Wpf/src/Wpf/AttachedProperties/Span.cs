using System;
using System.Linq;
using System.Windows;
using RunControl = System.Windows.Documents.Run;
using SpanControl = System.Windows.Documents.Span;

namespace Teronis.Wpf.AttachedProperties
{
    public static class Span
    {
        public static readonly DependencyProperty TrimRunsProperty =
            DependencyProperty.RegisterAttached("TrimRuns", typeof(bool), typeof(Span),
                new PropertyMetadata(false, trimRunsChanged));

        public static bool GetTrimRuns(SpanControl span)
            => (bool)span.GetValue(TrimRunsProperty);

        public static void SetTrimRuns(SpanControl span, bool value)
            => span.SetValue(TrimRunsProperty, value);

        private static void trimRunsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = d as SpanControl;
            textBlock.Loaded += OnTextBlockLoaded;
        }

        private static void TrimRunWhitespace(RunControl run)
        {
            var skipTrimStart = Run.GetSkipTrimStart(run);
            var skipTrimEnd = Run.GetSkipTrimEnd(run);
            var text = run.Text;

            if (!skipTrimStart && text.FirstOrDefault() == ' ') {
                text = text.Substring(1);
            }

            if (!skipTrimEnd && text.LastOrDefault() == ' ') {
                text = text[0..^1];
            }

            run.Text = text;
        }

        static void OnTextBlockLoaded(object sender, EventArgs args)
        {
            var span = sender as SpanControl;
            span.Loaded -= OnTextBlockLoaded;

            var runs = span.Inlines
                .OfType<RunControl>()
                .ToList();

            foreach (var run in runs) {
                TrimRunWhitespace(run);
            }
        }
    }
}
