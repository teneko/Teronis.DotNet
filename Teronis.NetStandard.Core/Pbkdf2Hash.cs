using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Teronis.NetStandard
{
    [ComplexType]
    public class Pbkdf2Hash : IPbkdf2Hash
    {
        [Required]
        public string Hash { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public int Interations { get; set; }
    }
}
