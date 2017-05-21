using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class Interpreter : IVisitor
    {

        public Symboltable<Expression> env = new Symboltable<Expression>();

        public void Interpret(AST ast)
        {
            visit(ast.Root);

            env.Print();
        }

        private Expression Apply(ConstantExpression c, Expression exp)
        {
            if (c is PlusConst && exp.Value is ValueExpression)
            {
                ValueExpression valExp = exp.Value as ValueExpression;
                return new PlusConstN(valExp);                
            }
            else if(c is PlusConstN && exp.Value is ValueExpression)
            {
                PlusConstN constExp = c as PlusConstN;
                ValueExpression valExp = exp.Value as ValueExpression;

                if (constExp.Nval.Type is TalType || valExp.Type is TalType)
                {
                    double n = double.Parse(constExp.Nval.val);
                    double m = double.Parse(valExp.val);

                    return new ValueExpression((n + m).ToString(), TokenType.tal);
                }
                else
                {
                    int n = int.Parse(constExp.Nval.val);
                    int m = int.Parse(valExp.val);

                    return new ValueExpression((n + m).ToString(), TokenType.heltal);
                }

            }
            else
            {
                throw new Exception("no!");
            }
        }


        public void visit(ASTNode node)
        {
            node.accept(this);
        }


        public void visit(EmptyDecl node)
        {
            // do nothing
        }

        public void visit(IfExpression node)
        {

            // if1
            // hvis node.condition er et udtryk, reducer condition

            //if2
            // hvis node.condition = sand reducer node.alt1

            //if3
            // hvis node.condition = falsk reducer node.alt1
            visit(node.condition);

            bool truth;

            // TODO clean up + handle exceptions

            object val = node.condition.Value;
            ValueExpression valexp = val as ValueExpression;
            string valString = valexp.val;

            truth = valString == "sand";
            
            if (truth)
            {
                visit(node.alt1);
                node.Value = node.alt1.Value;
            }
            else
            {
                visit(node.alt2);
                node.Value = node.alt2.Value;
            }
        }

        public void visit(AnonFuncExpression node)
        {
            // er sin egen værdi (fingers crossed)
            node.Value = node;
        }

        public void visit(EmptyListExpression node)
        {
            node.Value = node;
        }

        public void visit(PairConst node)
        {
            node.Value = node;
        }

        public void visit(MinusConst node)
        {
            node.Value = node;
        }

        public void visit(PlusConst node)
        {
            node.Value = node;
        }

        public void visit(ListConst node)
        {
            node.Value = node;
        }

        public void visit(ClosureExpression node)
        {
            throw new NotImplementedException();
        }

        public void visit(IdentifierExpression node)
        {
            node.Value = env.LookUp(node.varName);
        }

        public void visit(EmptyExpression node)
        {
            // do nothing
        }

        public void visit(ApplicationExpression node)
        {
            visit(node.argument); //app1
            visit(node.function); //app2

            if (node.function.Value is ConstantExpression) // hvis funk er const
            {
                ConstantExpression c = node.function.Value as ConstantExpression;
                node.Value = Apply(c, node.argument);
            }
            else if(node.function.Value is AnonFuncExpression) // hvis funk er Anonfunk
            {
                AnonFuncExpression function = node.function.Value as AnonFuncExpression;

                env.Add(function.arg.token.content, node.argument.Value);
                visit(function.exp);
                node.Value = function.exp.Value;
                env.Remove();
            }
            else
            {
                throw new Exception("SHit!");
            }
        }

        public void visit(ValueExpression node)
        {
            node.Value = node;
        }

        public void visit(LetExpression node)
        {
            //[LET1]
            visit(node.exp1);

            //[LET2]
            env.Add(node.id.token.content, node.exp1.Value);
            visit(node.exp2);
            node.Value = node.exp2.Value;
            env.Remove();
        }

        public void visit(VarDecl node)
        {
            visit(node.exp);
            env.Add(node.id.token.content, node.exp.Value);
            visit(node.nextDecl);
        }

        // [PROG]
        public void visit(ProgramAST node)
        {
            visit(node.varDecl);
            visit(node.exp);
        }

        public void visit(ConstantFuncs node)
        {
            throw new NotImplementedException();
        }
    }
}
