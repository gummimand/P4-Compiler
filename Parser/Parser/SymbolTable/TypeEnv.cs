using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class TypeEnv : Symboltable<TypeScheme>
    {
        public TypeEnv Clone()
        {
            TypeEnv clone = new TypeEnv();

            foreach (var item in table)
            {
                clone.AddTuple(item);
            }

            return clone;
        }

    }
}
