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
            manager.LoadCodeTable("code.txt");

            OpenMenu();

        }

        public static void MenuHeader()
        {
            string ver = cfg.getConfig().GetSection("Appsettings:Version").Value!;
            string selectedLangText = cfg.getConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:selectedLangText").Value!;
            sender.SendMessage($"MorseTranslator - v{ver} &3| &15{selectedLangText}&9%lang%\n");

        }

        private static void OpenMenu()
        {
            bool exit = false;

            MenuHeader();

            Commands commands = new();

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(cfg.getConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:commandInput").Value);
                Console.ForegroundColor= ConsoleColor.White;
                
                string commandInput = Console.ReadLine()!;

                commands.OnCommand(sender, commandInput.Split(' ')[0], commandInput.Split(' ').Where(s => s != commandInput.Split(' ')[0]).ToArray());
            }
            while (!exit);
        }

    }
}
