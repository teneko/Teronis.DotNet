namespace Teronis.AddOn.Microsoft.JSInterop.Facade
{
    public static class JSFacadeDictionaryBuilderExtensions
    {
        public static JSFacadeDictionaryBuilder AddDefault(this JSFacadeDictionaryBuilder builder) =>
            builder.AddModuleWrapper(module => module);
    }
}
