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
    public class ParserTest
    {
        private AST ParseCode(string code)
        {
            var scanner = new Scanner(code);
            var tl = scanner.Scan();
            var ts = new TokenStream(tl);
            var parser = new Parser(ts);
            return parser.Parse();
        }

        [Test]
        public void Parse_ifExpression_MakesAST()
        {
            string code = "hvis a så b ellers c";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                new ProgramAST
                (
                    new EmptyDecl(), 
                    new IfExpression
                    (
                        new IdentifierExpression(new Identifier(new Token("a", TokenType.identifier))),
                        new IdentifierExpression(new Identifier(new Token("b", TokenType.identifier))),
                        new IdentifierExpression(new Identifier(new Token("c", TokenType.identifier)))
                    )
                )
            );

            Assert.AreEqual(expectedAST, actualAST);
        }



        [Test]
        public void Parse_nestedIfExpression_MakesAST()
        {
            string code = "hvis x så a ellers hvis y så b ellers c ";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                new ProgramAST
                (
                    new EmptyDecl(),
                    new IfExpression
                    (
                        new IdentifierExpression(new Identifier(new Token("x", TokenType.identifier))),
                        new IdentifierExpression(new Identifier(new Token("a", TokenType.identifier))),
                        new IfExpression
                        (
                            new IdentifierExpression(new Identifier(new Token("y", TokenType.identifier))),
                            new IdentifierExpression(new Identifier(new Token("b", TokenType.identifier))),
                            new IdentifierExpression(new Identifier(new Token("c", TokenType.identifier)))
                        )
                    )
                )
            );

            Assert.AreEqual(expectedAST, actualAST);
        }
    }
}
