
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    class StrengType : BasicType
    {
        public override ConstructedType accept(TypeSubstitution typeSub)
        {
            return typeSub.Substitute(this);
        }

    }
}
