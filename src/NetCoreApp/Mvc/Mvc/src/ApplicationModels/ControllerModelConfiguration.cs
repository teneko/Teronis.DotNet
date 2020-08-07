using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Teronis.Identity.Bearer.Controllers;

namespace Teronis.Mvc.ApplicationModels
{
    public class ControllerModelConfiguration : IControllerModelConfiguration
    {
        public TypeInfo ControllerType { get; }

        public List<IControllerModelConvention> ControllerConventions { get; }
        public List<IActionModelConvention> ActionConventions { get; }
        public List<IParameterModelConvention> ParameterConventions { get; }
        public List<IParameterModelBaseConvention> ParameterBaseConventions { get; }

        public ControllerModelConfiguration(TypeInfo controllerType)
        {
            ControllerType = controllerType;
            ControllerConventions = new List<IControllerModelConvention>();
            ActionConventions = new List<IActionModelConvention>();
            ParameterConventions = new List<IParameterModelConvention>();
            ParameterBaseConventions = new List<IParameterModelBaseConvention>();
        }

        public IControllerModelConfiguration AddControllerConvention(IControllerModelConvention controllerConvention)
        {
            controllerConvention = controllerConvention ?? throw new ArgumentNullException(nameof(controllerConvention));
            ControllerConventions.Add(controllerConvention);
            return this;
        }

        public IControllerModelConfiguration AddActionConvention(IActionModelConvention actionConvention)
        {
            actionConvention = actionConvention ?? throw new ArgumentNullException(nameof(actionConvention));
            ActionConventions.Add(actionConvention);
            return this;
        }

        public IControllerModelConfiguration AddParameterConvention(IParameterModelConvention parameterConvention)
        {
            parameterConvention = parameterConvention ?? throw new ArgumentNullException(nameof(parameterConvention));
            ParameterConventions.Add(parameterConvention);
            return this;
        }

        public IControllerModelConfiguration AddParameterBaseConvention(IParameterModelBaseConvention parameterBaseConvention)
        {
            parameterBaseConvention = parameterBaseConvention ?? throw new ArgumentNullException(nameof(parameterBaseConvention));
            ParameterBaseConventions.Add(parameterBaseConvention);
            return this;
        }
    }
}
