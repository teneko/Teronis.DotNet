using System.Text;

namespace Teronis.Text
{
    public struct StringSeparationHelper
    {
        public string StringSeparator { get; private set; }
        public int LastSeparationIndex { get; private set; }

        public StringSeparationHelper(string stringSeparator, int initialLastSeparationIndex)
        {
            StringSeparator = stringSeparator;
            LastSeparationIndex = initialLastSeparationIndex;
        }

        public StringSeparationHelper(string stringSeparator)
            : this(stringSeparator, -1) { }

        public void SetStringSeparator(ref string inputText, int? lastSeperationIndex = null)
        {
            if (LastSeparationIndex >= 0) {
                var builder = new StringBuilder(inputText, (inputText.Length + StringSeparator.Length) * 2);
                SetStringSeparator(builder);
                inputText = builder.ToString();
            }

            LastSeparationIndex = lastSeperationIndex ?? inputText.Length;
        }

        public void SetStringSeparator(StringBuilder builder, int? lastSeperationIndex = null)
        {
            if (LastSeparationIndex >= 0) {
                builder.Insert(LastSeparationIndex, StringSeparator);
            }

            LastSeparationIndex = lastSeperationIndex ?? builder.Length;
        }
    }
}
