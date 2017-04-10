using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class Leaf:ASTNode
    {
        public Token token { get; set; }

        public Leaf(Token tkn)
        {
            token = tkn;
        }

        public override void PreOrderWalk()
        {
            Console.WriteLine(token.content + " : " + token.Type);
        }
    }
}
