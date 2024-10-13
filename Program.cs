using Morse.Command;
using Morse.Command.MenuCommands;
using Morse.Configuration;
using Morse.Models;
using Morse.Utils;

namespace Morse
{
    public class Program
    {
        public static ConfigurationManager cfg = new ConfigurationManager();
        public static MorseManager manager = new MorseManager();
        public static ConsoleSender sender = new();
        


        static void Main(string[] args)
        {
            manager.LoadCodeTable();
            OpenMenu();
        }

        public static void MenuHeader()
        {
            string ver = cfg.GetConfig().GetSection("Appsettings:Version").Value!;
            string selectedLangText = cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:selectedLangText").Value!;
            sender.SendMessage($"MorseTranslator - v{ver} &3| {selectedLangText}\n", true);
            foreach (var line in cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:WelcomeMessage").GetChildren().AsEnumerable())
            {
                sender.SendMessage(line.Value!.ToString(), true);
            }
        }

        private static void OpenMenu()
        {
            MenuHeader();

            Commands commands = new();

            do
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:commandInput").Value);
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
