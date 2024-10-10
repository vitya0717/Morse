using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morse.Models
{
    public class MorseCode
    {

        public string Letter { get; set; } = null!;
        public string LetterCode { get; set; } = null!;

        public MorseCode(string letter, string letterCode) 
        { 
            Letter = letter;
            LetterCode = letterCode;
        }
    }
}
