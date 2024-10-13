using Morse.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morse.Exceptions
{
    public class InvalidCharacterCodeException : Exception
    {
        public override string Message => Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:InvalidCharacterCodesMessage").Value!;
    }
}
