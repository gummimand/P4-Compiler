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

    public class ProgramNode:Node
    {
        public ProgramNode() :base("Program"){ }
    }

    public class DeclarartionsNode :Node
    {
        public DeclarartionsNode() : base("Declarations") { }
    }

    public class VarDeclarartionNode : Node
    {
        public VarDeclarartionNode() : base("VarDeclarartion") { }
    }

    public class TypeDeclarartionNode : Node
    {
        public TypeDeclarartionNode() : base("TypeDeclarartion") { }
    }

    public class FunctionDeclarartionNode : Node
    {
        public FunctionDeclarartionNode() : base("FunctionDeclarartion") { }
    }
    public class ClauseNode : Node
    {
        public ClauseNode() : base("Clause") { }
    }
    public class DefaultClauseNode : Node
    {
        public DefaultClauseNode() : base("DefaultClause") { }
    }

    public class ExpressionNode : Node
    {
        public ExpressionNode() : base("Expression") { }
    }

    public class IfExpressionNode : Node
    {
        public IfExpressionNode() : base("IfExpression") { }
    }

    public class LetExpressionNode : Node
    {
        public LetExpressionNode() : base("LetExpression") { }
    }

    public class AnonFuncNode : Node
    {
        public AnonFuncNode() : base("AnonFunc") { }
    }

    public class StructureExpressionNode : Node
    {
        public StructureExpressionNode() : base("StructureExpression") { }
    }

    public class ListExpressionNode : Node
    {
        public ListExpressionNode() : base("ListExpression") { }
    }

    public class TupleExpressionNode : Node
    {
        public TupleExpressionNode() : base("TupleExpression") { }
    }

    public class ConstantExpressionNode : Node
    {
        public ConstantExpressionNode() : base("ConstantExpression") { }
    }

}
