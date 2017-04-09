using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            Scope SymbolTable = new Scope();

            Variable counter = new Variable("counter","int", "7");

            SymbolTable.AddElement(counter);

            counter.PrintVariable();

            List<Variable> Liste = new List<Variable>();

            foreach (var item in Liste)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
