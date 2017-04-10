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
    public class CharacterStreamTest
    {
        [Test]
        public void CharacterStream_ConstructionEmptyString_MakesObject()
        {
            var cs = new CharacterStream("");
            Assert.IsNotNull(cs);
        }

        [Test]
        public void CharaterStream_ConstructionEmptyString_MakesCharList()
        {
            var cs = new CharacterStream("");
            Assert.IsNotNull(cs.Chars);
        }

        [Test]
        public void CharaterStream_ConstructionNonEmptyString_CharListIsNotEmpty()
        {
            var cs = new CharacterStream("a");
            Assert.IsNotEmpty(cs.Chars);
        }

        [Test]
        public void Peek_emptystring_returnsNullChar()
        {
            var cs = new CharacterStream("");

            var actual = cs.Peek();

            Assert.AreEqual('\0', actual);
        }

        [Test]
        public void Peek_singleChar_returnsChar()
        {
            var cs = new CharacterStream("a");

            var actual = cs.Peek();

            Assert.AreEqual('a', actual);
        }

        [Test]
        public void Peek_singleChar_charStaysInStream()
        {
            var cs = new CharacterStream("a");

            var actual = cs.Peek();

            Assert.IsNotEmpty(cs.Chars);
        }

        [Test]
        public void GetNextChar_emptystring_returnsNullChar()
        {
            var cs = new CharacterStream("");

            var actual = cs.GetNextChar();

            Assert.AreEqual('\0', actual);
        }

        [Test]
        public void GetNextChar_singleChar_returnsChar()
        {
            var cs = new CharacterStream("a");

            var actual = cs.GetNextChar();

            Assert.AreEqual('a', actual);
        }

        [Test]
        public void GetNextChar_singleChar_charRemovedFromStream()
        {
            var cs = new CharacterStream("a");

            cs.GetNextChar();

            Assert.IsEmpty(cs.Chars);
        }

        [Test]
        public void Advance_singleChar_charRemovedFromStream()
        {
            var cs = new CharacterStream("a");

            cs.Advance();

            Assert.IsEmpty(cs.Chars);
        }

        [Test]
        public void EOF_singleChar_returnsFalse()
        {
            var cs = new CharacterStream("a");

            var actual = cs.EOF();

            Assert.IsFalse(actual);
        }

        [Test]
        public void EOF_emptystring_returnsTrue()
        {
            var cs = new CharacterStream("");

            var actual = cs.EOF();

            Assert.IsTrue(actual);
        }



    }
}
