using Morse.Utils;
using System.Text;
using System.Text.RegularExpressions;

namespace Morse.Command
{
    public class ConsoleSender
    { 

        public void SendMessage(string message, bool onlyPalceholdersReplace)
        {
            Console.WriteLine();
            StringBuilder sb = new StringBuilder();
            string pattern = "&(1[0-5]|[0-9])";
            string[] split = message.Split('&');

            split = split.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (int i = 0; i < split.Length; i++)
            {
                if (!char.IsDigit(split[i][0]))
                {
                    split[i] = "15" + split[i];
                }
                sb.Append("&" + split[i]);
            }
            Regex colorRegex = new(pattern, RegexOptions.IgnoreCase);

            string[] messageSplit = colorRegex.Split(sb.ToString());

            for (int i = 0; i < messageSplit.Length; i++)
            {
                if (int.TryParse(messageSplit[i], out int result))
                {
                    Console.ForegroundColor = (ConsoleColor)int.Parse(messageSplit[i]);
                    Console.Write(StringUtils.Placeholders(messageSplit[i + 1], onlyPalceholdersReplace));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}