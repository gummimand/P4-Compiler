using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class Symboltable<T>
    {
        public List<Tuple<string, T>> table = new List<Tuple<string, T>>();

        public Symboltable()
        {

        }

        public void Add(string varName, T input)
        {
            table.Insert(0, Tuple.Create(varName, input));
        }

        protected void AddTuple(Tuple<string, T> tuple)
        {
            table.Add(tuple);
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

            for (int i = table.Count - 1; i >= 0; i--)
            {
                Console.WriteLine(table[i].Item1 + " = " + table[i].Item2.ToString());
            }
        }

    }


    

}
