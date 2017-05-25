using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public interface ITypeVisitor
    {
        ConstructedType Visit(TypeScheme A);
        ConstructedType Substitute(ConstructedType A);
        ConstructedType Visit(Polytype A);
        ConstructedType Substitute(FunctionType A);
        ConstructedType Substitute(TupleType A);
        ConstructedType Substitute(ListType A);
        ConstructedType Substitute(TypeVar A);
        ConstructedType Substitute(BasicType A);
    }
}
