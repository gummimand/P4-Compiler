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

        public override bool Equals(object obj)
        {
            TupleType other = obj as TupleType;

            if (other != null)
            {
                return this.Element1.Equals(other.Element1) && this.Element2.Equals(other.Element2);
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Element1.ToString() + " * " + Element2.ToString(); 
        }
    }
}