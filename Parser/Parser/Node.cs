using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{

    public abstract class ASTNode
    {
        public Node Parent { get; set; }
        public abstract void PreOrderWalk();
        public abstract void PrintPretty(string indent, bool last);
    }

    public class Node: ASTNode
    {
        public string Type { get; set; }

        public List<ASTNode> Children { get; set; } = new List<ASTNode>();

        public Node(string type)
        {
            this.Type = type;
        }

        public override void PreOrderWalk()
        {
            Console.WriteLine(Type);
            foreach (ASTNode n in Children)
                n.PreOrderWalk();
        }

        public void AddChild(ASTNode n)
        {
            Children.Add(n);
            n.Parent = this;
        }

        public override void PrintPretty(string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }
            Console.WriteLine(Type);

            for (int i = 0; i < Children.Count; i++)
                Children[i].PrintPretty(indent, i == Children.Count - 1);
        }
    }
}
