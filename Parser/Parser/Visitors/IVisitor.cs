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
        void visit(ConcatConst node);
        void visit(NotConst node);
        void visit(HeadConst node);
        void visit(TailConst node);
        void visit(OrConst node);
        void visit(AndConst node);
        void visit(SecondConst node);
        void visit(SelectConst node);
        void visit(FirstConst node);
    }
}
