using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public abstract class TypeScheme
    {
        public virtual List<TypeVar> Accept(TypeCloser C)
        {
            return C.FTV(this);
        }

        public abstract ConstructedType accept(TypeSubstitution typeSub);
    }


    public class Polytype : TypeScheme
    {
        public TypeVar TypeVariable;
        public TypeScheme TypeScheme;

        public Polytype(TypeVar a, TypeScheme A)
        {
            this.TypeVariable = a;
            this.TypeScheme = A;
        }

        public override string ToString()
        {
            return "\u2200" + TypeVariable.ToString() + "." + TypeScheme.ToString();
        }

        public override ConstructedType accept(TypeSubstitution typeSub)
        {
            return typeSub.Substitute(this);
        }

        public override List<TypeVar> Accept(TypeCloser C)
        {
            return C.FTV(this);
        }
    }
}
