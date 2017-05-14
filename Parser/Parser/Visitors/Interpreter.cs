using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject.Visitors
{
    public class Interpreter : IVisitor<Node>
    {

        public Node Interpret(AST ast)
        {
            return visit(ast.Root);
        }


        public Node visit(Node node)
        {
            throw new NotImplementedException();
        }

        public Node visit(Value node)
        {
            // return node.value;
            throw new NotImplementedException();
        }

        public Node visit(Decl node)
        {
            throw new NotImplementedException();
        }

        public Node visit(EmptyDecl node)
        {
            throw new NotImplementedException();
        }

        public Node visit(IfExpression node)
        {

            // if1
            // hvis node.condition er et udtryk, reducer condition
            //if2
            // hvis node.condition = sand reducer node.alt1
            //if3
            // hvis node.condition = falsk reducer node.alt1

            bool cond = true; // todo skal bruge resultatet af visit(condition)

            object condition = visit(node.condition); //returner værdien
            var a = condition as Value;
            if (a != null)
            {
                cond = a.token.content == "sand";
            }
            else
            {
                throw new Exception("aaaaaaaaaaaaaaaaaaaaaaaaaarh!");
            }

            
            if (cond)
            {
                return visit(node.alt1);
            }
            else
            {
                return visit(node.alt2);
            }
        }

        public Node visit(AnonFuncExpression node)
        {
            return node;
        }

        public Node visit(IdentifierExpression node)
        {
            throw new NotImplementedException();
        }

        public Node visit(EmptyExpression node)
        {
            return node;
        }

        public Node visit(ApplicationExpression node)
        {
            Node operand = visit(node.rand); //app1
            Node rator = visit(node.rator); //app2

            if (rator is ConstantExpression) // hvis rator er const
            {
                //apply(c, rand)
            }
            else if(rator is AnonFuncExpression) // hvis rator er Anonfunk
            {
                AnonFuncExpression function = rator as AnonFuncExpression;

                //E[function.arg -> rand]
                return visit(function.exp);
            }

            throw new NotImplementedException();
        }

        public Node visit(ValueExpression node)
        {
            return visit(node.value);
        }

        public Node visit(LetExpression node)
        {
            throw new NotImplementedException();
        }

        public Node visit(Expression node)
        {
            throw new NotImplementedException();
        }

        public Node visit(VarDecl node)
        {
            throw new NotImplementedException();
        }

        public Node visit(ProgramAST node)
        {
            throw new NotImplementedException();
        }

        public Node visit(Identifier node)
        {
            throw new NotImplementedException();
        }

        public Node visit(Leaf leaf)
        {
            throw new NotImplementedException();
        }

        public Node visit(ASTNode node)
        {
            throw new NotImplementedException();
        }
    }
}
