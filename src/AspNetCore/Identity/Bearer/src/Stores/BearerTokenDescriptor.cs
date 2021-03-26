// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.IdentityModel.Tokens;

namespace Teronis.AspNetCore.Identity.Bearer.Stores
{
    public class BearerTokenDescriptor : SecurityTokenDescriptor
    {
        public new SigningCredentials? SigningCredentials {
            get => base.SigningCredentials;
        }

        public BearerTokenDescriptor(SigningCredentials? signingCredentials) =>
            base.SigningCredentials = signingCredentials;
    }
}
