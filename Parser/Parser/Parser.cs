using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class Parser
    {
        public TokenStream TokenStream;
       

        public Parser(TokenStream ts)
        {
            TokenStream = ts;
        }

        public AST Parse()
        {
            var programNode = new Node("Program");

            
            ParseDeclarations(programNode);

            ParseExpression(programNode);

            return new AST(programNode);
        }

        private void ParseDeclarations(Node Parent)  //todo
        {
            var declNode = new Node("Declarations");
            Parent.AddChild(declNode);
        }

        private void ParseExpression(Node parent)
        {
            var expNode = new Node("Expression");
            parent.AddChild(expNode);

            ParseSimpleExpression(parent);

            var operators = new List<string>() { "+", "-", "*", "/", "%", "^", "==", "<=", ">=", "!=", "<", ">", "og", "eller", "." };
            var exprEnders = new List<string>() { "så", "ellers", "i", "slut", ")", "]", "}", ";" };

            var nextToken = TokenStream.peek();
            if(operators.Contains(nextToken.content))
            {
                var operatorleaf = new Leaf(TokenStream.next());
                parent.AddChild(operatorleaf);
                ParseExpression(parent);
            }
            else if (nextToken.type != "end" && !exprEnders.Contains(nextToken.content))
            {
                ParseExpression(parent);
            }            
        }

        private void ParseSimpleExpression(Node Parent)
        {
            var simExpr = new Node("SimpleExpression");
            Parent.AddChild(simExpr);

            var nextToken = TokenStream.peek();

            var constantTypes = new List<string>() { "Identifier", "Int", "Num", "String", "Bool", "Var", "-", "!" };

            if (constantTypes.Contains(nextToken.type))
            {
                var leaf = new Leaf(TokenStream.next());
                simExpr.AddChild(leaf);
            }
            else if (nextToken.content == "hvis")
            {
                ifStatement(simExpr);
            }
            else if (nextToken.content == "lad")
            {
                letEnvironment(simExpr);
            }
            else if (nextToken.content == "fn")
            {
                AnonFunction(simExpr);
            }
            else if (nextToken.content == "[")
            {
                Structure(simExpr);
            }
            else if (nextToken.content == "{")
            {
                ParseList(simExpr);
            }
            else if (nextToken.content == "(")
            {
                ParseTuple(simExpr);
            }
            else
            {
                throw new ArgumentException("Oh shiiit, son!");
            }

        }

        private void ParseTuple(Node parent)
        {
            var nextToken = TokenStream.peek();
            var StartBracketLeaf = new Leaf(TokenStream.next());
            parent.AddChild(StartBracketLeaf);

            ExprRow(parent);

            nextToken = TokenStream.peek();
            if (nextToken.content == ")")
            {
                var EndBracketLeaf = new Leaf(TokenStream.next());
                parent.AddChild(EndBracketLeaf);
            }
            else
            {
                throw new ArgumentException($"Expexted ')', was {nextToken.content}");
            }
        }

        private void ParseList(Node parent)
        {
            var nextToken = TokenStream.peek();
            var StartBracketLeaf = new Leaf(TokenStream.next());
            parent.AddChild(StartBracketLeaf);

            ExprRow(parent);

            nextToken = TokenStream.peek();
            if (nextToken.content == "}")
            {
                var EndBracketLeaf = new Leaf(TokenStream.next());
                parent.AddChild(EndBracketLeaf);
            }
            else
            {
                throw new ArgumentException($"Expexted '}}', was {nextToken.content}");
            }
        }

        private void Structure(Node parent)
        {
            var nextToken = TokenStream.peek();
            var StartBracketLeaf = new Leaf(TokenStream.next());
            parent.AddChild(StartBracketLeaf);

            ExprRow(parent);

            nextToken = TokenStream.peek();
            if(nextToken.content == "]")
            {
                var EndBracketLeaf = new Leaf(TokenStream.next());
                parent.AddChild(EndBracketLeaf);
            }
            else
            {
                throw new ArgumentException($"Expexted ']', was {nextToken.content}");
            }
        }

        private void ExprRow(Node parent)
        {
            var exprRow = new Node("EpressionRow");
            parent.AddChild(exprRow);

            ParseExpression(exprRow);
            var nextToken = TokenStream.peek();

            while(nextToken.content == ",")
            {
                var commaLeaf = new Leaf(TokenStream.next());
                exprRow.AddChild(commaLeaf);
                ParseExpression(exprRow);
            }

        }

        private void AnonFunction(Node parent)//TODO
        {
            var nextToken = TokenStream.peek();
            var fnLeaf = new Leaf(TokenStream.next());
            parent.AddChild(fnLeaf);
        }

        private void letEnvironment(Node parent)
        {
            var nextToken = TokenStream.peek();
            var ladLeaf = new Leaf(TokenStream.next());
            parent.AddChild(ladLeaf);

            ParseDeclarations(parent);

            nextToken = TokenStream.peek();
            if(nextToken.content == "i")
            {
                var iLeaf = new Leaf(TokenStream.next());
                parent.AddChild(iLeaf);
            }
            else
            {
                throw new ArgumentException($"Expexted keyword 'i', was {nextToken.content}");
            }

            ParseExpression(parent);

            nextToken = TokenStream.peek();
            if (nextToken.content == "slut")
            {
                var slutLeaf = new Leaf(TokenStream.next());
                parent.AddChild(slutLeaf);
            }
            else
            {
                throw new ArgumentException($"Expexted keyword 'slut', was {nextToken.content}");
            }
        }

        private void ifStatement(Node parent)
        {
            var nextToken = TokenStream.peek();
            var hvisLeaf = new Leaf(TokenStream.next());
            parent.AddChild(hvisLeaf);

            ParseExpression(parent);

            nextToken = TokenStream.peek();
            if (nextToken.content == "så")
            {
                var saaLeaf = new Leaf(TokenStream.next());
                parent.AddChild(saaLeaf);
            }
            else
            {
                throw new ArgumentException($"Expexted keyword 'så', was {nextToken.content}");
            }

            ParseExpression(parent);

            nextToken = TokenStream.peek();
            if (nextToken.content == "ellers")
            {
                var ellersLeaf = new Leaf(TokenStream.next());
                parent.AddChild(ellersLeaf);
            }
            else
            {
                throw new ArgumentException($"Expexted keyword 'ellers', was {nextToken.content}");
            }

            ParseExpression(parent);
        }
        

    }
}
