namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class IJSFacadeDictionaryBuilderExtensions
    {
        public static IJSFacadeDictionaryConfiguration AddDefault(this IJSFacadeDictionaryConfiguration builder) =>
            builder.Add(module => module);
    }
}
