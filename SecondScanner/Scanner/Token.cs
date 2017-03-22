using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstScanner
{
    class Token
    {
        public string content;
        public string type;

        public Token(string _content)
        {
            content = _content;
        }

        public Token(string _content, string _type)
        {
            content = _content;
            type = _type;
        }
    }
}