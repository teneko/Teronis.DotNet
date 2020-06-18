using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Identity.Controllers
{
    public static partial class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddSignInControllers(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.ConfigureApplicationPartManager(setup => {
                var types = new[] { typeof(SignInController).GetTypeInfo() };
                var typesProvider = new TypesProvidingApplicationPart(types);
                setup.ApplicationParts.Add(typesProvider);
            });

            return mvcBuilder;
        }
    }
}
