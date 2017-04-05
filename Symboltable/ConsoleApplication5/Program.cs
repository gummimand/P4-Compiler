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
            SymbolTable table = new SymbolTable();
            //table.EnterScope();
            table.AddIdentifier("Hej", "streng", "dette er en hilsen");
            table.EnterScope();
            table.AddIdentifier("Kommentar", "HansKommentar", "dette er noget sovs");
            table.EnterScope();
            table.AddIdentifier("Meme", "Fugtig", "Noget med Bubber");
            table.AddIdentifier("Joke", "dårlig", "mit liv");
            table.EnterScope();
            table.AddIdentifier("Holdning", "retarderet", "Der er mere end to køn!");
            table.PrintTables();
        }
    }
}
