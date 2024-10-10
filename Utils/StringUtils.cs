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
        public static string? Lang { get; set; } = Program.cfg.getConfig().GetSection("AppSettings:lang").Value;

        public static string Placeholders(string value)
        {
            if(Cmd != null)
            {
                value = value.Replace("%cmd%", Cmd);
            }
            if(Lang != null)
            {
                value = value.Replace("%lang%", Lang);
            }
            return value;
        }
    }
}
