using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class TupleType : ConstructedType
    {
        object Element1;
        object Element2;

        public TupleType(BasicType T1, BasicType T2)
        {
            Element1 = T1;
            Element2 = T2;
        }
        public TupleType(BasicType T1, ConstructedType T2)
        {
            Element1 = T1;
            Element2 = T2;
        }
        public TupleType(ConstructedType T1, ConstructedType T2)
        {
            Element1 = T1;
            Element2 = T2;
        }
    }
}