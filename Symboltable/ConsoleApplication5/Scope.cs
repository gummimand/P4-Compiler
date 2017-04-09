using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Scope : TableElement
    {
        private Dictionary<int, TableElement> _scope = new Dictionary<int, TableElement>();
        private int _internalScopelevel = 0;

        public Scope()
        { 
        }

        public void AddElement(TableElement element)
        {
            _scope.Add(_internalScopelevel, element);
            _internalScopelevel++;
        }
        
        public List<Variable> GetVariables()
        {
            List <Variable> output = new List<Variable>();

            foreach (Variable variable in _scope.Values)
            {
                output.Add(variable);
            }

            return output;
        }
    }
}
