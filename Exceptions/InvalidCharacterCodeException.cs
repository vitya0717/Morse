using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morse.Exceptions
{
    public class InvalidCharacterCodeException : Exception
    {

        public override string Message => "There was an error setting character codes.\nAvoid using the same symbol for both.";

        public InvalidCharacterCodeException()
        {

        }
    }
}
