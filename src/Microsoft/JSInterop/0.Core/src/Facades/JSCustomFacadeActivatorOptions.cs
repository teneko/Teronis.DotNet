namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSCustomFacadeActivatorOptions
    {
        public IJSCustomFacadeDictionaryBuilder JSFacadeDictionaryConfiguration =>
            JSFacadeDictionaryBuilder;

        internal readonly JSCustomFacadeDictionaryBuilder JSFacadeDictionaryBuilder;

        public JSCustomFacadeActivatorOptions() =>
            JSFacadeDictionaryBuilder = new JSCustomFacadeDictionaryBuilder()
                .AddDefault();
    }
}
