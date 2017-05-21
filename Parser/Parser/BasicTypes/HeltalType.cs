using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class HeltalType : BasicType
    {

        public HeltalType()
        {
        }
        
        public override ConstructedType accept(TypeSubstitution typeSub)
        {
            return typeSub.Substitute(this);
        }

        public override bool Equals(object obj)
        {
            return obj is HeltalType;
        }

        public override string ToString()
        {
            return "Heltal";
        }
    }
}
