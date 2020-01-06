using System.Windows;
using RunControl = System.Windows.Documents.Run;

namespace Teronis.AttachedProperties
{
    public static class Run
    {
        public static readonly DependencyProperty SkipTrimEndProperty
            = DependencyProperty.RegisterAttached("SkipTrimEnd", typeof(bool), typeof(Run),
                new PropertyMetadata(false));

        public static bool GetSkipTrimEnd(RunControl run)
            => (bool)run.GetValue(SkipTrimEndProperty);

        public static void SetSkipTrimEnd(RunControl run, bool value)
            => run.SetValue(SkipTrimEndProperty, value);

        public static readonly DependencyProperty SkipTrimStartProperty
            = DependencyProperty.RegisterAttached("SkipTrimStart", typeof(bool), typeof(Run),
                new PropertyMetadata(false));

        public static bool GetSkipTrimStart(RunControl run)
            => (bool)run.GetValue(SkipTrimStartProperty);

        public static void SetSkipTrimStart(RunControl run, bool value)
            => run.SetValue(SkipTrimStartProperty, value);
    }
}
