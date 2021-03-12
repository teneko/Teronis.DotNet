namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class IJSFacadeDictionaryBuilderExtensions
    {
        public static IJSFacadeDictionaryBuilder AddDefault(this IJSFacadeDictionaryBuilder builder) =>
            builder.AddFacade(module => module);
    }
}
