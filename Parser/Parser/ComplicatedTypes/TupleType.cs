using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class TupleType : ConstructedType
    {
        public ConstructedType Element1;
        public ConstructedType Element2;

        public TupleType(ConstructedType T1, ConstructedType T2)
        {
            Element1 = T1;
            Element2 = T2;
        }

        public override ConstructedType accept(TypeSubstitution typeSub)
        {
            return typeSub.Substitute(this);
        }
    }
}