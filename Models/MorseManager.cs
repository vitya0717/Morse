using Morse.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            {
                return _shortCharacter;
            }
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
            {
                return _longCharacter;
            }
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

        public void LoadCodeTable(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (var item in lines)
                {
                    string[] split = item.Split(';');
                    MorseCode temp = new MorseCode(split[0], split[1]);
                    loadedMorses.Add(temp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void WriteOutCodes()
        {
            foreach (MorseCode code in loadedMorses)
            {
                Console.WriteLine("{0} ---> {1}", code.Letter, code.LetterCode);
            }
        }

        public void ChangeMorseCharacters(char shortChar, char longChar)
        {
            ShortCharacter = shortChar;
            LongCharacter = longChar;
        }

        public void ChangeShortChar(char shortChar)
        {
            try
            {
                ShortCharacter = shortChar;
            }
            catch (InvalidCharacterCodeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ChangeLongChar(char longChar)
        {
            try
            {
                LongCharacter = longChar;
            }
            catch (InvalidCharacterCodeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
