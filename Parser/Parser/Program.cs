using System;
using System.Collections.Generic;
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
               new Token("hvis"),
               new Token("("),
               new Token("True", "Bool"),
               new Token(")"),
               new Token("så"),
               new Token("a", "Var"),
               new Token("ellers"),
               new Token("b", "Var")
           });

            var p = new Parser(ts);
            var ast = p.Parse();

            ast.print();

            Console.ReadKey();

        }
    }
}
