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

        public Variable(string name, string type, string value)
        {
            _name = name;
            _type = type;
            _value = value;
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

        public override void Print()
        {
            Console.Write(_name);
            Console.Write(_type);
            Console.WriteLine(_value);
        }
    }
}
