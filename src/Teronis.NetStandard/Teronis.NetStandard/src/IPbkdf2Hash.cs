

namespace Teronis
{
    public interface IPbkdf2Hash
    {
        string Hash { get; set; }
        string Salt { get; set; }
        int Interations { get; set; }
    }
}
