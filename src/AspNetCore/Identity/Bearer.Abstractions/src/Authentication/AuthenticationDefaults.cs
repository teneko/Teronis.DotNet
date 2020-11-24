namespace Teronis.AspNetCore.Identity.Bearer.Authentication
{
    public static class AuthenticationDefaults
    {
        public const string IdentityBasicScheme = nameof(Teronis) + nameof(IdentityBasicScheme);
        public const string IdentityRefreshTokenBearerScheme = nameof(Teronis) + nameof(IdentityRefreshTokenBearerScheme);
        public const string AccessTokenBearerScheme = nameof(Teronis) + nameof(AccessTokenBearerScheme);
    }
}
