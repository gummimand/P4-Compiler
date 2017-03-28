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
            Scanner scanner = new Scanner();
            scanner.Scan();

            TokenStream tokenstream = new TokenStream(scanner.outputtokens);

            var p = new Parser(tokenstream);
            var ast = p.Parse();

            ast.print();
            Console.ReadKey();

        }
    }
}
