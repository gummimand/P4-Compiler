using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class TypeSubstitution
    {
        public List<Tuple<TypeVar, ConstructedType>> table = new List<Tuple<TypeVar, ConstructedType>>();

        public void Add(TypeVar typeVar, ConstructedType type)
        {
            table.Insert(0, Tuple.Create(typeVar, type));
        }

        public TypeEnv Substitute(TypeEnv E)
        {
            TypeEnv newE = new TypeEnv();

            foreach (var item in E.table)
            {
                newE.Add(item.Item1, Substitute(item.Item2));
            }

            return newE;
        }

        public ConstructedType Substitute(TypeVar typevar)
        {
            if (table.Exists(p => p.Item1.Equals(typevar)))
            {
                return table.Find(type => type.Item1.Equals(typevar)).Item2;
            }
            else
            {
                return typevar;
            }
        }

        public ConstructedType Substitute(FunctionType type)
        {
            return new FunctionType(Substitute(type.inputType), Substitute(type.outputType));
        }

        public ConstructedType Substitute(ListType type)
        {
            return new ListType(Substitute(type.ListElementType));
        }

        public ConstructedType Substitute(TupleType type)
        {
            return new TupleType(Substitute(type.Element1),Substitute(type.Element2));
        }

        public ConstructedType Substitute(BasicType type)
        {
            return type;
        }

        public ConstructedType Substitute(ConstructedType type)
        {
            return type.accept(this);
        }

        public TypeSubstitution Compose(TypeSubstitution sigma)
        {
            TypeSubstitution newSigma = new TypeSubstitution();
            foreach (var sigmaItem in sigma.table)
            {
                newSigma.Add(sigmaItem.Item1, Substitute(sigmaItem.Item2)); 
            }
            foreach (var sigmaItem in table)
            {
                newSigma.Add(sigmaItem.Item1, sigmaItem.Item2);
            }

            return newSigma;
        }



    }
}
