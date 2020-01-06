using System;
using System.Windows;
using Teronis.Wpf.Localization;

namespace Teronis.Tools.Wpf
{
    public static class MessageBoxTools
    {
        public static MessageBoxResult ShowError(Exception error)
        {
            var caption = StringResources.AnErrorOccuredExclamation;
#if DEBUG
            return MessageBox.Show(error.ToString(), caption);
#else
            return MessageBox.Show(error.Message, caption);
#endif
        }

        public static MessageBoxResult ShowSureInterrogativeQuestion()
            => MessageBox.Show(StringResources.SureInterrogativeSentence, StringResources.SecurityQuestionHeader, MessageBoxButton.YesNo);
    }
}
