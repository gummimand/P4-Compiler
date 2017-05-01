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
    public class ScannerTest
    {

        [TestCase("i", TokenType.keyword)]
        [TestCase("", TokenType.EOF)]
        [TestCase("123", TokenType.heltal)]
        [TestCase("123.456", TokenType.tal)]
        [TestCase("id", TokenType.identifier)]
        [TestCase("sand", TokenType.boolean)]
        [TestCase("falsk", TokenType.boolean)]
        [TestCase(@"""abc""", TokenType.streng)]
        [TestCase("var", TokenType.decl)]
        [TestCase("(", TokenType.parentes)]
        [TestCase("*", TokenType.op)]
        [TestCase(";", TokenType.seperator)]
        [TestCase("|", TokenType.seperator)]
        public void Scan_ReceivesString_ReturnsCorrectTokentype(string str, TokenType type)
        {
            var S = new Scanner(str);
            var tokens = S.Scan();
            var firstToken = tokens[0];
            Assert.AreEqual(type, firstToken.Type);
        }



    }
}
