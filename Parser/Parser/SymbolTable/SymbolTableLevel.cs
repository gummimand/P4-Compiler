using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class SymbolTableLevel
    {
        private Dictionary<string, Tuple<ConstructedType, string>> table = new Dictionary<string, Tuple<ConstructedType, string>>(); 

        public SymbolTableLevel()
        {

        }

        public void add(string identifier, ConstructedType type, string value) 
        {
            table.Add(identifier, Tuple.Create(type, value));
        }

        public bool contains(string input) //used in SymbolTable.Lookup()
        {
            return table.ContainsKey(input);
        }
    }
}
