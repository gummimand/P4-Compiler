using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    class Program
    {
        static void Main(string[] args)
        {

            var ts = new TokenStream(new List<Token>()
           {
               new Token("var", "x"),
               new Token("a", "identifier"),
               new Token("=", "x"),
               new Token("15", "Num"),
               new Token("hvis", "keyword"),
               new Token("(", "bracket"),
               new Token("True", "Bool"),
               new Token(")", "x"),
               new Token("så", "x"),
               new Token("a", "Var"),
               new Token("ellers", "x"),
               new Token("b", "Var")
           });

            var p = new Parser(ts);
            var ast = p.Parse();

            ast.print();
            Console.ReadKey();

        }
    }
}
