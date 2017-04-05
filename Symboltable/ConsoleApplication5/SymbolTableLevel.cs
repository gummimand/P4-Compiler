using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class SymbolTableLevel
    {
        private Dictionary<string, Tuple<string, string>> table = new Dictionary<string, Tuple<string, string>>();

        public SymbolTableLevel()
        {

        }

        public void add(string identifier, string type, string value)
        {
            table.Add(identifier, Tuple.Create(type, value));
        }

        public bool contains(string input)
        {
            return table.ContainsKey(input);
        }

        public string getKey(int index)
        {
            return table.ElementAt(index).Key;
        }

        public Tuple<string,string> getValue(int index)
        {
            return table.ElementAt(index).Value;
        }

        public int getLength()
        {
            return table.Count;
        }
    }
}
