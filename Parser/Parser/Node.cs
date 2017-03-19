using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class Node
    {
        public Node Parent { get; set; }
        public Token token { get; set; }

        public List<Node> Children { get; set; }

        public void PreOrderWalk()
        {
            Console.WriteLine(token.Type + " - " + token.Value);
            foreach (Node n in Children)
                n.PreOrderWalk();
        }

        public void AddChild(Node n)
        {
            Children.Add(n);
            n.Parent = this;
        }
    }
}
