namespace Teronis.Microsoft.JSInterop.Facade
{
    public static class IJSFacadeDictionaryBuilderExtensions
    {
        public static IJSFacadeDictionaryBuilder AddDefault(this IJSFacadeDictionaryBuilder builder) =>
            builder.AddModuleWrapper(module => module);
    }
}
