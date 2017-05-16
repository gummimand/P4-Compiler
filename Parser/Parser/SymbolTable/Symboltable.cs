using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class Symboltable<T>
    {
        private List<Tuple<string, T>> table = new List<Tuple<string, T>>();

        public Symboltable()
        {

        }

        public void Add(string varName, T input)
        {
            table.Insert(0, Tuple.Create(varName, input));
        }

        public void Remove()
        {
            if (table.Count > 0)
            {
                table.RemoveAt(0);
            }
        }

        public T LookUp(string varName) // black magic
        {
            return table.Find(t => t.Item1 == varName).Item2;           
        }

        public void Print()
        {
            foreach (var entry in table)
            {
                Console.WriteLine(entry.Item1 + " = " + entry.Item2.ToString());
            }
        }

    }
}
