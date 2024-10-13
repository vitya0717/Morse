using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morse.Utils
{
    public class StringUtils
    {

        public static string? Cmd { get; set; }
        public static string? CmdReference { get; set; }

        public static char? ShortChar { get; set; }
        public static char? LongChar { get; set; }

        public static string? Lang { get; set; } = Program.cfg.GetConfig().GetSection("AppSettings:lang").Value;

        public static string Placeholders(string value)
        {
            value = variables(value);

            return value;
        }

        public static string Placeholders(string value, bool onlyPlaceholders)
        {
            value = variables(value);

            if (!onlyPlaceholders)
            {
                value = value.ToLower();
                value = char.ToUpper(value[0]) + value[1..];
            }
            return value;
        }

        private static string variables(string value)
        {
            if (Cmd != null)
            {
                value = value.Replace("%cmd%", Cmd);
            }
            if (Lang != null)
            {
                value = value.Replace("%lang%", Lang);
            }
            if (CmdReference != null)
            {
                value = value.Replace("%cmd_ref%", CmdReference);
            }
            if(ShortChar != null)
            {
                value = value.Replace("%shortchar%", ShortChar.ToString());
            }
            if (LongChar != null)
            {
                value = value.Replace("%longchar%", LongChar.ToString());
            }

            return value;
        }
    }
}
