using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class ConstructedType: TypeScheme
    {
        public ConstructedType() { } //So the other classes can have constructors

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
