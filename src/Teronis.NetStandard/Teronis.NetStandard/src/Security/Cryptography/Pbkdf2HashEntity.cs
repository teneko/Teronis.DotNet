using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teronis.Security.Cryptography
{
    [ComplexType]
    public class Pbkdf2HashEntity : IPbkdf2Hash
    {
        [Required]
        public string Hash { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public int Interations { get; set; }
    }
}
