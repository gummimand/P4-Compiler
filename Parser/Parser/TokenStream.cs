using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class TokenStream
    {
        private List<Token> tokenStream;

        public TokenStream(List<Token> tokens)
        {
            tokenStream = tokens;
        }

        public Token next()
        {
            if (tokenStream.Count > 0)
            {
                var token = tokenStream[0];
                tokenStream.RemoveAt(0);
                return token;
            }
            else
                return new Token("END", "end");
        }

        public Token peek()
        {
            if (tokenStream.Count > 0)
                return tokenStream[0];
            else
                return new Token("END", "end");
        }


    }
}
