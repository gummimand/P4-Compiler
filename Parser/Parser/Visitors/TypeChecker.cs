using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class TypeChecker : IVisitor
    {
        public Symboltable<ConstructedType> E = new Symboltable<ConstructedType>();
        public SigmaFunctions Sigmatabel = new SigmaFunctions();

        private TypeUnifier uni = new TypeUnifier();

        public Dictionary<string, string> sigmaConstants = new Dictionary<string, string>() {
            {"PLUS","heltal->heltal->heltal"},
            {"PLUS2","tal->heltal->tal" },
            {"MINUS","heltal->heltal->heltal"},
            {"MINUS2","tal->heltal->tal" },
            {"GANGE","heltal->heltal->heltal"},
            {"DIVIDER","heltal->heltal->heltal"},
            {"MODULUS","heltal->heltal->heltal"},
            {"POTENS","heltal->heltal->heltal"},
            {"NOT","bool->bool"},
            {"OG","bool->bool->bool"},
            {"ELLER","bool->bool->bool"},
            {"STØRRE","heltal->heltal->bool"},
            {"MINDRE","heltal->heltal->bool"},
            {"STØRREELLERLIGMED","heltal->heltal->bool"},
            {"MINDREELLERLIGMED","heltal->heltal->bool"},
            {"LIGMED","heltal->heltal->bool"},
            {"PAR","a->b->a*b"},//Better placeholder.
            {"LISTE","a->{a}->{a}"}//Better placeholder.
        };

        public void visit(IfExpression node)
        {
            throw new NotImplementedException();
        }

        public void visit(AnonFuncExpression node)
        {
            TypeVar a = new TypeVar(); // Todo ny var hver gang
            E.Add(node.arg.token.content, a);
            visit(node.exp);
            E.Remove();


            node.sigma = node.exp.sigma;
            node.Type = new FunctionType(node.sigma.Substitute(a), node.exp.Type);

        }

        public void visit(IdentifierExpression node)
        {
            node.Type = E.LookUp(node.varName);
        }

        public void visit(EmptyExpression node)
        {
            // no type
        }

        public void visit(EmptyListExpression node)
        {
            throw new NotImplementedException();
        }

        public void visit(PairConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(MinusConst node)
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

        public void visit(ApplicationExpression node)
        {

            visit(node.argument);
            visit(node.function);

            var sigma1 = node.function.sigma;
            var sigma2 = node.argument.sigma;

            var t1 = node.function.Type;
            var t2 = node.argument.Type;

            TypeVar a = new TypeVar();

            TypeSubstitution sigma = uni.Unify(sigma2.Substitute(t1), new FunctionType(t2, a));

            node.sigma = sigma.Compose(sigma1.Compose(sigma2));
            node.Type = sigma.Substitute(a);
        }

        public void visit(ValueExpression node)
        {
            // har allerede type
        }

        public void visit(LetExpression node)
        {
            visit(node.exp1);
            var sigma1 = node.exp1.sigma;
            var t1 = node.exp1.Type;

            //var closure = close();
        }

        public void visit(EmptyDecl node)
        {
            throw new NotImplementedException();
        }

        public void visit(VarDecl node)
        {
            throw new NotImplementedException();
        }

        public void visit(ProgramAST node)
        {
            throw new NotImplementedException();
        }

        public void visit(ASTNode node)
        {
            throw new NotImplementedException();
        }

        public void visit(ConstantFuncs node)
        {
            node.Type = Sigmatabel.Lookup(node.name); 
        }

        public void visit(PlusConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(TimesConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(DivideConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(PotensConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(ModuloConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(EqualConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(NotEqualConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(LesserThanConst node)
        {
            throw new NotImplementedException();
        }

        public void visit(GreaterThanConst node)
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
    }
}
