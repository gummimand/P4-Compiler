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

            Scope scope1 = new Scope();
            Scope scope2 = new Scope();
            Scope scope3 = new Scope();
            Scope scope4 = new Scope();
            Scope scope5 = new Scope();
            
            SymbolTable.AddScope(scope1);
            SymbolTable.AddScope(scope2);
            scope1.AddScope(scope3);
            scope2.AddScope(scope4);
            scope4.AddScope(scope5);

            SymbolTable.AddVariable("Bla", "fucking", "bla");
            SymbolTable.AddVariable("123", "456", "789");
            scope1.AddVariable("zzz", "zzz", "zzz");
            scope2.AddVariable("SPO", "SPO", "SPO");
            scope3.AddVariable("HANS", "HANS", "HANS");
            scope4.AddVariable("nej", "nej", "nej");
            scope5.AddVariable("lol", "lol", "lol");

            SymbolTable.Print();
            Console.ReadKey();
        }
    }
}
