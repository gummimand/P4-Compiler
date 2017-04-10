using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class CharacterStream
    {
        public List<char> Chars;

        public CharacterStream(string characterString)
        {
            Chars = characterString.ToList();
        }

        public char Peek()
        {
            if (EOF())
                return '\0';
            else
                return Chars[0];
        }

        public char GetNextChar()
        {
            char ch;

            if (EOF())
                return '\0';
            else
            {
                ch = Chars[0];
                Chars.RemoveAt(0);
                return ch;
            }   
        }

        public void Advance()
        {
            if (!EOF())
                Chars.RemoveAt(0);
        }

        public bool EOF()
        {
            return Chars.Count == 0;
        }

    }
}
