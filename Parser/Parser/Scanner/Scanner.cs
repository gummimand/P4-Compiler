using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class Scanner
    {

        private CharacterStream cs;

        public Scanner(string characterString)
        {
            cs = new CharacterStream(characterString);
        }

        //public enum Typer { Tal, Heltal, Streng, Identifier, Operator, Bracket, Keyword } 


        public List<Token> Scan()
        {
            var tokens = new List<Token>();

            if (cs.EOF())
                tokens.Add(new Token("EOF", TokenType.EOF));

            while (!cs.EOF())
            {
                RemoveWhiteSpace();
                ScanComment();
                if (cs.EOF())
                    tokens.Add(new Token("EOF", TokenType.EOF)); //todo, End of file char?
                else if (cs.Peek() == '#')
                {
                    while (cs.Peek() != '\n' && !cs.EOF())
                        cs.Advance();
                }
                else
                    tokens.Add(ScanToken());
            }

            return tokens;
        }

        private void RemoveWhiteSpace()
        {
            while (isWhiteSpace(cs.Peek()))
            {
                cs.Advance();
            }
        }

        private void ScanComment()
        {
            while (cs.Peek() == '#')
            {
                cs.Advance();
                while (cs.Peek() != '\n')
                {
                    cs.Advance();
                }
                RemoveWhiteSpace();
            }
        }

        private Token ScanToken()
        {
            char peeked = cs.Peek();

            if (isDigit(peeked))
            {
                return ScanDigit();
            }
            else if (isLetter(peeked))
            {
                return ScanLetter();
            }
            else if (isBracket(peeked))
            {
                return ScanBracket();
            }
            else if (peeked == '"')
            {
                return ScanString();
            }
            else if (isOperator(peeked))
            {
                return ScanOperator();
            }
            else if (isSeperator(peeked))
            {
                return ScanSeperator();
            }
            else
            {
                throw new ArgumentException($"Syntax error: Could not read {peeked}");
            }
        }

        private Token ScanSeperator()
        {
            string lexeme = cs.GetNextChar().ToString();
            return new Token(lexeme, TokenType.seperator);
        }

        private Token ScanString()
        {
            string lexeme = cs.GetNextChar().ToString();
          
            while (!cs.EOF() && cs.Peek() != '"')
            {
                lexeme += cs.GetNextChar();
            }

            if (cs.EOF())
                throw new ArgumentException("File ended while trying to scan a string");
            else if (cs.Peek() == '"') 
                return new Token(lexeme + cs.GetNextChar(), TokenType.streng);
            else
                throw new ArgumentException($"Something went wrong while scanning a string. next char was {cs.Peek()}");
        }

        private bool isDigit(char input)
        {
            Regex r = new Regex("[0-9]");

            return r.IsMatch(input.ToString());
        }

        private bool isWhiteSpace(char input)
        {
            Regex r = new Regex(@"\s");

            return r.IsMatch(input.ToString());
        }

        private bool isLetter(char input)
        {
            Regex r = new Regex("[ÆØÅæøåA-Za-z]");

            return r.IsMatch(input.ToString());
        }

        private bool isOperator(char input)
        {
            Regex r_operators = new Regex("[-+*/^%<>=|&!:]"); //@"\-\+\*\^.|&%/\:"TODO contain "!"?

            return r_operators.IsMatch(input.ToString());
        }

        private bool isBracket(char input)//todo
        {
            Regex r = new Regex(@"[\(\)\[\]{}]");

            return r.IsMatch(input.ToString());

        }

        private bool isSeperator(char input)
        {
            Regex r = new Regex(@"[,;]");

            return r.IsMatch(input.ToString());
        }

        private Token ScanOperator()
        {
            Regex r = new Regex("[<>=!]");

            char first = cs.Peek();
            string output = "";
            TokenType type = TokenType.op;

            if (r.IsMatch(first.ToString()))
            {
                output += cs.GetNextChar();
                if (cs.Peek() == '=')
                {
                    output += cs.GetNextChar();
                    return new Token(output, type);
                }
                else if (cs.Peek() == '>')
                {
                    output += cs.GetNextChar();
                    return new Token(output, type);
                }
                else
                    return new Token(output, type);
            }
            else if(first == '&')
            {
                cs.GetNextChar();
                if (cs.Peek() == '&') {
                    cs.GetNextChar();
                    return new Token("&&", type);
                }
                else
                    throw new ArgumentException("& is not a valid operator. Use &&.");
            }
            else if (first == '|')
            {
                cs.GetNextChar();
                if (cs.Peek() == '|')
                {
                    cs.GetNextChar();
                    return new Token("||", type);
                }
                else
                {
                    return new Token("|", TokenType.seperator);
                }
            }
            else
            {
                output += cs.GetNextChar();
                return new Token(output, TokenType.op);
            }
        }

        private Token ScanDigit()
        {
            string lexeme = "";
            TokenType type = TokenType.heltal;

            while (isDigit(cs.Peek()))
                lexeme += cs.GetNextChar();

            if (cs.Peek() == '.')
            {
                type = TokenType.tal;
                lexeme += cs.GetNextChar();

                if (!isDigit(cs.Peek()))
                {
                    lexeme += 0;
                    return new Token(lexeme, type);
                }
                else
                {
                    while (isDigit(cs.Peek()))
                        lexeme += cs.GetNextChar();

                    return new Token(lexeme, type);
                }
            }
            else
            {
                return new Token(lexeme, type);
            }
        }

        private Token ScanLetter()
        {
            string lexeme = "";

            while(isLetter(cs.Peek()) || isDigit(cs.Peek()))
                lexeme += cs.GetNextChar();

            switch(lexeme)
            {
                case "hvis":
                case "så":
                case "ellers":
                case "lad":
                case "i":
                case "slut":
                case "hoved":
                case "hale":
                case "tag":
                case "smid":
                case "fejl":
                case "fn":
                    return new Token(lexeme, TokenType.keyword);
                case "var":
                case "funktion":
                    return new Token(lexeme, TokenType.decl);
                case "type":
                    return new Token(lexeme, TokenType.typeDecl);
                case "falsk":
                case "sand":
                    return new Token(lexeme, TokenType.boolean);
                default:
                    return new Token(lexeme, TokenType.identifier);
            }
        }

        private Token ScanBracket()
        {
            return new Token(cs.GetNextChar().ToString(), TokenType.parentes);
        }
    }


}