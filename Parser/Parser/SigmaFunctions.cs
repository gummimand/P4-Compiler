using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class SigmaFunctions
    {
        Dictionary<string, ConstructedType> SigmaTable = new Dictionary<string, ConstructedType>();

        public SigmaFunctions()
        {
            FillSigmaTable();
        }

        public void FillSigmaTable()
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
            SigmaTable.Add("!=", new FunctionType(new FunctionType(new BasicType(), new BasicType()), new BoolType()));
            SigmaTable.Add("Par", 
            SigmaTable.Add("Liste"

        }

        public ConstructedType Lookup(string constant)
        {
            ConstructedType output;

            if (SigmaTable.TryGetValue(constant, out  output))
            {
                return output;
            }
            else
                throw new Exception("Constant not regonized");
        } 


    }
}
