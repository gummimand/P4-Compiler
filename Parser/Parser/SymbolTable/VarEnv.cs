using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class VarEnv : Symboltable<Expression>
    {

        public void Print()
        {

            for (int i = table.Count - 1; i >= 0; i--)
            {
                Console.WriteLine(table[i].Item1 + " = " + table[i].Item2.ToString() + " : " + table[i].Item2.Type.ToString());
            }
        }
    }
}
