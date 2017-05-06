using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
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
