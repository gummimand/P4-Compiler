using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    class Scanner
    {
        InputStream CS = new InputStream(@"MyTest.txt");

        public List<Token> outputtokens = new List<Token>();

        string TokenContent;

        enum Typer { Tal, Heltal, Streng, Identifier, Operator, Bracket, Keyword }

        public void Scan()
        {
            while (isWhiteSpace(CS.peek()))
            {
                CS.advance();
            }

            char peeked = CS.peek();

            if (CS.EOF())
            {
                outputtokens.Add(new Token("EOF")); //todo, End of file char?
            }
            else if (isDigit(peeked))
            {
                outputtokens.Add(ScanDigit());
                Scan();
            }
            else if (isLetter(peeked))
            {
                outputtokens.Add(ScanLetter());
                Scan();
            }
            else if (isBracket(peeked))
            {
                outputtokens.Add(ScanBracket());
                Scan();
            }

            else if (peeked == '"')
            {
                TokenContent += CS.add();

                {
                    while (CS.peek() != '"')
                        TokenContent += CS.add();
                }

                outputtokens.Add(new Token(TokenContent, Typer.Streng.ToString()));
                Scan();
            }
            else if (isOperator(peeked))
            {
                outputtokens.Add(ScanOperator());
                Scan();
            }
            else
            {
              outputtokens.Add(new Token(CS.add().ToString(), "error"));

              Scan();
            }

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
            Regex r = new Regex("[ÆØÅæøåA-Za-z0-9]");

            return r.IsMatch(input.ToString());
        }

        private bool isOperator(char input)
        {
            Regex r_operators = new Regex("[-+*/^%<>=|&.:]"); //@"\-\+\*\^.|&%/\:"

            return r_operators.IsMatch(input.ToString());
        }

        private bool isBracket(char input)
        {
            Regex r = new Regex(@"\(\)\[\]{}");

            return r.IsMatch(input.ToString());

        }

        private Token ScanOperator()
        {
            Regex r = new Regex("[<>=!]");

            string output = "";

            if (r.IsMatch(CS.peek().ToString()))
            {
                output += CS.add();
                if (CS.peek() == '=')
                {
                    output += CS.add();
                    return new Token(output, Typer.Operator.ToString());

                }
                else
                    return new Token(output, Typer.Operator.ToString());
            }
            else
            {
                output += CS.add();
                return new Token(output, Typer.Operator.ToString());
            }
        }

        private Token ScanDigit()
        {
            string output = "";
            string type = Typer.Heltal.ToString();

            while (isDigit(CS.peek()))
            {
                output += CS.add();
            }
            if (CS.peek() == '.')
            {
                type = Typer.Tal.ToString();
                output += CS.add();

                if (!isDigit(CS.peek()))
                {
                    output += 0;
                    return new Token(output, type);
                }
                else
                {
                    while (isDigit(CS.peek()))
                    {
                        output += CS.add();
                    }
                    return new Token(output, type);
                }
            }
            else
                return new Token(output, type);
        }

        private Token ScanLetter()
        {
            Regex r_letterAndDigit = new Regex("[A-ZÆØÅa-zæøå0-9]");

            string output = "";

            while (r_letterAndDigit.IsMatch(CS.peek().ToString()))
            {
                output += CS.add();
            }
            switch(output)
            {
                case "hvis":
                case "så":
                case "ellers":
                case "lad":
                case "i":
                case "slut":
                case "var":
                case "funktion":
                case "FALSK":
                case "SAND":
                case "hoved":
                case "hale":
                case "tag":
                case "smid":
                case "fejl":
                    return new Token(output, Typer.Keyword.ToString());
                default:
                    return new Token(output, Typer.Identifier.ToString());
            }
        }

        private Token ScanBracket()
        {
            return new Token(CS.add().ToString(), Typer.Bracket.ToString());
        }

        public void printTokens()
        {
            foreach (Token token in outputtokens)
            {
                Console.WriteLine(token.type);
                Console.WriteLine(token.content);
                Console.WriteLine("---------------------");
            }
            Console.ReadKey();
        }

        public void test()
        {
            while (!CS.EOF())
            {
                Console.WriteLine(CS.AddNumber());
            }
            Console.Read();
        }
    }


}