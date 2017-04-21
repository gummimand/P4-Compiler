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

        public override bool Equals(object obj)
        {
            Token other = obj as Token;
            if (other != null)
                return this.content == other.content && this.Type == other.Type;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    
}
