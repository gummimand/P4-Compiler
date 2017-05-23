using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Parserproject;

namespace ParserTest
{
    [TestFixture]
    public class TypeCloserTest
    {

        [Test]
        public void FTV_singleTypevar_returns1elementList()
        {
            ConstructedType t = new TypeVar();
            TypeCloser tc = new TypeCloser();

            List<TypeVar> actual = tc.FTV(t);

            Assert.AreEqual(1, actual.Count);
        }
    }
}
