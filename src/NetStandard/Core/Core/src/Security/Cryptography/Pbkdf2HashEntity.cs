

namespace Teronis.Security.Cryptography
{
    public class Pbkdf2HashEntity : IPbkdf2Hash
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
        public int Interations { get; set; }
    }
}
