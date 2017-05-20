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
            string path = @"C:\Users\TheChamp\Desktop\test3.txt";

            string sourcecode = System.IO.File.ReadAllText(path);

            Console.WriteLine(sourcecode);

            var scanner = new Scanner(sourcecode);
            var tokens = scanner.Scan();   

            var length = tokens.Count;
            for (int i = 0; i < length; i++)
            {
                Console.WriteLine(tokens[i].content + "  " + tokens[i].Type.ToString());
            }

            Console.ReadKey();
            var tokenStream = new TokenStream(tokens);
            var parser = new Parser(tokenStream);
            var ast = parser.Parse();

            ast.print();

            Console.ReadKey();

            var interpreter = new Interpreter();
            interpreter.Interpret(ast);

            Console.ReadKey();



            //Scanner scanner = new Scanner();
            //scanner.Scan1();

            //TokenStream tokenstream = new TokenStream(scanner.outputtokens);

            //var p = new Parser(tokenstream);
            //var ast = p.Parse();

            //ast.print();
            //Console.ReadKey();

        }
    }
}
