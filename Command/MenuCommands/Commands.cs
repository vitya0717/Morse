using Microsoft.Extensions.Configuration;
using Morse.Exceptions;
using Morse.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morse.Command.MenuCommands
{
    public class Commands : ICommandExecutor
    {
        private IConfigurationRoot cfg = Program.cfg.getConfig();

        public bool OnCommand(ConsoleSender sender, string cmd, string[] args)
        {
            Console.Clear();

            Program.MenuHeader();

            StringUtils.Cmd = cmd;

            if(cmd.Length > 16)
            {
                sender.SendMessage(cfg.GetSection($"LanguageSettings:{StringUtils.Lang}:moreThan16length").Value!);
                return true;
            }

            if (cmd.Equals("help"))
            {
                sender.SendMessage(cfg.GetSection($"LanguageSettings:{StringUtils.Lang}:HelpPage:change").Value!);
                sender.SendMessage(cfg.GetSection($"LanguageSettings:{StringUtils.Lang}:HelpPage:changeLang").Value!);
                sender.SendMessage(cfg.GetSection($"LanguageSettings:{StringUtils.Lang}:HelpPage:decode").Value!);
                sender.SendMessage(cfg.GetSection($"LanguageSettings:{StringUtils.Lang}:HelpPage:encode").Value!);
                sender.SendMessage(cfg.GetSection($"LanguageSettings:{StringUtils.Lang}:HelpPage:exit").Value!);
                return true;
            }

            if (cmd.Equals("decode"))
            {
                if(args.Length == 0)
                {
                    sender.SendMessage("");
                    Console.WriteLine();
                    return true;
                }

                sender.SendMessage("&4Hello&15Hungary&2Csumi");
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

            if (cmd.Equals("change"))
            {
                if (args.Length == 0)
                {
                    sender.SendMessage("&7Usage: &10change &12<short charater> <long character>");
                    return true;
                }
                if (args.Length == 1)
                {
                    sender.SendMessage("&7Usage: &10change <short charater> &12<long character>");
                    return true;
                }

                char shortChar = char.Parse(args[0]);
                char longChar = char.Parse(args[1]);

                try
                {
                    Program.manager.ChangeMorseCharacters(shortChar, longChar);
                    sender.SendMessage("&10Successfully changed the morse characters!");
                    return true;
                }
                catch (InvalidCharacterCodeException ex)
                {
                    sender.SendMessage($"&12{ex.Message}");
                    return true;
                }
            }


            sender.SendMessage("&12%cmd% command not found!");
            return true;
        }
        /*private static string compareCommandsToArgs(string argument)
        {
            int argumentLength = argument.Length;
            string output = "Sorry, but I don't have any ide what you wanna do :c";
            Dictionary<string, int> commandsWithLength = new Dictionary<string, int>();
            foreach (var item in commands)
            {
                commandsWithLength.Add(item, item.Length);
            }
            int max = 0;
            foreach (var item in commandsWithLength.OrderByDescending(s => s.Value))
            {
                double percentage = calcPercentage(argument.Length, item.Value);
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

        private static double calcPercentage(double length, double value)
        {
            return (length > value) ? Math.Round((value / length) * 100) : Math.Round((length / value) * 100);
        }*/
    }
}
