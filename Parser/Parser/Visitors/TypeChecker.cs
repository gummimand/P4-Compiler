using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class TypeChecker : IVisitor
    {
        public TypeEnv typeEnvironment = new TypeEnv();
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

        public void CheckType(AST ast)
        {
            visit(ast.Root);
        }

        public void visit(IfExpression node)
        {
            TypeEnv E = node.E.Clone();

            node.condition.E = E;
            visit(node.condition);

            TypeSubstitution sigma1 = node.condition.sigma;
            ConstructedType type1 = node.condition.Type;

            if (!(type1 is BoolType))
            {
                throw new Exception("Condition was not of type Bool");
            }

            TypeEnv E2 = sigma1.Substitute(E);
            node.alt1.E = E2;
            visit(node.alt1);

            ConstructedType type2 = node.alt1.Type;
            TypeSubstitution sigma2 = node.alt1.sigma;

            node.alt2.E = sigma2.Substitute(E2);
            visit(node.alt2);

            ConstructedType type3 = node.alt2.Type;
            TypeSubstitution sigma3 = node.alt2.sigma;

            TypeSubstitution sigma = uni.Unify(sigma3.Substitute(type2), type3);

            node.sigma = sigma.Compose(sigma3.Compose(sigma2.Compose(sigma1)));
            node.Type = sigma.Substitute(type2);
        }

        public void visit(AnonFuncExpression node)
        {
            string x = node.arg.token.content;// todo make arg identifierExpression an replace node.arg.varName;
            TypeVar a = new TypeVar();

            TypeEnv E = node.E.Clone();
            E.Add(x, a); 

            node.exp.E = E;
            visit(node.exp);

            TypeSubstitution sigma = node.exp.sigma;
            ConstructedType type = node.exp.Type;

            node.sigma = sigma;
            node.Type = new FunctionType(sigma.Substitute(a), type);
        }

        public void visit(IdentifierExpression node)
        {
            node.Type = node.E.LookUp(node.varName);
            node.sigma = new TypeSubstitution();
        }

        public void visit(EmptyExpression node)
        {
            // no type
        }

        public void visit(EmptyListExpression node)
        {
            node.Type = new ListType(new TypeVar());
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
            TypeEnv E = node.E.Clone();

            node.function.E = E;
            visit(node.function);
            TypeSubstitution sigma1 = node.function.sigma;
            ConstructedType type1 = node.function.Type;

            node.argument.E = sigma1.Substitute(E); 
            visit(node.argument);

            TypeSubstitution sigma2 = node.argument.sigma;
            ConstructedType type2 = node.argument.Type;

            TypeVar a = new TypeVar();

            TypeSubstitution sigma = uni.Unify(sigma2.Substitute(type1), new FunctionType(type2, a));

            node.sigma = sigma.Compose(sigma1.Compose(sigma2));
            node.Type = sigma.Substitute(a);
        }

        public void visit(ValueExpression node)
        {
            // har allerede type
            node.sigma = new TypeSubstitution();
        }

        public void visit(LetExpression node)
        {
            TypeEnv E = node.E.Clone();
            string x = node.id.token.content;
            node.exp1.E = E;
            visit(node.exp1);

            TypeSubstitution sigma1 = node.exp1.sigma;
            ConstructedType type1 = node.exp1.Type;

            TypeScheme closure = Close(E, type1);
            TypeEnv E_ = sigma1.Substitute(E);
           // E_.Add(x, closure);  TODO make E var -> typeScheme

            node.exp2.E = E_;
            visit(node.exp2);

            TypeSubstitution sigma2 = node.exp2.sigma;
            ConstructedType type2 = node.exp2.Type;

            node.sigma = sigma2.Compose(sigma1);
            node.Type = type2;
        }

        public void visit(EmptyDecl node)
        {
            node.Type = new OkType();
        }

        public void visit(VarDecl node)
        {
            TypeEnv E = node.E;

            string x = node.id.token.content;

            node.exp.E = E;
            visit(node.exp);

            TypeSubstitution sigma = node.exp.sigma;
            ConstructedType type = node.exp.Type;

            TypeEnv E_ = E;
            E_.Add(x, type);

            node.nextDecl.E = E_;
            visit(node.nextDecl);

            if (node.nextDecl.Type is OkType)
            {
                node.Type = new OkType();
                node.sigma = node.nextDecl.sigma;
            }
            else
            {
                throw new Exception("Declaration was not well typed");
            }

        }

        public void visit(ProgramAST node)
        {
            TypeEnv E = new TypeEnv();

            node.varDecl.E = E;
            visit(node.varDecl);

            ConstructedType type = node.varDecl.Type;
            TypeSubstitution sigma = node.varDecl.sigma;

            if (!(type is OkType))
            {
                throw new Exception("Program was not well typed");
            }

            node.exp.E = E;
            visit(node.exp);

            node.Type = node.exp.Type;
        }

        public void visit(ConstantFuncs node)
        {
            node.Type = Sigmatabel.Lookup(node.name); 
        }

        public void visit(PlusConst node)
        {
            node.Type = new FunctionType(new HeltalType(), new FunctionType(new HeltalType(), new HeltalType()));
            node.sigma = new TypeSubstitution();
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
            TypeVar a = new TypeVar();
            node.Type = new FunctionType(a.Clone(), new FunctionType(a.Clone(), new BoolType()));
            node.sigma = new TypeSubstitution();
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

        public TypeScheme Close(TypeEnv E, ConstructedType t)
        {
            List<TypeVar> FreeOft = FTV(t);
            List<TypeVar> FreeOfE = FTV(E);

            List<TypeVar> closeSet = new List<TypeVar>();

            TypeScheme closure = t;

            foreach (TypeVar item in FreeOft)
            {
                if (!FreeOfE.Contains(item))
                {
                    closeSet.Add(item);
                }
            }

            foreach (TypeVar item in closeSet)
            {
                closure = new Polytype(item, closure);
            }

            return closure;
        }

        private List<TypeVar> FTV(ConstructedType t)
        {
            // return all typevars
            throw new NotImplementedException();
        }

        private List<TypeVar> FTV(Polytype t)
        {
            //return all FTV(t.typeScheme) - t.typevarriable, which is bound.
            throw new NotImplementedException();
        }

        private List<TypeVar> FTV(TypeEnv E)
        {
            // iterate over all x in E and append FTV(x) to list and return it
            throw new NotImplementedException();
        }

        public void visit(ASTNode node)
        {
            node.accept(this);
        }
    }
}
