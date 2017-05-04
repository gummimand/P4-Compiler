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
            Decl varDecl = ParseSingleVarDeclaration();

            Expression exp;

            if(TokenStream.peek().Type != TokenType.EOF)
                exp = ParseExpression();
            else
                exp = new EmptyExpression();

            ProgramAST prog = new ProgramAST(varDecl, exp);

            return new AST(prog);
        }

        private void AcceptToken()
        {
            TokenStream.next();
        }

        private Token AcceptToken(TokenType type)
        {
            Token t = TokenStream.peek();

            if (t.Type == type)
            {
                return TokenStream.next();
            }
            else
            {
                throw new Exception($"Unexpexted token type. Expected {type.ToString()}, but was {t.Type.ToString()}");
            }
        }

        private Token AcceptToken(string content)
        {
            Token t = TokenStream.peek();

            if (t.content == content)
            {
                return TokenStream.next();
            }
            else
            {
                throw new Exception($"Unexpexted token content. Expected {content}, but was {t.content}");
            }
        }

        private Decl ParseSingleVarDeclaration()
        {

            Decl decl;
            var nextToken = TokenStream.peek();

            if (nextToken.content == "var")
                decl = VariableDeclaration();
            else if (nextToken.content == "funktion")
                decl = FunctionDeclaration();
            else
                decl = new EmptyDecl();
            
            return decl;
        }

        private VarDecl FunctionDeclaration()
        {
            Identifier functionName;
            Identifier argument;
            Expression functionBody;
            Expression functionExpression;

            AcceptToken("funktion");

            functionName = new Identifier(AcceptToken(TokenType.identifier));

            AcceptToken("(");
            argument = new Identifier(AcceptToken(TokenType.identifier));
            AcceptToken(")");

            functionBody = ParseClauseExp();

            functionExpression = new AnonFuncExpression(argument, functionBody);

            // optional ';'
            if (TokenStream.peek().content == ";")
                AcceptToken();

            return new VarDecl(functionName, functionExpression, ParseSingleVarDeclaration());
        }

        private Expression ParseClauseExp()
        {
            Expression clause;
            
            var nextToken = TokenStream.peek();
            if (nextToken.content == "{")
            {
                AcceptToken();

                Expression condition = ParseExpression( );

                AcceptToken("}");

                AcceptToken("=");

                Expression alt1 = ParseExpression( );

                AcceptToken("|");

                Expression alt2 = ParseClauseExp();

                clause = new IfExpression(condition, alt1, alt2);
            }
            else if (nextToken.content == "=")
            {
                AcceptToken();

                clause = ParseExpression( );

            }
            else
            {
                throw new ArgumentException("Syntax error, expected clause.");
            }
            return clause;
        }

        private VarDecl VariableDeclaration()
        {
            // accept 'var'
            AcceptToken();
            Identifier id;
            

            var nextToken = TokenStream.peek();
            if(nextToken.Type == TokenType.identifier)
            {
                id = new Identifier(TokenStream.next());
            }
            else
            {
                throw new ArgumentException("Syntax error, expected identifier.");
            }

            nextToken = TokenStream.peek();
            
            if (nextToken.content == "=") {
                // accept '='
                AcceptToken();
            }
            else {
                throw new ArgumentException("Syntax error, expected '='.");
            }

            Expression exp = ParseExpression( );

            if (TokenStream.peek().content == ";")
                AcceptToken();

            return new VarDecl(id, exp, ParseSingleVarDeclaration());
        }

        private Expression ParseExpression()
        {
            Expression exp = ParseSimpleExpression();

            while(TokenStream.peek().Type == TokenType.op)
            {
                Expression op = GetOperationConst(TokenStream.next().content);

                Expression exp2 = ParseSimpleExpression();

                exp = new ApplicationExpression(new ApplicationExpression(op, exp),exp2);
            }

            while(TokenStream.peek().Type != TokenType.EOF && !IsExpressionEnding(TokenStream.peek()))
            {
                Expression exp2 = ParseSimpleExpression();
                exp = new ApplicationExpression(exp, exp2);
            }

            return exp;        
        }

        private Expression GetOperationConst(string content)
        {
            switch (content)
            {
                case "+":
                    return new PlusConst();
                case "-":
                    return new MinusConst();
                default:
                    throw new ArgumentException($"Unknown operator. Was {content}");
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

        private Expression ParseSimpleExpression()
        {
            Expression exp;

            if (IsConst(TokenStream.peek().Type) || TokenStream.peek().Type == TokenType.identifier)
            {
                if(TokenStream.peek().Type == TokenType.identifier) {
                    Identifier identifier = new Identifier(TokenStream.next());
                    exp = new IdentifierExpression(identifier);
                }
                else {
                    Value value = new Value(TokenStream.next());
                    exp = new ValueExpression(value);
                }
            }
            else if (TokenStream.peek().content == "hvis")
            {
                exp = ifStatement();
            }
            else if (TokenStream.peek().content == "lad")
            {
                AcceptToken();
                exp = ParseLetExpression();
            }
            else if (TokenStream.peek().content == "fn")
            {
                exp = AnonFunction();
            }
            else if (TokenStream.peek().content == "{")
            {
                AcceptToken();
                exp = ParseList();
            }
            else if (TokenStream.peek().content == "(")
            {
                exp = ParseTuple();
            }
            else
            {
                throw new ArgumentException($"Can't parse expression starting with {TokenStream.peek().content}.");
            }

            return exp;
        }

        private bool IsConst(TokenType type)
        {
            switch (type)
            {
                case TokenType.tal:
                case TokenType.heltal:
                case TokenType.boolean:
                case TokenType.streng: return true;
                default: return false;
            }
        }

        private Expression ParseTuple()
        {
            AcceptToken();
            Expression exp;

            if (TokenStream.peek().content == ")")
                throw new ArgumentException("emty pair");

            Expression head = ParseExpression( );
            exp = new ApplicationExpression(new PairConst(), head);

            if (TokenStream.peek().content == ")")
                return head;
            else
            {
                
                while (TokenStream.peek().content ==",")
                {
                    AcceptToken();
                    Expression tail = ParseExpression( );
                    if (TokenStream.peek().content == ")")
                    {
                        exp = new ApplicationExpression(exp, tail);
                    }
                    else
                    {
                        Expression nextPair = new ApplicationExpression(new PairConst(), tail);
                        exp = new ApplicationExpression(exp, new ApplicationExpression(nextPair, ParseTuple()));
                    }
     
                }
            }
            return exp;
        }

        private Expression ParseList()
        {
            if (TokenStream.peek().content == "}")
            {
                AcceptToken();
                return new EmptyListExpression();
            }
            else
            {
                Expression head;
                head = ParseExpression( );

                Expression exp = new ApplicationExpression(new ListConst(), head);

                if (TokenStream.peek().content == ",")
                {
                    AcceptToken();
                }

                return new ApplicationExpression(exp, ParseList());
            }
        }

        private AnonFuncExpression AnonFunction()
        {
            // accept 'fn'
            AcceptToken();

            AnonFuncExpression anonexp;
            Expression exp;
            Identifier arg;

            if (TokenStream.peek().content == "(")
            {
                // Accept '('
                AcceptToken();
                
            }
            else
            {
                throw new ArgumentException($"Expected '(', but was {TokenStream.peek().content}");
            }

            if (TokenStream.peek().Type == TokenType.identifier)
            {
                arg = new Identifier(TokenStream.next());
            }
            else
            {
                throw new ArgumentException($"Expected identifier, but was {TokenStream.peek().content}");
            }

            if (TokenStream.peek().content == ")")
            {
                // Accept ')'
                AcceptToken();
            }
            else
            {
                throw new ArgumentException($"Expected ')', but was {TokenStream.peek().content}");
            }
            if (TokenStream.peek().content == "=>")
            {
                // accept '=>'
                AcceptToken();
                exp = ParseExpression( );
            }
            else
            {
                throw new ArgumentException($"Expected '=>', but was {TokenStream.peek().content}");
            }


            anonexp = new AnonFuncExpression(arg, exp);
            return anonexp;

        }

        private LetExpression ParseLetExpression()
        { 
            Identifier id;
            Expression exp1;
            Expression exp2;

            if (TokenStream.peek().Type == TokenType.decl)
                AcceptToken();
            else
                throw new ArgumentException($"Expected 'var', but was {TokenStream.peek().content}");

            if (TokenStream.peek().Type == TokenType.identifier)
                id = new Identifier(TokenStream.next());
            else
                throw new ArgumentException($"Expected identifier, but was {TokenStream.peek().content}");

            if (TokenStream.peek().content == "=")
                AcceptToken();
            else
                throw new ArgumentException($"Expected '=', but was {TokenStream.peek().content}");

            exp1 = ParseExpression( );

            if (TokenStream.peek().content == ";")
                AcceptToken();
            else
                throw new ArgumentException($"Expected ';', but was {TokenStream.peek().content}");


            if (TokenStream.peek().Type == TokenType.decl)
            {
                exp2 = ParseLetExpression();
            }
            else if (TokenStream.peek().content == "i")
            {
                // accept 'i'
                AcceptToken();
                exp2 = ParseExpression( );

                if (TokenStream.peek().content == "slut")
                {
                    // Accept 'slut'
                    AcceptToken();
                }
                else
                {
                    throw new ArgumentException($"Expexted keyword 'slut', was {TokenStream.peek().content}");
                }
            }
            else
            {
                throw new ArgumentException($"Expexted keyword 'i' or 'var', was {TokenStream.peek().content}");
            }

            return new LetExpression(id, exp1, exp2);
        }

        private IfExpression ifStatement()
        {
            // accept 'if'
            AcceptToken();

            IfExpression ifexp;
            Expression condition;
            Expression alt1;
            Expression alt2;



            condition = ParseExpression( );

            if (TokenStream.peek().content == "så")
            {
                // accept 'så'
                AcceptToken();
                alt1 = ParseExpression( );
            }
            else
            {
                throw new ArgumentException($"Expexted keyword 'så', was {TokenStream.peek().content}");
            }

            if (TokenStream.peek().content == "ellers")
            {
                // accept 'ellers'
                AcceptToken();

                alt2 = ParseExpression( );
            }
            else
            {
                throw new ArgumentException($"Expexted keyword 'ellers', was {TokenStream.peek().content}");
            }

            ifexp = new IfExpression(condition, alt1, alt2);
            return ifexp;
            
        }
        

    }
}
