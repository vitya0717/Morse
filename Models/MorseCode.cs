using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morse.Models
{
    public class MorseCode
    {

        public char Letter { get; set; }
        public string LetterCode { get; set; } = null!;

        public MorseCode(char letter, string letterCode) 
        { 
            Letter = letter;
            LetterCode = letterCode;
        }
    }
}
