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
        public TokenType type;
        public abstract void PreOrderWalk();
        public abstract void PrintPretty(string indent, bool last);
        public abstract void accept(IVisitor v);
    }

    public class Node : ASTNode
    {
        //To label node in the AST structure -> for printing.
        public string NodeLabel { get; set; }

        public List<ASTNode> Children { get; set; } = new List<ASTNode>();

        public override void accept(IVisitor v) { v.visit(this); }

        public Node(string nodelabel)
        {
            this.NodeLabel = nodelabel;
        }

        public override void PreOrderWalk()
        {
            Console.WriteLine(NodeLabel);
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
            Console.WriteLine(NodeLabel);

            for (int i = 0; i < Children.Count; i++)
                Children[i].PrintPretty(indent, i == Children.Count - 1);
        }

        public override bool Equals(object obj)
        {
            Node other = obj as Node;
            if (other != null)
            {
                if (this.NodeLabel == other.NodeLabel)
                {
                    int thisChildren = this.Children.Count;
                    int otherChildren = other.Children.Count;

                    if (thisChildren == otherChildren)
                    {
                        if (thisChildren == 0)
                        {
                            return true;
                        }
                        else
                        {
                            bool isEqual = true;
                            int i = 0;

                            while (isEqual && i < thisChildren)
                            {
                                isEqual = isEqual && this.Children[i].Equals(other.Children[i]);
                                i++;
                            }

                            return isEqual;
                        }
                    }
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ProgramAST : Node
    {
        public Decl varDecl;
        public Expression exp;

        public override void accept(IVisitor v) { v.visit(this); }

        public ProgramAST(Decl varDecl, Expression exp) : base("Program")
        {
            this.varDecl = varDecl;
            this.exp = exp;

            AddChild(varDecl);
            AddChild(exp);
        }
    }

    public abstract class Decl : Node
    {
        public Decl(string type) : base(type) { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class VarDecl : Decl
    {
        public Identifier id;
        public Expression exp;
        public Decl nextDecl;

        public override void accept(IVisitor v) { v.visit(this); }

        public VarDecl(Identifier id, Expression exp, Decl nextDecl) : base("VAR_DECL")
        {
            this.id = id;
            this.exp = exp;
            this.nextDecl = nextDecl;

            AddChild(id);
            AddChild(exp);
            AddChild(nextDecl);
        }
    }

    public class EmptyDecl : Decl
    {
        public EmptyDecl() : base("EMPTY_DECL") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public abstract class Expression : Node
    {
        public Expression(string type) : base(type) { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class IfExpression : Expression
    {

        public Expression condition;
        public Expression alt1;
        public Expression alt2;

        public override void accept(IVisitor v) { v.visit(this); }

        public IfExpression(Expression condition, Expression alt1, Expression alt2) : base("IF_EXPRESSION")
        {
            this.condition = condition;
            this.alt1 = alt1;
            this.alt2 = alt2;

            AddChild(condition);
            AddChild(alt1);
            AddChild(alt2);
        }
    }

    public class LetExpression : Expression
    {
        public Identifier id;
        public Expression exp1;
        public Expression exp2;

        public override void accept(IVisitor v) { v.visit(this); }

        public LetExpression(Identifier id, Expression exp1, Expression exp2) : base("LET_EXPRESSION")
        {
            this.id = id;
            this.exp1 = exp1;
            this.exp2 = exp2;

            AddChild(id);
            AddChild(exp1);
            AddChild(exp2);
        }
    }


    public class ValueExpression : Expression
    {
        public Value value;

        public override void accept(IVisitor v) { v.visit(this); }

        public ValueExpression(Value value) : base("VALUE_EXPRESSION")
        {
            this.value = value;

            AddChild(value);
        }
    }

    public class IdentifierExpression : Expression
    {
        public Identifier id;

        public override void accept(IVisitor v) { v.visit(this); }

        public IdentifierExpression(Identifier id) : base("IDENTIFIER_EXPRESSION")
        {
            this.id = id;

            AddChild(id);
        }
    }

    public class EmptyListExpression : Expression
    {
        public override void accept(IVisitor v) { v.visit(this); }

        public EmptyListExpression() : base("EMPTY_LIST") { }

    }

    public class ApplicationExpression : Expression
    {
        public Expression rator;
        public Expression rand;

        public override void accept(IVisitor v) { v.visit(this); }

        public ApplicationExpression(Expression rator, Expression rand) : base("APPLICATION_EXPRESSION")
        {
            this.rator = rator;
            this.rand = rand;

            AddChild(rator);
            AddChild(rand);
        }
    }

    public class EmptyExpression : Expression
    {
        public EmptyExpression() : base("EMPTY_EXPRESSION") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public abstract class ConstantExpression : Expression
    {
        public ConstantExpression(string type) : base(type)
        {
        }
    }

    public class AnonFuncExpression : ConstantExpression
    {
        public Identifier arg;
        public Expression exp;

        public override void accept(IVisitor v) { v.visit(this); }

        public AnonFuncExpression(Identifier arg, Expression exp) : base("ANONFUNC_EXPRESSION")
        {
            this.exp = exp;
            this.arg = arg;

            AddChild(arg);
            AddChild(exp);
        }
    }

    public class ListConst : ConstantExpression
    {
        public ListConst() : base("LIST")
        {
        }
    }

    public class PairConst : ConstantExpression
    {
        public PairConst() : base("PAR")
        {
        }
    }

    public class PlusConst : ConstantExpression
    {
        public PlusConst() : base("PLUS")
        {
        }
    }

    public class MinusConst : ConstantExpression
    {
        public MinusConst() : base("MINUS")
        {
        }
    }

    //public class Id : ConstantExpression
    //{
    //    public string Content;
    //    public Id(string content) : base("Identifier")
    //    {
    //        Content = content;
    //    }
    //}

    public class Identifier : Leaf
    {
        public Identifier(Token t) : base(t) { }
        public override void accept(IVisitor v) { v.visit(this); }
        //Static polymorphism
    }

    public class Value : Leaf
    {
        public Value(Token t) : base(t) { }
        public override void accept(IVisitor v) { v.visit(this); }
    }
}

