using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class SymbolTable
    {
        private int ScopeLevel = 0;

        Dictionary<int, SymbolTableLevel> Symboltable = new Dictionary<int, SymbolTableLevel>();

        public SymbolTable()
        {
            Symboltable.Add(ScopeLevel, new SymbolTableLevel());
        }

        public void EnterScope()
        {
            ScopeLevel++;
            if (!Symboltable.ContainsKey(ScopeLevel))
            {
                Symboltable.Add(ScopeLevel, new SymbolTableLevel());
            }
        }
        public void ExitScope()
        {
            if (ScopeLevel > 0)
            {
                ScopeLevel--;
            }
            else
                throw new Exception("Du går for langt ud!");
        }

        public void AddIdentifier(string identifier, string type, string value)
        {
            if (!Symboltable[ScopeLevel].contains(identifier))
            {
                Symboltable[ScopeLevel].add(identifier, type, value);
            }

            else
                throw new Exception("Den findes sgu allerede, brorlort"); //Maybe override? Vel ikke i det funktionelle paradigme?
        }

        public void PrintTables()
        {
            Console.WriteLine("Identifier(Type, Value)");
            for(int i = 0; i < Symboltable.Count; i++ )
            {
                Console.WriteLine("---Next level---");
                for (int k = 0; k < Symboltable[i].getLength(); k++)
                {
                    Console.Write(Symboltable[i].getKey(k));
                    Console.WriteLine(Symboltable[i].getValue(k));
                }
            }

            Console.ReadKey();

        }





    }
}
