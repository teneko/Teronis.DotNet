using Teronis.Reflection;

namespace Teronis.Extensions
{
    public static class VariableInfoSettingsExtensions
    {
        internal static VariableInfoDescriptor DefaultIfNull(this VariableInfoDescriptor? descriptor, bool seal)
        {
            descriptor ??= new VariableInfoDescriptor();

            if (seal) {
                descriptor.Seal();
            }

            return descriptor;
        }
    }
}
