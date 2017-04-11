using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SymbolTableV3
{
    class SymbolTable
    {
        List<Dictionary<string, Entry>> symboltable = new List<Dictionary<string, Entry>>();

        private int scopecounter = 0;

        public SymbolTable()
        {
            symboltable.Add(new Dictionary<string, Entry>());
        }


        public void AddScope()
        {
            symboltable.Add(new Dictionary<string, Entry>());
            scopecounter++;
        }

        public void AddEntry(string name, string type, string value)
        {
            for (int tempcount = scopecounter; tempcount >= 0; tempcount--)
            {
                if (symboltable[tempcount].ContainsKey(name))
                {
                    throw new ArgumentException("Entry already exists in scope " + tempcount.ToString()); //remember! Should it allow overwriting of variables in the same scope?
                }
            }
            
            Entry entry = new Entry{ Name = name, Type = type, Value = value};

            if (symboltable.ElementAt(scopecounter) == null) //Fail safe
                symboltable.Add(new Dictionary<string, Entry>());

            symboltable[scopecounter].Add(name, entry);
        }

        public Entry LookUp(string name)
        {
            for (int tempcount = scopecounter; tempcount >= 0; tempcount--)
            {
                if (symboltable[tempcount].ContainsKey(name))
                {
                    return symboltable[tempcount][name];
                }
            }
            return null;
        }

        public void ExitScope()
        {
            symboltable.RemoveAt(scopecounter);
            scopecounter--;
        }
    }
}
