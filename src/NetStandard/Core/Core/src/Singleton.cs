// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis
{
    public sealed class Singleton : ISingleton
    {
        public static readonly Singleton Default = new Singleton();

        private Singleton() { }
    }
}
