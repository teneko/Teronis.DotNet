namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class IJSFacadeDictionaryBuilderExtensions
    {
        public static IJSCustomFacadeDictionaryBuilder AddDefault(this IJSCustomFacadeDictionaryBuilder builder) =>
            builder.Add(module => module);
    }
}
