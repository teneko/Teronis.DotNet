using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Identity.Controllers
{
    public static partial class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddIdentityControllers(this IMvcBuilder mvcBuilder)
        {
            AddAccountControllers(mvcBuilder);
            AddSignInControllers(mvcBuilder);
            return mvcBuilder;
        }
    }
}
