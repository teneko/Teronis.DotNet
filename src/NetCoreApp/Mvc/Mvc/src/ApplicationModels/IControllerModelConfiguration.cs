using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.Identity.Bearer.Controllers
{
    /// <summary>
    /// Interface provides configuration options for customizing <see cref="IControllerModelConfiguration"/>.
    /// </summary>
    public interface IControllerModelConfiguration
    {
        TypeInfo ControllerType { get; }

        /// <summary>
        /// Adds <see cref="IControllerModelConvention"/> instance to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="controllerConvention"></param>
        /// <returns></returns>
        IControllerModelConfiguration AddControllerConvention(IControllerModelConvention controllerConvention);
        /// <summary>
        /// Adds <see cref="IActionModelConvention"/> instance to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="actionConvention"></param>
        /// <returns></returns>
        IControllerModelConfiguration AddActionConvention(IActionModelConvention actionConvention);
        /// <summary>
        /// Adds <see cref="IParameterModelConvention"/> instance to <see cref="MvcOptions"/>
        /// </summary>
        /// <param name="parameterConvention"></param>
        /// <returns></returns>
        IControllerModelConfiguration AddParameterConvention(IParameterModelConvention parameterConvention);
        /// <summary>
        /// Adds <see cref="IParameterModelBaseConvention"/> instance to <see cref="MvcOptions"/>
        /// </summary>
        /// <param name="parameterBaseConvention"></param>
        /// <returns></returns>
        IControllerModelConfiguration AddParameterBaseConvention(IParameterModelBaseConvention parameterBaseConvention);
    }
}
