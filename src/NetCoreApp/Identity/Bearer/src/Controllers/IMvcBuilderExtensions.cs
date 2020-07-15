using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Mvc;

namespace Teronis.Identity.Controllers
{
    public static partial class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddBearerSignInControllers(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.ConfigureApplicationPartManager(setup => {
                var types = new[] { typeof(BearerSignInController).GetTypeInfo() };
                var typesProvider = new TypesProvidingApplicationPart(types);
                setup.ApplicationParts.Add(typesProvider);
            });

            return mvcBuilder;
        }
    }
}
