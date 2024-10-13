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

        private readonly JObject json = JObject.Parse(File.ReadAllText("appsettings.json"));

        private static IConfigurationSection config = Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}")!;

        public bool OnCommand(ConsoleSender sender, string cmd, string[] args)
        {
            Console.Clear();
            Program.MenuHeader();

            //current used command for the StringUtils# %cmd%
            StringUtils.Cmd = cmd;

            if (cmd.Length > 16)
            {
                sender.SendMessage(Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:moreThan16length").Value!, false);
                return true;
            }

            //help panel
            if (cmd.Equals("help"))
            {
                foreach (var command in Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:HelpPage").GetChildren().AsEnumerable())
                {
                    sender.SendMessage($"{command.Value}", false);
                }
                return true;
            }

            //decode morse into regular text
            if (cmd.Equals("decode"))
            {
                if (args.Length == 0)
                {
                    sender.SendMessage(Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:decodeUsage").Value!,false);
                    return true;
                }
                Program.manager.decode(MergeCommandArguments(args).TrimEnd(' '));
                return true;
            }

            //encode text into morse
            if (cmd.Equals("encode"))
            {
                if (args.Length == 0)
                {
                    sender.SendMessage(Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:encodeUsage").Value!, false);
                    return true;
                }
                Program.manager.encode(MergeCommandArguments(args).ToUpper().TrimEnd(' '));
                return true;
            }

            //Exit from the application
            if (cmd.Equals("exit"))
            {
                string exit;
                do
                {
                    sender.SendMessage(Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:exitText").Value!,false);
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

            //change language
            if(cmd.Equals("changelang"))
            {
                if (args.Length == 0)
                {
                    sender.SendMessage(Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:changeLangUsage").Value!, false);
                    return true;
                }
                var vane = Program.cfg.GetConfig().GetSection("LanguageSettings").GetChildren().AsEnumerable();

                foreach (var v in vane)
                {
                   if(v.Key == args[0])
                    {
                        //modifying language in appsettings.json
                        json["AppSettings"]!["lang"] = args[0];
                        File.WriteAllText("appsettings.json", json.ToString());

                        StringUtils.Lang = json["AppSettings"]!["lang"]!.ToString();

                        Console.Clear();
                        Program.MenuHeader();
                        return true;
                    }
                }

                sender.SendMessage(Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:languageNotFound").Value!, false);
                return true;
            }


            if (cmd.Equals("change"))
            {
                if (args.Length == 0)
                {
                    sender.SendMessage(Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:changeCharactersUsage").Value!,false);
                    return true;
                }

                char shortChar = char.Parse(args[0]);
                char longChar = char.Parse(args[1]);

                try
                {
                    
                    Program.manager.ChangeMorseCharacters(shortChar, longChar);

                    sender.SendMessage(Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:successChangeCharacters").Value!, false);

                    StringUtils.ShortChar = shortChar;
                    StringUtils.LongChar = longChar;

                    Console.Clear();
                    Program.MenuHeader();

                    return true;
                }
                catch (InvalidCharacterCodeException ex)
                {
                    sender.SendMessage($"&12{ex.Message}",false);
                    return true;
                }
            }

            string commandReference = CompareCommandsToArgs(cmd.ToLower());

            StringUtils.CmdReference = commandReference;
            //sender.SendMessage(config.GetSection("commandNotFound").Value!, false);
            sender.SendMessage(Program.cfg.GetConfig().GetSection($"LanguageSettings:{StringUtils.Lang}:commandNotFound").Value!, false);
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
            foreach (var item in config.GetSection("HelpPage")!.GetChildren().AsEnumerable())
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
