using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.Mvc.ApplicationModels
{
    /// <summary>
    /// Interface provides configuration options for customizing <see cref="ControllerModel"/>.
    /// </summary>
    public interface ISelectedControllerModelConfiguration
    {
        TypeInfo SelectedControllerType { get; }

        /// <summary>
        /// Adds <see cref="IControllerModelConvention"/> instance to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="controllerConvention"></param>
        /// <returns></returns>
        ISelectedControllerModelConfiguration AddControllerConvention(IControllerModelConvention controllerConvention);
        /// <summary>
        /// Adds <see cref="IActionModelConvention"/> instance to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="actionConvention"></param>
        /// <returns></returns>
        ISelectedControllerModelConfiguration AddActionConvention(IActionModelConvention actionConvention);
        /// <summary>
        /// Adds <see cref="IParameterModelConvention"/> instance to <see cref="MvcOptions"/>
        /// </summary>
        /// <param name="parameterConvention"></param>
        /// <returns></returns>
        ISelectedControllerModelConfiguration AddParameterConvention(IParameterModelConvention parameterConvention);
        /// <summary>
        /// Adds <see cref="IParameterModelBaseConvention"/> instance to <see cref="MvcOptions"/>
        /// </summary>
        /// <param name="parameterBaseConvention"></param>
        /// <returns></returns>
        ISelectedControllerModelConfiguration AddParameterBaseConvention(IParameterModelBaseConvention parameterBaseConvention);
    }
}
