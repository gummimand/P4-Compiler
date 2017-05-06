using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class Symboltable
    {
        List<SymbolTableLevel> SymbolTable = new List<SymbolTableLevel>();
        
        public Symboltable()
        {
            SymbolTable.Add(new SymbolTableLevel());
        }

        public void AddSymboltableLevel(SymbolTableLevel Level)
        {
            SymbolTable.Add(Level);
        }

        public void PopSymboltableLevel()
        {
            SymbolTable.RemoveAt(SymbolTable.Count);
        }

        public bool Lookup(string identifier) //used to check if a variable already exists on a higher level
        {
            foreach (SymbolTableLevel Level in SymbolTable)
            {
                if (Level.contains(identifier))
                    return false;
            }
            return true;  
        }

    }
}
