using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.Mvc.ApplicationModels
{
    public struct DelegatedControllerModelConfiguration
    {
        private readonly ISelectedControllerModelConfiguration configuration;

        public DelegatedControllerModelConfiguration(ISelectedControllerModelConfiguration configuration) =>
            this.configuration = configuration;

        public ISelectedControllerModelConfiguration AddActionConvention(IActionModelConvention actionConvention) =>
            configuration.AddActionConvention(actionConvention);

        public ISelectedControllerModelConfiguration AddControllerConvention(IControllerModelConvention controllerConvention) =>
            configuration.AddControllerConvention(controllerConvention);

        public ISelectedControllerModelConfiguration AddParameterBaseConvention(IParameterModelBaseConvention parameterBaseConvention) =>
            configuration.AddParameterBaseConvention(parameterBaseConvention);

        public ISelectedControllerModelConfiguration AddParameterConvention(IParameterModelConvention parameterConvention) =>
            configuration.AddParameterConvention(parameterConvention);
    }
}
