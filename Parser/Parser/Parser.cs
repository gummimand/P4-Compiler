﻿using System;
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

            while(TokenStream.peek().Type == TokenType.decl)
            {
                ParseDeclarations(programNode);
            }

            if(TokenStream.peek().Type != TokenType.EOF)
            {
                ParseExpression(programNode);
            }

            return new AST(programNode);
        }

        private void ParseDeclarations(Node Parent)  //todo
        {
            var declNode = new Node("Declarations");
            Parent.AddChild(declNode);

            ParseSingleDeclaration(declNode);

            var nextToken = TokenStream.peek();
            if (nextToken.content == ";" || nextToken.Type == TokenType.EOF)
            {
                Parent.AddChild(new Leaf(TokenStream.next()));
            }
            else if (nextToken.Type == TokenType.decl)
            {

            }
            else
            {
                throw new ArgumentException("Syntax error, expected ';'");
            }

            if (TokenStream.peek().Type == TokenType.decl)
            {
                ParseDeclarations(declNode);
            }

        }

        private void ParseSingleDeclaration(Node parent)
        {
            var declNode = new Node("Declaration");
            parent.AddChild(declNode);

            var nextToken = TokenStream.peek();
            if(nextToken.content == "var")
            {
                VariableDeclaration(declNode);
            }
            else if(nextToken.content == "type")
            {
                TypeDeclaration(declNode);
            }
            else if (nextToken.content == "funktion")
            {
                FunctionDeclaration(declNode);
            }
            else
            {
                throw new ArgumentException("Syntax error for declaration");
            }
        }

        private void FunctionDeclaration(Node parent)
        {
            var funLeaf = new Leaf(TokenStream.next());
            parent.AddChild(funLeaf);

            var nextToken = TokenStream.peek();
            if (nextToken.Type == TokenType.identifier)
            {
                parent.AddChild(new Leaf(TokenStream.next()));
            }
            else
            {
                throw new ArgumentException("Syntax error, expected identifier.");
            }

            nextToken= TokenStream.peek();
            if(nextToken.content == "(")
            {
                parent.AddChild(new Leaf(TokenStream.next()));

                DeclarationRow(parent);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected '('.");
            }

            if (TokenStream.peek().content == ")")
            {
                parent.AddChild(new Leaf(TokenStream.next()));

            }
            else
            {
                throw new ArgumentException("Syntax error, expected ')'.");
            }

            ParseClause(parent);

        }

        private void ParseClause(Node parent)
        {
            var clNode = new Node("clause");
            parent.AddChild(clNode);

            var nextToken = TokenStream.peek();

            if (nextToken.content == "{")
            {
                clNode.AddChild(new Leaf(TokenStream.next()));
                ParseExpression(clNode);

                nextToken = TokenStream.peek();
                if (nextToken.content == "}")
                {
                    clNode.AddChild(new Leaf(TokenStream.next()));
                }
                else
                {
                    throw new ArgumentException("Syntax error, expected '}'.");
                }

                nextToken = TokenStream.peek();
                if (nextToken.content == "=")
                {
                    clNode.AddChild(new Leaf(TokenStream.next()));
                }
                else
                {
                    throw new ArgumentException("Syntax error, expected '='.");
                }

                ParseExpression(clNode);

                nextToken = TokenStream.peek();
                if (nextToken.content == "|")
                {
                    clNode.AddChild(new Leaf(TokenStream.next()));
                }
                else
                {
                    throw new ArgumentException("Syntax error, expected '|'.");
                }

                ParseClause(clNode);
            }
            else if (nextToken.content == "=")
            {
                clNode.AddChild(new Leaf(TokenStream.next()));
                ParseExpression(clNode);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected clause.");
            }

        }

        private void TypeDeclaration(Node parent)
        {
            var typeToken = TokenStream.next();
            var typeLeaf = new Leaf(typeToken);
            parent.AddChild(typeLeaf);

            var nextToken = TokenStream.peek();
            if (nextToken.Type == TokenType.identifier)
            {
                var idLeaf = new Leaf(TokenStream.next());
                parent.AddChild(idLeaf);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected identifier.");
            }

            nextToken = TokenStream.peek();
            if (nextToken.content == "=")
            {
                var eqLeaf = new Leaf(TokenStream.next());
                parent.AddChild(eqLeaf);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected '='.");
            }

            nextToken = TokenStream.peek();
            if (nextToken.content == "[")
            {
                var startbracketLeaf = new Leaf(TokenStream.next());
                parent.AddChild(startbracketLeaf);

                TypedDeclRow(parent);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected '['.");
            }

            nextToken = TokenStream.peek();

            if (nextToken.content == "]")
            {
                var endbracketLeaf = new Leaf(TokenStream.next());
                parent.AddChild(endbracketLeaf);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected ']'.");
            }
        }

        private void TypedDeclRow(Node parent)
        {
            var declRowNode = new Node("TypedDeclarationRow");
            parent.AddChild(declRowNode);

            var nextToken = TokenStream.peek();
            if (nextToken.Type == TokenType.datatype)
            {
                declRowNode.AddChild(new Leaf(TokenStream.next()));
            }
            else
            {
                throw new ArgumentException("Syntax error, expected datatype.");
            }

            nextToken = TokenStream.peek();
            if (nextToken.Type == TokenType.identifier)
            {
                declRowNode.AddChild(new Leaf(TokenStream.next()));
            }
            else
            {
                throw new ArgumentException("Syntax error, expected identifier.");
            }

            nextToken = TokenStream.peek();
            if (nextToken.content == ",")
            {
                declRowNode.AddChild(new Leaf(TokenStream.next()));

                TypedDeclRow(declRowNode);
            }
            
        }

        private void DeclarationRow(Node parent)
        {
            var declRowNode = new Node("DeclarationRow");
            parent.AddChild(declRowNode);

            var nextToken = TokenStream.peek();
            if(nextToken.Type == TokenType.identifier)
            {
                declRowNode.AddChild(new Leaf(TokenStream.next()));
            }
            else
            {
                throw new ArgumentException("Syntax error, expected identifier.");
            }

            nextToken = TokenStream.peek();
            if(nextToken.content ==",")
            {
                declRowNode.AddChild(new Leaf(TokenStream.next()));
                DeclarationRow(declRowNode);
            }
        }

        private void VariableDeclaration(Node parent)
        {
            var varToken = TokenStream.next();
            var varLeaf = new Leaf(varToken);
            parent.AddChild(varLeaf);

            var nextToken = TokenStream.peek();
            if(nextToken.Type == TokenType.identifier)
            {
                var idLeaf = new Leaf(TokenStream.next());
                parent.AddChild(idLeaf);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected identifier.");
            }

            nextToken = TokenStream.peek();
            if(nextToken.content == "=")
            {
                var eqLeaf = new Leaf(TokenStream.next());
                parent.AddChild(eqLeaf);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected '='.");
            }

            ParseExpression(parent);
        }

        private void ParseExpression(Node parent)
        {

            var expNode = new Node("Expression");
            parent.AddChild(expNode);

            ParseSimpleExpression(parent);

            var nextToken = TokenStream.peek();
            if(nextToken.Type == TokenType.op)
            {
                var operatorleaf = new Leaf(TokenStream.next());
                parent.AddChild(operatorleaf);
                ParseExpression(parent);
            }
            else if (nextToken.Type != TokenType.EOF && !IsExpressionEnding(nextToken) )
            {
                ParseExpression(parent);
            }            
        }

        private bool IsExpressionEnding(Token token)
        {
            bool a, b;
            switch (token.Type)
            {
                case TokenType.keyword:
                case TokenType.seperator:
                case TokenType.decl: a = true; break;
                default: a = false; break;
            }
            switch (token.content)
            {
                case ")":
                case "}":
                case "]": b = true; break;
                default: b = false; break;
            }
            return a || b;
        }

        private void ParseSimpleExpression(Node Parent)
        {
            var simExpr = new Node("SimpleExpression");
            Parent.AddChild(simExpr);

            var nextToken = TokenStream.peek();

            if (IsConst(nextToken.Type))
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

        private bool IsConst(TokenType type)
        {
            switch (type)
            {
                case TokenType.tal:
                case TokenType.heltal:
                case TokenType.boolean:
                case TokenType.streng:
                case TokenType.identifier: return true;
                default: return false;
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
