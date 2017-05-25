using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class TypeChecker : IVisitor
    {
        #region Fields

        public TypeEnv typeEnvironment = new TypeEnv(); //obsolete

        private Dictionary<ConstantExpression, ConstructedType> Signatures = new Dictionary<ConstantExpression, ConstructedType>
        {
            {new PlusConst(), new FunctionType(new TalType(), new FunctionType(new TalType(), new TalType()))},
            {new MinusConst(), new FunctionType(new TalType(), new FunctionType(new TalType(), new TalType()))},
            {new TimesConst(), new FunctionType(new TalType(), new FunctionType(new TalType(), new TalType()))},
            {new DivideConst(), new FunctionType(new TalType(), new FunctionType(new TalType(), new TalType()))},
            {new ModuloConst(), new FunctionType(new TalType(), new FunctionType(new TalType(), new TalType()))},
            {new PotensConst(), new FunctionType(new TalType(), new FunctionType(new TalType(), new TalType()))}
        }; //obsolete

        private TypeUnifier uni = new TypeUnifier();
        private TypeCloser clo = new TypeCloser();

        #endregion

        public void CheckType(AST ast)
        {
            visit(ast.Root);
        }

        public void visit(ASTNode node)
        {
            node.accept(this);
        }

        #region Visitor methods for Program node

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

        #endregion

        #region Visitor methods for Declaration nodes

        public void visit(EmptyDecl node)
        {
            node.Type = new OkType();
        }

        public void visit(VarDecl node)
        {
            TypeEnv E = node.E;

            string x = node.id.token.content;

            E.Add(x, new TypeVar());
            node.exp.E = E;
            visit(node.exp);
            E.Remove();

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

        #endregion

        #region Visitor methods for Expression nodes

        // [HVIS]-rule
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

        // [APP]-rule
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

        // [VAR]-rule
        public void visit(IdentifierExpression node)
        {
            node.Type = GetTypeFromTypeScheme(node.E.LookUp(node.varName));
            node.sigma = new TypeSubstitution();
        }

        private ConstructedType GetTypeFromTypeScheme(TypeScheme A)
        {
            if (A is ConstructedType)
            {
                return A as ConstructedType;
            }
            else if (A is Polytype)
            {
                Polytype a = A as Polytype;
                return GetTypeFromTypeScheme(a.TypeScheme);
            }
            else
            {
                throw new Exception($"Could not retrieve type of {A.ToString()}");
            }

        }

        public void visit(EmptyExpression node)
        {
            // no type
        }

        public void visit(ClosureExpression node)
        {
            visit(node.exp);
        }

        // [ANON]-rule
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

        // [LAD]-rule
        public void visit(LetExpression node)
        {
            TypeEnv E = node.E.Clone();
            string x = node.id.token.content;
            node.exp1.E = E;
            visit(node.exp1);

            TypeSubstitution sigma1 = node.exp1.sigma;
            ConstructedType type1 = node.exp1.Type;

            TypeScheme closure = clo.Close(E, type1);
            TypeEnv E_ = sigma1.Substitute(E);
            E_.Add(x, closure);

            node.exp2.E = E_;
            visit(node.exp2);

            TypeSubstitution sigma2 = node.exp2.sigma;
            ConstructedType type2 = node.exp2.Type;

            node.sigma = sigma2.Compose(sigma1);
            node.Type = type2;
        }

        #endregion

        #region Visitor methods for Constant nodes

        /*
         *  For each constant, The signature (big sigma) is defined for each node for convenience
         */

        public void visit(EmptyListExpression node)
        {
            node.Type = new ListType(new TypeVar());
            node.sigma = new TypeSubstitution();

        }

        public void visit(PairConst node)
        {
            TypeVar a = new TypeVar();
            TypeVar b = new TypeVar();
            node.Type = new FunctionType(a.Clone(), new FunctionType(b.Clone(), new TupleType(a.Clone(), b.Clone())));
            node.sigma = new TypeSubstitution();
        }

        public void visit(MinusConst node)
        {
            node.Type = new FunctionType(new HeltalType(), new FunctionType(new HeltalType(), new HeltalType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(ListConst node)
        {
            TypeVar a = new TypeVar();
            node.Type = new FunctionType(a.Clone(), new FunctionType(new ListType(a.Clone()), new ListType(a.Clone())));
            node.sigma = new TypeSubstitution();
        }

        public void visit(ConstantFuncs node)
        {
            node.Type = Signatures[node];
            node.sigma = new TypeSubstitution();
        }

        public void visit(PlusConst node)
        {
            node.Type = new FunctionType(new HeltalType(), new FunctionType(new HeltalType(), new HeltalType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(TimesConst node)
        {
            node.Type = new FunctionType(new HeltalType(), new FunctionType(new HeltalType(), new HeltalType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(DivideConst node)
        {
            node.Type = new FunctionType(new HeltalType(), new FunctionType(new HeltalType(), new HeltalType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(PotensConst node)
        {
            node.Type = new FunctionType(new HeltalType(), new FunctionType(new HeltalType(), new HeltalType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(ModuloConst node)
        {
            node.Type = new FunctionType(new HeltalType(), new FunctionType(new HeltalType(), new HeltalType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(EqualConst node)
        {
            TypeVar a = new TypeVar();
            node.Type = new FunctionType(a.Clone(), new FunctionType(a.Clone(), new BoolType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(NotEqualConst node)
        {
            TypeVar a = new TypeVar();
            node.Type = new FunctionType(a.Clone(), new FunctionType(a.Clone(), new BoolType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(LesserThanConst node)
        {
            TypeVar a = new TypeVar();
            node.Type = new FunctionType(a.Clone(), new FunctionType(a.Clone(), new BoolType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(GreaterThanConst node)
        {
            TypeVar a = new TypeVar();
            node.Type = new FunctionType(a.Clone(), new FunctionType(a.Clone(), new BoolType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(GreaterThanOrEqualConst node)
        {
            TypeVar a = new TypeVar();
            node.Type = new FunctionType(a.Clone(), new FunctionType(a.Clone(), new BoolType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(LesserThanOrEqualConst node)
        {
            TypeVar a = new TypeVar();
            node.Type = new FunctionType(a.Clone(), new FunctionType(a.Clone(), new BoolType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(NotConst node)
        {
            node.Type = new FunctionType(new BoolType(), new BoolType());
            node.sigma = new TypeSubstitution();
        }

        public void visit(HeadConst node)
        {
            TypeVar a = new TypeVar();
            node.Type = new FunctionType(new ListType(a.Clone()), a.Clone());
            node.sigma = new TypeSubstitution();
        }

        public void visit(TailConst node)
        {
            TypeVar a = new TypeVar();
            node.Type = new FunctionType(new ListType(a.Clone()), new ListType(a.Clone()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(OrConst node)
        {
            node.Type = new FunctionType(new BoolType(), new FunctionType(new BoolType(), new BoolType()));
            node.sigma = new TypeSubstitution();
        }

        public void visit(AndConst node)
        {
            node.Type = new FunctionType(new BoolType(), new FunctionType(new BoolType(), new BoolType()));
            node.sigma = new TypeSubstitution();
        }
        #endregion


        public void visit(ConcatConst node) {
            
        }
    }
}
