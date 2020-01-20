using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Text.Json.Serialization
{
    public interface IVariablesClusionHelper
    {
        void ConsiderVariable(Type declaringType, params string[] propertyName);
    }
}
