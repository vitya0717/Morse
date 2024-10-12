using Microsoft.Extensions.Configuration;
using Morse.Exceptions;
using Morse.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morse.Command.MenuCommands
{
    public class Commands : ICommandExecutor
    {

        private static IConfigurationRoot cfg = Program.cfg.getConfig();

        public bool OnCommand(ConsoleSender sender, string cmd, string[] args)
        {
            Console.Clear();
            Program.MenuHeader();

            //current used command for the StringUtils# %cmd%
            StringUtils.Cmd = cmd;

            if (cmd.Length > 16)
            {
                sender.SendMessage(cfg.GetSection($"LanguageSettings:{StringUtils.Lang}:moreThan16length").Value!);
                return true;
            }

            if (cmd.Equals("help"))
            {
                foreach (var command in cfg.GetSection($"LanguageSettings:{StringUtils.Lang}:HelpPage").GetChildren().AsEnumerable())
                {
                    sender.SendMessage($"{command.Value}");
                }
                return true;
            }

            if (cmd.Equals("decode"))
            {
                if (args.Length == 0)
                {
                    sender.SendMessage("&12Please, enter a text to decode!");
                    return true;
                }
                Program.manager.decode(MergeCommandArguments(args).TrimEnd(' '));
                return true;
            }

            if (cmd.Equals("encode"))
            {
                if (args.Length == 0)
                {
                    sender.SendMessage("&12Please, enter a text to encode!");
                    return true;
                }
                Program.manager.encode(MergeCommandArguments(args).ToUpper().TrimEnd(' '));
                return true;
            }

            if (cmd.Equals("exit"))
            {
                string exit;
                do
                {
                    sender.SendMessage(cfg.GetSection($"LanguageSettings:{StringUtils.Lang}:exitText").Value!);
                    exit = Console.ReadLine()!;

                } while (!(exit.Equals("y") || exit.Equals("n")));

                if (exit.Equals("y"))
                {
                    Environment.Exit(0);
                }
                Console.Clear();
                Program.MenuHeader();
                return true;
            }

            if(cmd.Equals("changelang"))
            {
                if (args.Length == 0)
                {
                    sender.SendMessage("&12Please, enter a language to change it!");
                    return true;
                }

                var json = JObject.Parse(File.ReadAllText("appsettings.json"));
                json["AppSettings"]!["lang"] = args[0];
                File.WriteAllText("appsettings.json", json.ToString());

                Process.Start(new ProcessStartInfo
                {
                    FileName = Process.GetCurrentProcess().MainModule!.FileName,
                    UseShellExecute = true
                });
                Environment.Exit(0);
                return true;
            }


            if (cmd.Equals("change"))
            {
                if (args.Length == 0)
                {
                    sender.SendMessage("&12Please, enter short and long characters!");
                    return true;
                }
                char shortChar = char.Parse(args[0]);
                char longChar = char.Parse(args[1]);
                try
                {
                    Program.manager.ChangeMorseCharacters(shortChar, longChar);
                    sender.SendMessage("&10Successfully changed the morse characters!");
                    sender.SendMessage(shortChar+"");
                    sender.SendMessage(longChar+"");
                    return true;
                }
                catch (InvalidCharacterCodeException ex)
                {
                    sender.SendMessage($"&12{ex.Message}");
                    return true;
                }
            }


            string commandReference = CompareCommandsToArgs(cmd.ToLower());
            sender.SendMessage($"&12%cmd% command not found! try this: {commandReference}");
            return true;
        }


        public string MergeCommandArguments(string[] arguments)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < arguments.Length; i++)
            {
                sb.Append(arguments[i]).Append(' ');
            }
            return sb.ToString();
        }

        //It's just a function that can figure out which command you want to use when you mistype a command.
        private static string CompareCommandsToArgs(string argument)
        {
            int argumentLength = argument.Length;
            string output = "help command :c";
            Dictionary<string, int> commandsWithLength = new Dictionary<string, int>();
            foreach (var item in cfg.GetSection($"LanguageSettings:{StringUtils.Lang}:HelpPage")!.GetChildren().AsEnumerable())
            {
                commandsWithLength.Add(item.Key, item.Key.Length);
            }
            int max = 0;
            foreach (var item in commandsWithLength.OrderByDescending(s => s.Value))
            {
                double percentage = CalcPercentage(argument.Length, item.Value);
                if (percentage >= 60)
                {
                    int temp = 0;
                    foreach (var letter in item.Key)
                    {
                        if (argument.Contains(letter))
                        {
                            temp++;
                        }
                    }
                    if (temp > max)
                    {
                        max = temp;
                        output = item.Key;
                    }
                    temp = 0;
                }
            }
            return output;
        }

        //Sub-function for the CompareCommandsToArgs function
        private static double CalcPercentage(double length, double value)
        {
            return (length > value) ? Math.Round((value / length) * 100) : Math.Round((length / value) * 100);
        }
    }
}
