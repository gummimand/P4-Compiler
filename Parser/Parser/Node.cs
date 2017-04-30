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

    public class Identifier : Leaf
    {
        public Identifier(Token t) : base(t) { }
        public override void accept(IVisitor v) { v.visit(this); }
        //Static polymorphism

    }

    public class Operator : Leaf
    {
        public Operator(Token t) : base(t) { }
        public override void accept(IVisitor v) { v.visit(this); }

    }

    public class Constructor : Leaf
    {
        public Constructor(Token t) : base(t) { }
        public override void accept(IVisitor v) { v.visit(this); }

    }

    public class Value : Leaf
    {
        public Value(Token t) : base(t) { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class ProgramAST : Node
    {
        private Decl decl;
        private Expression exp;

        public override void accept(IVisitor v) { v.visit(this); }

        public ProgramAST(Decl decl, Expression exp) : base("Program") {
            this.decl = decl;
            this.exp = exp;

            AddChild(decl);
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
        private Identifier id;
        private Expression exp;

        public override void accept(IVisitor v) { v.visit(this); }

        public VarDecl(Identifier id, Expression exp) : base("VAR_DECL")
        {
            this.id = id;
            this.exp = exp;

            AddChild(id);
            AddChild(exp);
        }
    }

    public class SeqDecl : Decl
    {
        private Decl decl1;
        private Decl decl2;

        public override void accept(IVisitor v) { v.visit(this); }

        public SeqDecl(Decl decl1, Decl decl2) : base("SEQ_DECL")
        {
            this.decl1 = decl1;
            this.decl2 = decl2;

            AddChild(decl1);
            AddChild(decl2);
        }
    }

    public class FuncDecl : Decl
    {
        private Identifier id;
        private Clause cl;
        private Identifier[] args;

        public override void accept(IVisitor v) { v.visit(this); }

        public FuncDecl(Identifier id, Clause cl, params Identifier[] args) : base("FUNC_DECL")
        {
            this.id = id;
            this.cl = cl;
            this.args = args;

            AddChild(id);
            foreach (var arg in args)
                AddChild(arg);
            AddChild(cl);
        }
    }

    public class TypeDecl : Decl
    {
        private Identifier id;
        private DatatypeLabelPair[] labels;

        public override void accept(IVisitor v) { v.visit(this); }

        public TypeDecl(Identifier id, params DatatypeLabelPair[] labels) : base("TYPE_DECL")
        {
            this.id = id;
            this.labels = labels;

            AddChild(id);
            foreach (var label in labels)
                AddChild(label);
        }
    }

    public class EmptyDecl : Decl
    {
        public EmptyDecl() : base("EMPTY_DECL") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class DatatypeLabelPair : Node
    {
        Identifier label;
        public Identifier elementType;

        public override void accept(IVisitor v) { v.visit(this); }

        public DatatypeLabelPair(Identifier label, Identifier type) : base("DATATYPELABEL_PAIR")
        {
            this.label = label;
            this.elementType = type;

            AddChild(label);
            AddChild(type);
        }
    }

    public abstract class Clause : Node
    {
        public Clause(string type) : base(type) { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class DefaultClause : Clause
    {
        private Expression exp;

        public override void accept(IVisitor v) { v.visit(this); }

        public DefaultClause(Expression exp) : base("DEFAULT_CLAUSE")
        {
            this.exp = exp;
            AddChild(exp);
        }
    }

    public class ConditionalClause : Clause
    {
        private Expression condition;
        private Expression exp;
        private Clause altClause;

        public override void accept(IVisitor v) { v.visit(this); }

        public ConditionalClause(Expression condition, Expression exp, Clause altClause) : base("CONDITIONAL_CLAUSE")
        {
            this.condition = condition;
            this.exp = exp;
            this.altClause = altClause;

            AddChild(condition);
            AddChild(exp);
            AddChild(altClause);
        }
    }

    public abstract class Expression : Node
    {
        public Expression(string type) : base(type) { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class ConstrExpression : Expression
    {
        private Constructor constructor;
        private Expression[] exps;

        public override void accept(IVisitor v) { v.visit(this); }

        public ConstrExpression(Constructor constructor, params Expression[] exps) : base("CONSTR_EXPRESSION")
        {
            this.constructor = constructor;
            this.exps = exps;

            AddChild(constructor);
            foreach (var exp in exps)
                AddChild(exp);
        }
    }

    public class OperatorExpression : Expression
    {
        private Operator op;
        private Expression[] exps;

        public override void accept(IVisitor v) { v.visit(this); }

        public OperatorExpression(Operator op, params Expression[] exps) : base("OPERATOR_EXPRESSION")
        {
            this.op = op;
            this.exps = exps;

            AddChild(op);
            foreach (var exp in exps)
                AddChild(exp);
        }
    }

    public class IfExpression : Expression
    {

        private Expression condition;
        private Expression alt1;
        private Expression alt2;

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
        private Decl decl;
        private Expression exp;

        public override void accept(IVisitor v) { v.visit(this); }

        public LetExpression(Decl decl, Expression exp) : base("LET_EXPRESSION")
        {
            this.decl = decl;
            this.exp = exp;

            AddChild(decl);
            AddChild(exp);
        }
    }

    public class AnonFuncExpression : Expression
    {
        private Identifier[] args;
        private Expression exp;

        public override void accept(IVisitor v) { v.visit(this); }

        public AnonFuncExpression(Expression exp, params Identifier[] args) : base("ANONFUNC_EXPRESSION")
        {
            this.exp = exp;
            this.args = args;

            foreach (var arg in args)
                AddChild(arg);
            AddChild(exp);
        }
    }

    public class ValueExpression : Expression
    {
        private Value value;

        public override void accept(IVisitor v) { v.visit(this); }

        public ValueExpression(Value value) : base("VALUE_EXPRESSION")
        {
            this.value = value;

            AddChild(value);
        }
    }

    public class IdentifierExpression : Expression
    {
        private Identifier id;

        public override void accept(IVisitor v) { v.visit(this); }

        public IdentifierExpression(Identifier id) : base("IDENTIFIER_EXPRESSION")
        {
            this.id = id;

            AddChild(id);
        }
    }

    public class StructureExpression : Expression
    {
        Expression[] exps;

        public override void accept(IVisitor v) { v.visit(this); }

        public StructureExpression(params Expression[] exps) : base("STRUCTURE_EXPRESSION")
        {
            this.exps = exps;

            foreach (var exp in exps)
                AddChild(exp);
        }
    }

    public class ListExpression : Expression
    {
        Expression[] exps;

        public override void accept(IVisitor v) { v.visit(this); }

        public ListExpression(params Expression[] exps) : base("LIST_EXPRESSION")
        {
            this.exps = exps;

            foreach (var exp in exps)
                AddChild(exp);
        }
    }

    public class TupleExpression : Expression
    {
        Expression[] exps;

        public override void accept(IVisitor v) { v.visit(this); }

        public TupleExpression(params Expression[] exps) : base("TUPLE_EXPRESSION")
        {
            this.exps = exps;

            foreach (var exp in exps)
                AddChild(exp);
        }
    }

    public class ApplicationExpression: Expression
    {
        private Expression rator;
        private Expression rand;

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


    //PARSETREE CLASSESs, Maybe remove accept methods from here on and down. 
    public class ProgramNode : Node
    {
        public ProgramNode() : base("Program") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class TypeDeclarartionNode : Node
    {
        public TypeDeclarartionNode() : base("TypeDeclarartion") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class DeclarartionsNode : Node
    {
        public DeclarartionsNode() : base("Declarations") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class FunctionDeclarartionNode : Node
    {
        public FunctionDeclarartionNode() : base("FunctionDeclarartion") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }
    public class ClauseNode : Node
    {
        public ClauseNode() : base("Clause") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }
    public class DefaultClauseNode : Node
    {
        public DefaultClauseNode() : base("DefaultClause") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class ExpressionNode : Node
    {
        public ExpressionNode() : base("Expression") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class IfExpressionNode : Expression
    {
        public IfExpressionNode() : base("IfExpression") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class LetExpressionNode : Node
    {
        public LetExpressionNode() : base("LetExpression") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class AnonFuncNode : Node
    {
        public AnonFuncNode() : base("AnonFunc") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class StructureExpressionNode : Node
    {
        public StructureExpressionNode() : base("StructureExpression") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class ListExpressionNode : Node
    {
        public ListExpressionNode() : base("ListExpression") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class TupleExpressionNode : Node
    {
        public TupleExpressionNode() : base("TupleExpression") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

    public class ConstantExpressionNode : Node
    {
        public ConstantExpressionNode() : base("ConstantExpression") { }
        public override void accept(IVisitor v) { v.visit(this); }
    }

}
