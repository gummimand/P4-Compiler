using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public enum TokenType
    {
        identifier,
        datatype,
        streng,
        heltal,
        tal,
        boolean,
        keyword,
        decl,
        op,
        parentes,
        seperator,
        EOF
    }

    public class Token
    {
        public string content;
        public TokenType Type;

        public Token(string content, TokenType type)
        {
            this.content = content;
            this.Type = type;
        }
    }
}
