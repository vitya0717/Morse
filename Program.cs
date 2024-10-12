using Morse.Command;
using Morse.Command.MenuCommands;
using Morse.Configuration;
using Morse.Models;
using Morse.Utils;

namespace Morse
{
    public class Program
    {
        public static MorseManager manager = new MorseManager();
        public static ConsoleSender sender = new();
        public static ConfigurationManager cfg = new ConfigurationManager();


        static void Main(string[] args)
        {
            /*
             *  Default letters/symbols:
             *      short: .
             *      long: -
             *  Charaters are changeable in MorseManager class
             *      manager#changeMorseCharacters(char param, char param);
             *      manager#changeShortChar(char param);
             *      manager#changeLongChar(char param);
             *
             */
            manager.LoadCodeTable();

            OpenMenu();

        }

        public static void MenuHeader()
        {
            string ver = cfg.getConfig().GetSection("Appsettings:Version").Value!;
            string selectedLangText = cfg.getConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:selectedLangText").Value!;
            sender.SendMessage($"MorseTranslator - v{ver} &3| &15{selectedLangText}&9%lang%\n");
            sender.SendMessage($"&2Default command is &6help &2to get informations about the commands!");
            sender.SendMessage($"&2This is a Morse Translator that can encode and decode text and morse code!\n");
            sender.SendMessage($"&15----------------------------------------------------------------------------");
        }

        private static void OpenMenu()
        {
            MenuHeader();

            Commands commands = new();

            do
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(cfg.getConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:commandInput").Value);
                Console.ForegroundColor= ConsoleColor.White;
                
                string commandInput = Console.ReadLine()!;

                if (string.IsNullOrWhiteSpace(commandInput)) continue;

                string[] arguments = commandInput.Split(' ').Where(s => s != commandInput.Split(' ')[0]).ToArray();

                commands.OnCommand(sender, commandInput.Split(' ')[0], arguments);
            }
            while (true);
        }

    }
}
