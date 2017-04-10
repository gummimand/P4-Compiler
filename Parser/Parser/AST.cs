using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class AST
    {
        public ASTNode Root { get; set; }

        public AST(ASTNode root)
        {
            Root = root;
        }

        public void print()
        {
            Root.PrintPretty("",true);
        }


    }
}
