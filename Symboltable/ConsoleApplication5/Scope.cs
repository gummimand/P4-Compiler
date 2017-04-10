using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Scope : TableElement
    {
        Dictionary<int, TableElement> level = new Dictionary<int, TableElement>();

        private int _internalScopelevel = 0;

        public Scope()
        { 
        }

        public void AddScope(Scope scope)
        {
            level.Add(_internalScopelevel, scope);
            _internalScopelevel++;
        }

        public void AddVariable(string name, string type, string value)
        {
            level.Add(_internalScopelevel, new Variable(name, type, value));
            _internalScopelevel++;
        }

        public List<Variable> GetVariables()
        {
            List <Variable> output = new List<Variable>();

            foreach (Variable variable in level.Values)
            {
                output.Add(variable);
            }

            return output;
        }

        public TableElement GetElement(int index)
        {
            return level.ElementAt(index).Value;
        }

        public override void Print()
        {
            Console.WriteLine("----Scope start----");
            foreach(TableElement item in level.Values)
            {
                item.Print();
            }
            Console.WriteLine("----Scope end----");
        }

    }
}
