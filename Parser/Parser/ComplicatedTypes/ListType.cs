using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    class ListType : ConstructedType
    {
        ConstructedType ListElementType;

        ListType(ConstructedType T)
        {
            ListElementType = T;
        }
        
        public void test()
        {

        }

    }
}
