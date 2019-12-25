using System.Text;

namespace Teronis.Text
{
    public class StringSeparationHelper
    {
        public string StringSeparator { get; private set; }
        public int LastSeperationIndex { get; private set; }

        public StringSeparationHelper(string stringSeparator, int initialLastSeparationIndex)
        {
            StringSeparator = stringSeparator;
            LastSeperationIndex = initialLastSeparationIndex;
        }

        public StringSeparationHelper(string stringSeparator) : this(stringSeparator, -1) { }

        public void SetStringSeparator(ref string inputText)
        {
            if (LastSeperationIndex >= 0) {
                var builder = new StringBuilder(inputText, (inputText.Length + StringSeparator.Length) * 2);
                SetStringSeparator(builder);
                inputText = builder.ToString();
            }

            LastSeperationIndex = inputText.Length;
        }

        public void SetStringSeparator(StringBuilder builder)
        {
            if (LastSeperationIndex >= 0)
                builder.Insert(LastSeperationIndex, StringSeparator);

            LastSeperationIndex = builder.Length;
        }
    }
}
