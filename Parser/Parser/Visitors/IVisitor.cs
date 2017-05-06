using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public interface IVisitor
    {
        void visit(ASTNode node);
        void visit(Leaf leaf);
        void visit(Node node);
        void visit(Identifier node);
        void visit(Value node);
        void visit(ProgramAST node);
        void visit(Decl node);
        void visit(VarDecl node);
        void visit(EmptyDecl node);
        void visit(Expression node);
        void visit(IfExpression node);
        void visit(LetExpression node);
        void visit(AnonFuncExpression node);
        void visit(ValueExpression node);
        void visit(IdentifierExpression node);
        void visit(ApplicationExpression node);
        void visit(EmptyExpression node);

        //One for each node ... zzzz
    }
}
