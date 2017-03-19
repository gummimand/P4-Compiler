using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    class TokenStream
    {
        private List<Token> tokenStream;
        private int currentIndex = 0;

        public Token next()
        {
            currentIndex++;
            if (tokenStream.Count > currentIndex)
                return tokenStream[currentIndex];
            else
                return new Token("", "end");
        }

        public Token peek()
        {
            if (tokenStream.Count > currentIndex + 1)
                return tokenStream[currentIndex + 1];
            else
                return new Token("", "end");
        }


    }
}
