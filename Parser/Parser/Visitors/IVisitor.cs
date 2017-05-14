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


    public interface IVisitor<T>
    {
        T visit(ASTNode node);
        T visit(Leaf leaf);
        T visit(Node node);
        T visit(Identifier node);
        T visit(Value node);
        T visit(ProgramAST node);
        T visit(Decl node);
        T visit(VarDecl node);
        T visit(EmptyDecl node);
        T visit(Expression node);
        T visit(IfExpression node);
        T visit(LetExpression node);
        T visit(AnonFuncExpression node);
        T visit(ValueExpression node);
        T visit(IdentifierExpression node);
        T visit(ApplicationExpression node);
        T visit(EmptyExpression node);
    }
}
