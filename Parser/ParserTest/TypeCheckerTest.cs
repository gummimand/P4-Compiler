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
    public class TypeCheckerTest
    {

        [Test]
        public void CheckType_heltal_returnsType()
        {
            TypeChecker tc = new TypeChecker();
            ASTNode node = new ValueExpression("45", TokenType.heltal);

            tc.visit(node);

            ConstructedType actual = node.Type;
            ConstructedType expected = new HeltalType();

            Assert.AreEqual(expected, actual);


        }
    }
}
