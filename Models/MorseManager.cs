using Morse.Command;
using Morse.Exceptions;
using Morse.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Morse.Models
{
    public class MorseManager
    {

        public List<MorseCode> loadedMorses = new List<MorseCode>();

        private char _shortCharacter = '.';
        private char _longCharacter = '-';

        public char ShortCharacter
        {
            get
            { return _shortCharacter; }
            set
            {
                if (_longCharacter == value)
                {
                    throw new InvalidCharacterCodeException();
                }
                _shortCharacter = value;
            }
        }

        public char LongCharacter
        {
            get
            { return _longCharacter; }
            set
            {
                if (_shortCharacter == value)
                {
                    throw new InvalidCharacterCodeException();
                }
                _longCharacter = value;
            }
        }

        public MorseManager()
        {

        }

        public MorseManager(char shortChar, char longChar)
        {
            ShortCharacter = shortChar;
            LongCharacter = longChar;
        }

        public void LoadCodeTable()
        {
            try
            {
                var morseCollection = Program.cfg.getConfig().GetSection($"CharacterSet").GetChildren().AsEnumerable();

                foreach (var item in morseCollection)
                {
                    MorseCode temp = new MorseCode(char.Parse(item.Key), item.Value!);

                    loadedMorses.Add(temp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //decode the given morse code into regular text!
        public void decode(string morseCode)
        {
            string[] split = morseCode.Split(' ');
            StringBuilder outputText = new StringBuilder();

            foreach (var match in split)
            {
                var letter = loadedMorses.FirstOrDefault(t => t.LetterCode.Equals(match))!;
                if(letter == null)
                {
                    outputText.Append('#');
                } else
                {
                    outputText.Append(letter!.Letter);
                }
            }
            Program.sender.SendMessage(outputText.ToString());
        }

        public void encode(string text)
        {
            StringBuilder outputText = new StringBuilder();

            foreach (var item in text)
            {
                var letter = loadedMorses.FirstOrDefault(t => t.Letter == item)!;
                if (letter == null)
                {
                    outputText.Append('#');
                }
                else
                {
                    outputText.Append(letter.LetterCode!).Append(' ');
                }
            }
            Program.sender.SendMessage(outputText.ToString());
        }

        public void ChangeMorseCharacters(char shortChar, char longChar)
        {
            if (shortChar == longChar)
            {
                throw new InvalidCharacterCodeException();
            }
            for (int i = 0; i < loadedMorses.Count; i++)
            {
                loadedMorses[i].LetterCode = loadedMorses[i].LetterCode
                    .Replace(_shortCharacter, shortChar)
                    .Replace(_longCharacter, longChar);
            }
            ShortCharacter = shortChar;
            LongCharacter = longChar;
        }
    }
}
