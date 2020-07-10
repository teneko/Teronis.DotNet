using System;

namespace Teronis.Text.Json.Serialization
{
    public interface IVariablesClusionHelper
    {
        void ConsiderVariable(Type declaringType, params string[] propertyName);
    }
}
