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

    public class Node : ASTNode
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

    public class Identifier : Leaf
    {
        public Identifier(Token t) : base(t) { }
    }

    public class Operator : Leaf
    {
        public Operator(Token t) : base(t) { }
    }

    public class Constructor : Leaf
    {
        public Constructor(Token t) : base(t) { }
    }

    public class Value : Leaf
    {
        public Value(Token t) : base(t) { }
    }

    public class ProgramNode : Node
    {
        public ProgramNode() : base("Program") { }
    }

    public class DeclarartionsNode : Node
    {
        public DeclarartionsNode() : base("Declarations") { }
    }



    public abstract class Decl : Node
    {
        public Decl(string type) : base(type) { }
    }

    public class VarDecl : Decl
    {
        private Identifier id;
        private Expression exp;

        public VarDecl(Identifier id, Expression exp) : base("VAR_DECL")
        {
            this.id = id;
            this.exp = exp;
        }
    }

    public class SeqDecl : Decl
    {
        private Decl decl1;
        private Decl decl2;

        public SeqDecl(Decl decl1, Decl decl2) : base("SEQ_DECL")
        {
            this.decl1 = decl1;
            this.decl2 = decl2;
        }
    }

    public class FuncDecl : Decl
    {
        private Identifier id;
        private Clause cl;
        private Identifier[] args;

        public FuncDecl(Identifier id, Clause cl, params Identifier[] args) : base("FUNC_DECL")
        {
            this.id = id;
            this.cl = cl;
            this.args = args;
        }
    }

    public class TypeDecl : Decl
    {
        private Identifier id;
        private Identifier[] labels;

        public TypeDecl(Identifier id, params Identifier[] labels) : base("TYPE_DECL")
        {
            this.id = id;
            this.labels = labels;
        }
    }

    public abstract class Clause : Node
    {
        public Clause(string type) : base(type)
        {
        }
    }

    public class DefaultClause : Clause
    {
        private Expression exp;

        public DefaultClause(Expression exp) : base("DEFAULT_CLAUSE")
        {
            this.exp = exp;
        }
    }

    public class ConditionalClause : Clause
    {
        private Expression condition;
        private Expression exp;
        private Clause altClause;

        public ConditionalClause(Expression condition, Expression exp, Clause altClause) : base("CONDITIONAL_CLAUSE")
        {
            this.condition = condition;
            this.exp = exp;
            this.altClause = altClause;
        }
    }

    public abstract class Expression : Node
    {
        public Expression(string type) : base(type) { }
    }

    public class ConstrExpression : Expression
    {
        private Constructor constructor;
        private Expression[] exps;

        public ConstrExpression(Constructor constructor, params Expression[] exps) : base("CONSTR_EXPRESSION")
        {
            this.constructor = constructor;
            this.exps = exps;
        }
    }

    public class OperatorExpression : Expression
    {
        private Operator op;
        private Expression[] exps;

        public OperatorExpression(Operator op, params Expression[] exps) : base("OPERATOR_EXPRESSION")
        {
            this.op = op;
            this.exps = exps;
        }
    }

    public class IfExpression : Expression
    {

        private Expression condition;
        private Expression alt1;
        private Expression alt2;

        public IfExpression(Expression condition, Expression alt1, Expression alt2) : base("IF_EXPRESSION")
        {
            this.condition = condition;
            this.alt1 = alt1;
            this.alt2 = alt2;
        }
    }

    public class LetExpression : Expression
    {
        private Decl decl;
        private Expression exp;

        public LetExpression(Decl decl, Expression exp) : base("LET_EXPRESSION")
        {
            this.decl = decl;
            this.exp = exp;
        }
    }

    public class AnonFuncExpression : Expression
    {
        private Identifier[] args;
        private Expression exp;

        public AnonFuncExpression(Expression exp, params Identifier[] args) : base("ANONFUNC_EXPRESSION")
        {
            this.exp = exp;
            this.args = args;
        }
    }

    public class ValueExpression : Expression
    {
        private Value value;

        public ValueExpression(Value value) : base("VALUE_EXPRESSION")
        {
            this.value = value;
        }
    }

    public class IdentifierExpression : Expression
    {
        private Identifier id;

        public IdentifierExpression(Identifier id) : base("IDENTIFIER_EXPRESSION")
        {
            this.id = id;
        }
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

   

    public class IfExpressionNode : Expression
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
