using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class TypeCloser
    {
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

        public List<TypeVar> FTV(TypeVar t)
        {
            return new List<TypeVar>() { t };
        }

        private List<T> Union<T>(List<T> L1, List<T> L2)
        {
            List<T> L = new List<T>();
            L.AddRange(L1);
            L.AddRange(L2);
            return L.Distinct().ToList();
        }

        public List<TypeVar> FTV(FunctionType t)
        {
            List<TypeVar> ftv1 = FTV(t.inputType);
            List<TypeVar> ftv2 = FTV(t.outputType);

            return Union(ftv1, ftv2);
        }

        public List<TypeVar> FTV(TupleType t)
        {
            List<TypeVar> ftv1 = FTV(t.Element1);
            List<TypeVar> ftv2 = FTV(t.Element2);

            return Union(ftv1, ftv2);
        }

        public List<TypeVar> FTV(ListType t)
        {
            return FTV(t.ListElementType);
        }

        public List<TypeVar> FTV(BasicType t)
        {
            return new List<TypeVar>();
        }

        //public List<TypeVar> FTV(ConstructedType t)
        //{
        //    return t.Accept(this);
        //}

        public List<TypeVar> FTV(TypeScheme t)
        {
            return t.Accept(this);
        }

        //return all FTV(t.typeScheme) - t.typevarriable, which is bound.
        public List<TypeVar> FTV(Polytype t)
        {
            List<TypeVar> freeVars = new List<TypeVar>();

            freeVars = FTV(t.TypeScheme);

            freeVars = freeVars.Distinct().ToList();

            int i = freeVars.FindIndex(p => p.Equals(t));

            freeVars.RemoveAt(i);

            return freeVars;
        }


        // FTV(E) = union of FTV(t), for all x \in dom(E) and E(x) = t
        public List<TypeVar> FTV(TypeEnv E)
        {
            List<TypeVar> freeVars = new List<TypeVar>();

            foreach (var x in E.table)
            {
                freeVars = Union(freeVars, FTV(x.Item2));
            }

            return freeVars;
        }
    }
}
