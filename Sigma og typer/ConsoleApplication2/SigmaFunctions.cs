using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class SigmaFunctions
    {
        Dictionary<string, ConstructedType> SigmaTable = new Dictionary<string, ConstructedType>();

        public SigmaFunctions()
        {
            FillSigmaTable();
        }

        public void FillSigmaTable() //Contains operators only, numbers, strings and bools are handled by the lookup method.
        {
            SigmaTable.Add("!", new FunctionType(new BoolType(), new BoolType()));
            SigmaTable.Add("+", new FunctionType(new FunctionType(new TalType(), new TalType()), new TalType()));
            SigmaTable.Add("-", new FunctionType(new FunctionType(new TalType(), new TalType()), new TalType()));
            SigmaTable.Add("*", new FunctionType(new FunctionType(new TalType(), new TalType()), new TalType()));
            SigmaTable.Add("/", new FunctionType(new FunctionType(new TalType(), new TalType()), new TalType()));
            SigmaTable.Add("%", new FunctionType(new FunctionType(new TalType(), new TalType()), new TalType()));
            SigmaTable.Add("^", new FunctionType(new FunctionType(new TalType(), new TalType()), new TalType()));
            SigmaTable.Add("<", new FunctionType(new FunctionType(new TalType(), new TalType()), new BoolType()));
            SigmaTable.Add(">", new FunctionType(new FunctionType(new TalType(), new TalType()), new BoolType()));
            SigmaTable.Add("<=", new FunctionType(new FunctionType(new TalType(), new TalType()), new BoolType()));
            SigmaTable.Add(">=", new FunctionType(new FunctionType(new TalType(), new TalType()), new BoolType()));
            SigmaTable.Add("==", new FunctionType(new FunctionType(new BasicType(), new BasicType()), new BoolType())); //Antager at man kan sammenligne om to bool udtryk evaluerer til den samme sandheds værdi.
            SigmaTable.Add("!=", new FunctionType(new FunctionType(new BasicType(), new BasicType()), new BoolType()))
        }


        public ConstructedType Lookup(string constant)
        {
            ConstructedType output;
            if (constant.All(char.IsDigit))
            {
                return new TalType();
            }
            else if (constant.First() == '"' && constant.Last() == '"')
            {
                return new StrengType();
            }
            else if (constant == "false" || constant == "true") //Maybe also "TRUE", "True", "FALSE" osv.. 
            {
                return new BoolType();
            }
            else if (SigmaTable.TryGetValue(constant, out output))
            {
                return output;
            }
            else
                throw new Exception("Constant not regonized");
        } 


    }
}
