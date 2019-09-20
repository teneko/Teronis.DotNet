using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Teronis.Configuration
{
    public class ApplicationSettingsAssistance
    {
        public ApplicationSettingsBase Settings { get; private set; }
        public bool AreSettingsUnsaved { get; private set; }

        public ApplicationSettingsAssistance(ApplicationSettingsBase settingsBase)
        {
            Settings = settingsBase;
            Settings.PropertyChanged += SettingsBase_PropertyChanged;
            Settings.SettingsSaving += SettingsBase_SettingsSaving;
        }

        private void SettingsBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) 
            => AreSettingsUnsaved = true;

        private void SettingsBase_SettingsSaving(object sender, System.ComponentModel.CancelEventArgs e) 
            => AreSettingsUnsaved = false;

        /// <summary>
        /// Saves only to disk if at least one property has been changed.
        /// </summary>
        public void Save()
        {
            if (AreSettingsUnsaved)
                Settings.Save();
        }

        /// <summary>
        /// Reloads only from disk if at least one property has been changed.
        /// </summary>
        public void Reload()
        {
            if (AreSettingsUnsaved)
                Settings.Reload();
        }

        public void DetachEventHandlers()
        {
            Settings.PropertyChanged -= SettingsBase_PropertyChanged;
            Settings.SettingsSaving -= SettingsBase_SettingsSaving;
        }
    }
}
