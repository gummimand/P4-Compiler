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

        public ListType(ConstructedType T)
        {
            ListElementType = T;
        }

        public override ConstructedType accept(TypeSubstitution typeSub)
        {
            return typeSub.Substitute(this);
        }

        public override List<TypeVar> Accept(TypeCloser C)
        {
            return C.FTV(this);
        }

        public override bool Equals(object obj)
        {
            ListType other = obj as ListType;

            if (other != null)
            {
                return this.ListElementType.Equals(other.ListElementType);
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return "{" + ListElementType.ToString() + "}";
        }

    }
}
