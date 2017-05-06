using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class HeltalType : BasicType
    {
        string testvalue;

        public HeltalType(string i)
        {
            testvalue = i;
        }

        public override string ToString()
        {
            return testvalue;
        }
    }
}
