using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class FunctionType : ConstructedType
    {
        object input;
        object output;

        public FunctionType(BasicType T1, BasicType T2)
        {
            input = T1;
            output = T2;
        }
        public FunctionType(BasicType T1, ConstructedType T2)
        {
            input = T1;
            output = T2;
        }
        public FunctionType(ConstructedType T1, ConstructedType T2)
        {
            input = T1;
            output = T2;
        }
        
    }

}
