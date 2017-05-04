using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class ListType : ConstructedType
    {
        ConstructedType Element1;
        ConstructedType Element2;

        ListType PreviousList;

        //For building a list of basic types
        public ListType(BasicType T1, BasicType T2)
        {
            Element1 = T1;
            Element2 = T2;
            PreviousList = this;
        }

        //For adding to a list of a basictype
        ListType(ListType T1,  BasicType T2)
        {
            Element1 = T1;
            Element2 = T2;
        }

        //for building a list of a complicatedType
        ListType(ConstructedType T1, ConstructedType T2)
        {
            Element1 = T1;
            Element2 = T2;
        }

        //for adding to a list of a complicatedType
        ListType(ListType T1, ConstructedType T2)
        {
            Element1 = T1;
            Element2 = T2;
        }
         
        public void AddToBasicList(BasicType T2)
        {
            ConstructedType temp = Element1; //saves element1
            Element1 = new ListType(temp, Element2); 
            Element2 = T2;
        }
    }
}
