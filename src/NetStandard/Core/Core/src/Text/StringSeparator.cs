// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Teronis.Text
{
    public struct StringSeparator
    {
        public string Separator { get; private set; }
        public int LastSeparationIndex { get; private set; }

        public StringSeparator(string stringSeparator, int initialLastSeparationIndex)
        {
            Separator = stringSeparator;
            LastSeparationIndex = initialLastSeparationIndex;
        }

        public StringSeparator(string separator)
            : this(separator, initialLastSeparationIndex: -1) { }

        public void SetSeparator(ref string inputText, int? lastSeperationIndex = null)
        {
            if (LastSeparationIndex >= 0) {
                var builder = new StringBuilder(inputText, (inputText.Length + Separator.Length) * 2);
                SetSeparator(builder);
                inputText = builder.ToString();
            }

            LastSeparationIndex = lastSeperationIndex ?? inputText.Length;
        }

        public void SetSeparator(StringBuilder builder, int? lastSeperationIndex = null)
        {
            if (LastSeparationIndex >= 0) {
                builder.Insert(LastSeparationIndex, Separator);
            }

            LastSeparationIndex = lastSeperationIndex ?? builder.Length;
        }
    }
}
