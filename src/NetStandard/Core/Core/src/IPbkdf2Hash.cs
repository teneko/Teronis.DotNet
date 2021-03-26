// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.



namespace Teronis
{
    public interface IPbkdf2Hash
    {
        string? Hash { get; set; }
        string? Salt { get; set; }
        int Interations { get; set; }
    }
}
