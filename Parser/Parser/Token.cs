using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class Token
    {

        public string Value { get; set; }
        public string Type { get; set; }

        public Token(string value, string type)
        {
            Value = value;
            Type = type;
        }
        
    }
}
