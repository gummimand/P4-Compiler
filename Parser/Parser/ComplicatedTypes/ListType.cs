using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class ListType : ConstructedType
    {
        public ConstructedType ListElementType;

        public ListType() //for interpreter
        {

        }

        public ListType(ConstructedType T)
        {
            ListElementType = T;
        }

        public override ConstructedType accept(TypeSubstitution typeSub)
        {
            return typeSub.Substitute(this);
        }

        public override string ToString() {
            return "ListType";
        }

    }
}
