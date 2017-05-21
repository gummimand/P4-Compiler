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

        public override bool Equals(object obj)
        {
            FunctionType other = obj as FunctionType;

            if (other != null)
            {
                return this.inputType.Equals(other.inputType) && this.outputType.Equals(other.outputType);
            }
            else
            {
                return false;
            }
        }

        public override ConstructedType accept(TypeSubstitution typeSub)
        {
            return typeSub.Substitute(this);
        }

        public override string ToString()
        {
            return inputType.ToString() +" -> "+ outputType.ToString();
        }

    }

}
