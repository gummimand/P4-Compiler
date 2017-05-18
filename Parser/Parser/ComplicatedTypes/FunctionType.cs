using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class FunctionType : ConstructedType
    {
        public ConstructedType inputType;
        public ConstructedType outputType;

        public FunctionType(ConstructedType input, ConstructedType output)
        {
            this.inputType = input;
            this.outputType = output;
        }
        
    }

}
