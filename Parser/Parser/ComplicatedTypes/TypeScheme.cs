using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public abstract class TypeScheme
    {

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
    }
}
