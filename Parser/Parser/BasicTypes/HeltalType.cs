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
    }
}
