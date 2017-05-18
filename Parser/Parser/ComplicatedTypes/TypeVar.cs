using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class TypeVar : ConstructedType
    {
        public string id;
        private static char nextId = 'a';

        public TypeVar()
        {
            id = nextId.ToString();
            nextId++;
        }

        private TypeVar(string id)
        {
            this.id = id;
        }

        public override bool Equals(object obj)
        {
            TypeVar other = obj as TypeVar;

            if (other != null)
            {
                return this.id == other.id;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return "'"+id;
        }

        public TypeVar Clone()
        {
            return new TypeVar("this.id");
        }

        public override ConstructedType accept(TypeSubstitution typeSub)
        {
            return typeSub.Substitute(this);
        }

    }
}
