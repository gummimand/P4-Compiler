using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    class PrintVisitor : IVisitor
    {
        public void visit(Node node)
        {
            throw new NotImplementedException();
        }

        public void visit(ConstantFuncs node)
        {

        }

        //public void visit(Operator node)
        //{
        //    throw new NotImplementedException();
        //}

        public void visit(Value node)
        {
            throw new NotImplementedException();
        }

        public void visit(Decl node)
        {
            throw new NotImplementedException();
        }

        //public void visit(SeqDecl node)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visit(TypeDecl node)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visit(DatatypeLabelPair node)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visit(DefaultClause node)
        //{
        //    throw new NotImplementedException();
        //}

        public void visit(Expression node)
        {
            throw new NotImplementedException();
        }

        //public void visit(OperatorExpression node)
        //{
        //    throw new NotImplementedException();
        //}

        public void visit(LetExpression node)
        {
            throw new NotImplementedException();
        }

        public void visit(ValueExpression node)
        {
            throw new NotImplementedException();
        }

        //public void visit(StructureExpression node)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visit(TupleExpression node)
        //{
        //    throw new NotImplementedException();
        //}

        public void visit(EmptyExpression node)
        {
            throw new NotImplementedException();
        }

        public void visit(ApplicationExpression node)
        {
            throw new NotImplementedException();
        }

        public void visit(EmptyListExpression node)
        {
            throw new NotImplementedException();
        }

        public void visit(PairConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(TimesConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(PotensConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(EqualConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(LesserThanConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(GreaterThanOrEqualConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(LesserThanOrEqualConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(GreaterThanConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(NotEqualConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(ModuloConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(DivideConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(MinusConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(PlusConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(ListConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(ClosureExpression node)
        {
            throw new NotImplementedException();
        }

        //public void visit(ListExpression node)
        //{
        //    throw new NotImplementedException();
        //}

        public void visit(IdentifierExpression node)
        {
            throw new NotImplementedException();
        }

        public void visit(AnonFuncExpression node)
        {
            throw new NotImplementedException();
        }

        public void visit(IfExpression node)
        {
            throw new NotImplementedException();
        }

        //public void visit(ConstrExpression node)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visit(ConditionalClause node)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visit(Clause node)
        //{
        //    throw new NotImplementedException();
        //}

        public void visit(EmptyDecl node)
        {
            throw new NotImplementedException();
        }

        //public void visit(FuncDecl node)
        //{
        //    throw new NotImplementedException();
        //}

        public void visit(VarDecl node)
        {
            throw new NotImplementedException();
        }

        public void visit(ProgramAST node)
        {
            throw new NotImplementedException();
        }

        //public void visit(Constructor node)
        //{
        //    throw new NotImplementedException();
        //}

        public void visit(Identifier node)
        {
            throw new NotImplementedException();
        }

        public void visit(Leaf leaf)
        {
            throw new NotImplementedException();
        }

        public void visit(ASTNode node)
        {
            throw new NotImplementedException();
        }

        /*
        public void visit(ConcatConst node)
        {
            throw new NotImplementedException();
        }
        */

        public void visit(NotConst node)
        {
            throw new NotImplementedException();
        }
    }
}
