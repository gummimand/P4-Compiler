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

        public ConstructedType Substitute(TypeVar typevar)
        {
            return table.Find(type => type.Item1.id == typevar.id).Item2;
        }

        public ConstructedType Substitute(FunctionType type)
        {
            return new FunctionType(Substitute(type.inputType), Substitute(type.outputType));
        }

        public TypeSubstitution Compose(TypeSubstitution sigma)
        {
            TypeSubstitution newSigma = new TypeSubstitution();
            foreach (var sigmaItem in sigma.table)
            {
                var newtype = Substitute(sigmaItem.Item2);
                newSigma.Add(sigmaItem.Item1, newtype); 
            }
        }



    }
}
