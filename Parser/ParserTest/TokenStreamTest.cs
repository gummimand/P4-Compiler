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
    class TokenStreamTest
    {

        [Test]
        public void Peek_isEmpty_returnsEndToken()
        {
            var ts = new TokenStream(new List<Token>());

            var token = ts.peek();

            Assert.AreEqual("end", token.type);
        }

        [Test]
        public void Peek_hasToken_returnsToken()
        {
            var ts = new TokenStream(new List<Token>() { new Token("content","type")});

            var token = ts.peek();

            Assert.AreEqual("type", token.type);
        }

        [Test]
        public void Peek_hasToken_doesNotRemoveToken()
        {
            var ts = new TokenStream(new List<Token>() { new Token("content", "type") });

            ts.peek();
            var token = ts.peek();

            Assert.AreEqual("type", token.type);
        }


        [Test]
        public void Next_isEmpty_returnsEndToken()
        {
            var ts = new TokenStream(new List<Token>());

            var token = ts.next();

            Assert.AreEqual("end", token.type);
        }

        [Test]
        public void Next_hasToken_returnsToken()
        {
            var ts = new TokenStream(new List<Token>() { new Token("content", "type") });

            var token = ts.next();

            Assert.AreEqual("type", token.type);
        }

        [Test]
        public void Peek_hasToken_RemovesToken()
        {
            var ts = new TokenStream(new List<Token>() { new Token("content", "type") });

            ts.next();
            var token = ts.peek();

            Assert.AreEqual("end", token.type);
        }

    }
}
