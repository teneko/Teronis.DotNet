using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.Mvc.ApplicationModels
{
    /// <summary>
    /// Provides configuration for <see cref="ControllerModel"/> by adding conventions.
    /// </summary>
    public class ControllerModelConfiguration : ISelectedControllerModelConfiguration
    {
        public TypeInfo SelectedControllerType { get; }

        public List<IControllerModelConvention> ControllerConventions { get; }
        public List<IActionModelConvention> ActionConventions { get; }
        public List<IParameterModelConvention> ParameterConventions { get; }
        public List<IParameterModelBaseConvention> ParameterBaseConventions { get; }

        public ControllerModelConfiguration(TypeInfo controllerType)
        {
            SelectedControllerType = controllerType;
            ControllerConventions = new List<IControllerModelConvention>();
            ActionConventions = new List<IActionModelConvention>();
            ParameterConventions = new List<IParameterModelConvention>();
            ParameterBaseConventions = new List<IParameterModelBaseConvention>();
        }

        public ISelectedControllerModelConfiguration AddControllerConvention(IControllerModelConvention controllerConvention)
        {
            controllerConvention = controllerConvention ?? throw new ArgumentNullException(nameof(controllerConvention));
            ControllerConventions.Add(controllerConvention);
            return this;
        }

        public ISelectedControllerModelConfiguration AddActionConvention(IActionModelConvention actionConvention)
        {
            actionConvention = actionConvention ?? throw new ArgumentNullException(nameof(actionConvention));
            ActionConventions.Add(actionConvention);
            return this;
        }

        public ISelectedControllerModelConfiguration AddParameterConvention(IParameterModelConvention parameterConvention)
        {
            parameterConvention = parameterConvention ?? throw new ArgumentNullException(nameof(parameterConvention));
            ParameterConventions.Add(parameterConvention);
            return this;
        }

        public ISelectedControllerModelConfiguration AddParameterBaseConvention(IParameterModelBaseConvention parameterBaseConvention)
        {
            parameterBaseConvention = parameterBaseConvention ?? throw new ArgumentNullException(nameof(parameterBaseConvention));
            ParameterBaseConventions.Add(parameterBaseConvention);
            return this;
        }
    }
}
