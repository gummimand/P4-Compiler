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


        //[Test]
        //public void skabelon()
        //{
        //    string code = "";

        //    var actualAST = ParseCode(code);

        //    AST expectedAST = new AST
        //    (
        //         new ProgramAST
        //         (
        //             new EmptyDecl(),
        //             new EmptyDecl(),
        //             new EmptyExpression()
        //         )
        //     );

        //    Assert.AreEqual(expectedAST, actualAST);
        //}

        [Test]
        public void Parse_ThreeElementPair_MakesAST()
        {
            string code = "(0, 1, 2)";

            var actualAST = ParseCode(code);


            // (par 0)(par 1 2)
            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new ApplicationExpression
                     (
                        new ApplicationExpression
                        (
                            new PairConst(),
                            new ValueExpression(new Value(new Token("0", TokenType.heltal)))
                        ),
                        new ApplicationExpression
                        (
                            new ApplicationExpression
                            (
                                new PairConst(),
                                new ValueExpression(new Value(new Token("1", TokenType.heltal)))
                            ),
                            new ValueExpression(new Value(new Token("2", TokenType.heltal)))
                        )
                     )
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_TwoElementPair_MakesAST()
        {
            string code = "(0, 1)";

            var actualAST = ParseCode(code);


            // par 0 1
            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new ApplicationExpression
                     (
                        new ApplicationExpression
                        (
                            new PairConst(),
                            new ValueExpression(new Value(new Token("0", TokenType.heltal)))
                        ),
                        new ValueExpression(new Value(new Token("1", TokenType.heltal)))
                     )
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }


        [Test]
        public void Parse_TwoElementList_MakesAST()
        {
            string code = "{0, 1}";

            var actualAST = ParseCode(code);


            // (list 0)(list 1 {})
            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new ApplicationExpression
                     (
                        new ApplicationExpression
                        (
                            new ListConst(),
                            new ValueExpression(new Value(new Token("0", TokenType.heltal)))
                        ),
                        new ApplicationExpression
                        (
                             new ApplicationExpression
                             (
                                new ListConst(),
                                new ValueExpression(new Value(new Token("1", TokenType.heltal)))
                             ),
                             new EmptyListExpression()
                        )
                     )
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_singleElementList_MakesAST()
        {
            string code = "{0}";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new ApplicationExpression
                     (
                        new ApplicationExpression
                        (
                            new ListConst(),
                            new ValueExpression(new Value(new Token("0", TokenType.heltal)))                        
                        ),
                        new EmptyListExpression()
                     )
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_emptylist_MakesAST()
        {
            string code = "{}";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new EmptyListExpression()
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_lad_MakesAST()
        {
            string code = "lad var x = 0; i x slut";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new LetExpression
                     (                        
                        new Identifier(new Token("x", TokenType.identifier)),
                        new ValueExpression(new Value(new Token("0", TokenType.heltal))),     
                        new IdentifierExpression(new Identifier(new Token("x", TokenType.identifier)))
                     )
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_multiplelad_MakesAST()
        {
            string code = "lad var x = 0; var y = 1; i x y slut";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new LetExpression
                     (
                        new Identifier(new Token("x", TokenType.identifier)),
                        new ValueExpression(new Value(new Token("0", TokenType.heltal))),
                        new LetExpression
                        (
                            new Identifier(new Token("y", TokenType.identifier)),
                            new ValueExpression(new Value(new Token("1", TokenType.heltal))),
                            new ApplicationExpression
                            (
                                new IdentifierExpression(new Identifier(new Token("x", TokenType.identifier))),
                                new IdentifierExpression(new Identifier(new Token("y", TokenType.identifier)))
                            )
                        )
                     )
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_Application_MakesAST()
        {
            string code = "f x";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new ApplicationExpression
                     (
                         new IdentifierExpression(new Identifier(new Token("f", TokenType.identifier))),
                         new IdentifierExpression(new Identifier(new Token("x", TokenType.identifier)))
                     )
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_SeqApplication_MakesAST()
        {
            string code = "f x y";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new ApplicationExpression
                     (
                         new ApplicationExpression
                         (
                            new IdentifierExpression(new Identifier(new Token("f", TokenType.identifier))),
                            new IdentifierExpression(new Identifier(new Token("x", TokenType.identifier)))
                         ),
                         new IdentifierExpression(new Identifier(new Token("y", TokenType.identifier)))
                     )
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_varDecl_MakesAST()
        {
            string code = "var x = 0";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new VarDecl(
                        new Identifier(new Token("x", TokenType.identifier)),
                        new ValueExpression(new Value(new Token("0", TokenType.heltal))),
                        new EmptyDecl()
                        ),
                     new EmptyExpression()
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_multipleVarDecls_MakesAST()
        {
            string code = "var x = 0; var y = 1";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new VarDecl(
                        new Identifier(new Token("x", TokenType.identifier)),
                        new ValueExpression(new Value(new Token("0", TokenType.heltal))),
                        new VarDecl(
                            new Identifier(new Token("y", TokenType.identifier)),
                            new ValueExpression(new Value(new Token("1", TokenType.heltal))),
                            new EmptyDecl()
                        )
                     ),
                     new EmptyExpression()
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_varDeclFunction_MakesAST()
        {
            string code = "funktion f(x) = 0";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new VarDecl
                     (
                        new Identifier(new Token("f", TokenType.identifier)),
                        new AnonFuncExpression
                        (
                            new Identifier(new Token("x", TokenType.identifier)), 
                            new ValueExpression(new Value(new Token("0", TokenType.heltal)))
                        ),
                        new EmptyDecl()
                     ),
                     new EmptyExpression()
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_MultipleVarDeclFunction_MakesAST()
        {
            string code = "funktion f(x) = 0; funktion g(x) = sand";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new VarDecl
                     (
                        new Identifier(new Token("f", TokenType.identifier)),
                        new AnonFuncExpression
                        (
                            new Identifier(new Token("x", TokenType.identifier)),
                            new ValueExpression(new Value(new Token("0", TokenType.heltal)))
                        ),
                        new VarDecl
                        (
                            new Identifier(new Token("g", TokenType.identifier)),
                            new AnonFuncExpression
                            (
                                new Identifier(new Token("x", TokenType.identifier)),
                                new ValueExpression(new Value(new Token("sand", TokenType.boolean)))
                            ),
                            new EmptyDecl()
                        )
                     ),
                     new EmptyExpression()
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_varDeclFunctionClauses_MakesAST()
        {
            string code = "funktion f(x){sand} = 1 | = 0";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new VarDecl
                     (
                        new Identifier(new Token("f", TokenType.identifier)),
                        new AnonFuncExpression
                        (
                            new Identifier(new Token("x", TokenType.identifier)),
                            new IfExpression
                            (
                                new ValueExpression(new Value(new Token("sand", TokenType.boolean))),
                                new ValueExpression(new Value(new Token("1", TokenType.heltal))),
                                new ValueExpression(new Value(new Token("0", TokenType.heltal)))
                            )
                        ),
                        new EmptyDecl()
                     ),
                     new EmptyExpression()
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_identifierExpression_MakesAST()
        {
            string code = "x";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new IdentifierExpression
                     (
                         new Identifier(new Token("x", TokenType.identifier))
                     )
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
        }

        [Test]
        public void Parse_valueExpressionHeltal_MakesAST()
        {
            string code = "1";

            var actualAST = ParseCode(code);

            AST expectedAST = new AST
            (
                 new ProgramAST
                 (
                     new EmptyDecl(),
                     new EmptyDecl(),
                     new ValueExpression
                     (
                         new Value(new Token("1", TokenType.heltal))
                     )
                 )
             );

            Assert.AreEqual(expectedAST, actualAST);
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
