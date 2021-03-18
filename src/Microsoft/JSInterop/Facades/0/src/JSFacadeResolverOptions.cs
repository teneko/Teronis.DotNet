namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadeResolverOptions
    {
        public IJSFacadeDictionaryConfiguration JSFacadeDictionaryConfiguration =>
            JSFacadeDictionaryBuilder;

        internal readonly JSFacadeDictionaryBuilder JSFacadeDictionaryBuilder;

        public JSFacadeResolverOptions() =>
            JSFacadeDictionaryBuilder = new JSFacadeDictionaryBuilder()
                .AddDefault();
    }
}
