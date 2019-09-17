using System;
using System.Collections.Generic;
using System.Text;
using Teronis.Reflection;

namespace Teronis.Extensions.NetStandard
{
    public static class VariableInfoSettingsExtensions
    {
        public static VariableInfoSettings DefaultIfNull(this VariableInfoSettings settings, bool seal)
        {
            settings = settings ?? new VariableInfoSettings();

            if (seal)
                settings.Seal();

            return settings;
        }
    }
}
