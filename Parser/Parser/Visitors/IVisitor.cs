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
        void visit(ProgramAST node);
        void visit(VarDecl node);
        void visit(EmptyDecl node);
        void visit(IfExpression node);
        void visit(LetExpression node);
        void visit(AnonFuncExpression node);
        void visit(ValueExpression node);
        void visit(IdentifierExpression node);
        void visit(ApplicationExpression node);
        void visit(EmptyExpression node);
        void visit(ClosureExpression node);
        void visit(EmptyListExpression node);
        void visit(ListConst node);
        void visit(PairConst node);
        void visit(ConstantFuncs node);
        void visit(PlusConst node);
        void visit(MinusConst node);
        void visit(TimesConst node);
        void visit(DivideConst node);
        void visit(PotensConst node);
        void visit(ModuloConst node);
        void visit(EqualConst node);
        void visit(NotEqualConst node);
        void visit(LesserThanConst node);
        void visit(LesserThanOrEqualConst node);
        void visit(GreaterThanConst node);
        void visit(GreaterThanOrEqualConst node);
<<<<<<< Updated upstream
        void visit(ConcatConst node);
        void visit(NotConst node);
=======
        void visit(NotConst node);
        void visit(ConcatConst node);
        //void visit()
>>>>>>> Stashed changes



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
