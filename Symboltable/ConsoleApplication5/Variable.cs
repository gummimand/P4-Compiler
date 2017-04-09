using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Variable : TableElement
    {
        private string _type;
        private string _value;
        private string _name;

        public Variable(string type, string value, string name)
        {
            _type = type;
            _value = value;
            _name = nameof();
        }

        public string GetName()
        {
            return _name;
        }

        public string GetType() 
        {
            return _type;
        }

        public string GetValue()
        {
            return _value;
        }

        public void PrintVariable()
        {
            Console.WriteLine(_name);
            Console.WriteLine(_type);
            Console.WriteLine(_value);
        }
    }
}
