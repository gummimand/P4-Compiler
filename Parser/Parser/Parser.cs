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
            Decl varDecl = ParseDeclaration();

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

        private Decl ParseDeclaration()
        {

            Decl decl;
            var nextToken = TokenStream.peek();

            if (nextToken.content == "var")
            {
                AcceptToken();
                decl = VariableDeclaration();
            }
            else if (nextToken.content == "funktion")
            {
                AcceptToken();
                decl = FunctionDeclaration();
            }
            else
            {
                decl = new EmptyDecl();
            }
                
            return decl;
        }

        private VarDecl FunctionDeclaration()
        {
            Identifier functionName;
            Identifier argument;
            Expression functionBody;
            Expression functionExpression;

            List<Identifier> args = new List<Identifier>();
           
            functionName = new Identifier(AcceptToken(TokenType.identifier));

            AcceptToken("(");

            args.Add(new Identifier(AcceptToken(TokenType.identifier)));


            while (TokenStream.peek().content == ",")
            {
                AcceptToken();
                args.Add(new Identifier(AcceptToken(TokenType.identifier)));
            }

            AcceptToken(")");

            functionBody = ParseClauseExp();

            if (args.Count == 1)
            {
                argument = args[0];
                functionExpression = new AnonFuncExpression(argument, functionBody);
            }
            else if (args.Count == 2)
            {
                Identifier pair = new Identifier(new Token("pair", TokenType.identifier));

                LetExpression let2 = new LetExpression(args[1], new ApplicationExpression(new SecondConst(), new IdentifierExpression("pair")), functionBody);
                LetExpression let1 = new LetExpression(args[0], new ApplicationExpression(new FirstConst(), new IdentifierExpression("pair")), let2);

                functionExpression = new AnonFuncExpression(pair, let1);
            }
            else
            {
                List<IdentifierExpression> pairs = new List<IdentifierExpression>();
                for (int j = 0; j < args.Count - 1; j++)
                {
                    pairs.Add(new IdentifierExpression("pair" + j));
                }

                int i = args.Count - 2;

                functionBody = new LetExpression(args[i + 1], new ApplicationExpression(new SecondConst(), pairs[i]), functionBody);
                functionBody = new LetExpression(args[i], new ApplicationExpression(new FirstConst(), pairs[i]), functionBody);

                i--;

                while (i >= 0)
                {
                    functionBody = new LetExpression(new Identifier(new Token(pairs[i + 1].varName, TokenType.identifier)), new ApplicationExpression(new SecondConst(), pairs[i]), functionBody);
                    functionBody = new LetExpression(args[i], new ApplicationExpression(new FirstConst(), pairs[i]), functionBody);

                    i--;
                }

                functionExpression = new AnonFuncExpression(new Identifier(new Token(pairs[i + 1].varName, TokenType.identifier)), functionBody);
            }

            AcceptToken(";");

            return new VarDecl(functionName, functionExpression, ParseDeclaration());
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
            Identifier id;
            Expression exp;

            id = new Identifier(AcceptToken(TokenType.identifier));

            AcceptToken("=");

            exp = ParseExpression();

            AcceptToken(";");

            return new VarDecl(id, exp, ParseDeclaration());
        }

        private Expression ParseExpression()
        {
           
            Expression exp1 = ParseSimpleExpression();

            while (TokenStream.peek().Type != TokenType.EOF && TokenStream.peek().Type != TokenType.op && !IsExpressionEnding(TokenStream.peek()))
            {
                Expression exp2 = ParseSimpleExpression();
                exp1 = new ApplicationExpression(exp1, exp2);
            }

            while (TokenStream.peek().Type == TokenType.op)
                {
                    Expression op = GetOperationConst(TokenStream.next().content);

                    Expression exp2 = ParseExpression();

                    exp1 = new ApplicationExpression(new ApplicationExpression(op, exp1), exp2);
                }

              

            return exp1;        
        }

        private Expression GetOperationConst(string content)
        {
            switch (content)
            {
                case "+":
                    return new PlusConst();
                case "-":
                    return new MinusConst();
                case "*":
                    return new TimesConst();
                case "/":
                    return new DivideConst();
                case "^":
                    return new PotensConst();
                case "%":
                    return new ModuloConst();
                case "<":
                    return new LesserThanConst();
                case ">":
                    return new GreaterThanConst();
                case "==":
                    return new EqualConst();
                case "!=":
                    return new NotEqualConst();
                case "!":
                    return new NotConst();
                case "<=":
                    return new LesserThanOrEqualConst();
                case ">=":
                    return new GreaterThanOrEqualConst();
                case ":":
                    return new ListConst();
                case "hoved":
                    return new HeadConst();
                case "første":
                    return new FirstConst();
                case "anden":
                    return new SecondConst();
                case "hale":
                    return new TailConst();
                case "&&":
                    return new AndConst();
                case "||":
                    return new OrConst();
                default:
                    throw new ArgumentException($"Unknown operator. was {content}");
            };
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
                case "=>":
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
                    //Identifier identifier = new Identifier(TokenStream.next());
                    exp = new IdentifierExpression(TokenStream.next().content);
                }
                else
                {
                    Token valueToken =TokenStream.next();

                    ConstructedType tIn;
                    switch(valueToken.Type) {
                        case TokenType.boolean:
                            tIn = new BoolType();
                            break;
                        case TokenType.heltal:
                            tIn = new HeltalType();
                            break;
                        case TokenType.streng:
                            tIn = new StrengType();
                            break;
                        case TokenType.tal:
                            tIn = new TalType();
                            break;
                        default:
                            throw new Exception($"Unexpected type, was {valueToken.Type}");
                    }


                    exp = new ValueExpression(valueToken.content, tIn);
                }
            }

            else if (TokenStream.peek().content == "!")
            {
                Expression op = GetOperationConst(TokenStream.next().content);
                Expression exp2 = ParseSimpleExpression();
                exp2 = new ApplicationExpression(op, exp2);

                return exp2;
            }
            else if (TokenStream.peek().content == "hoved" || TokenStream.peek().content == "hale" || TokenStream.peek().content == "første" || TokenStream.peek().content == "anden")
            {
                Expression op = GetOperationConst(TokenStream.next().content);
                if (TokenStream.peek().content == "(")
                    AcceptToken();
                else
                    throw new Exception("Missing bracket: (");

                Expression exp2 = ParseExpression();

                if (TokenStream.peek().content == ")")
                    AcceptToken();
                else
                    throw new Exception("Missing ending bracket: )");

                exp2 = new ApplicationExpression(op, exp2);

                return exp2;
            }
            else if (TokenStream.peek().content == "hvis")
            {
                AcceptToken();
                exp = ifStatement();
            }
            else if (TokenStream.peek().content == "lad")
            {
                AcceptToken();
                exp = ParseLetExpression();
            }
            else if (TokenStream.peek().content == "fn")
            {
                AcceptToken();
                exp = AnonFunction();
            }
            else if (TokenStream.peek().content == "{")
            {
                AcceptToken();
                exp = ParseList();
            }
            else if (TokenStream.peek().content == "(")
            {
                AcceptToken();
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
            Expression first;

            if (TokenStream.peek().content == ")")
                throw new ArgumentException("empty pair");

            first = ParseExpression();

            if (TokenStream.peek().content == ")")
            {
                AcceptToken();
                return first;
            }
            else
            {
                AcceptToken(",");

                Expression firstPair = new ApplicationExpression(new PairConst(), first);
                Expression second = ParseTuple();

                return new ApplicationExpression(firstPair, second);
            }
            
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
                head = ParseExpression();

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
            Identifier arg;
            Expression exp;

            AcceptToken("(");

            arg = new Identifier(AcceptToken(TokenType.identifier));

            AcceptToken(")");

            AcceptToken("=>");

            exp = ParseExpression();

            return new AnonFuncExpression(arg, exp);

        }

        private LetExpression ParseLetExpression()
        { 
            Identifier id;
            Expression exp1;
            Expression exp2;

            AcceptToken(TokenType.decl);

            id = new Identifier(AcceptToken(TokenType.identifier));

            AcceptToken("=");
    
            exp1 = ParseExpression();

            AcceptToken(";");

            if (TokenStream.peek().Type == TokenType.decl)
            {
                exp2 = ParseLetExpression();
            }
            else
            {
                AcceptToken("i");

                exp2 = ParseExpression( );

                AcceptToken("slut");
            }

            return new LetExpression(id, exp1, exp2);
        }

        private IfExpression ifStatement()
        {
            Expression condition;
            Expression alt1;
            Expression alt2;

            condition = ParseExpression();

            AcceptToken("så");

            alt1 = ParseExpression();

            AcceptToken("ellers");

            alt2 = ParseExpression();

            return new IfExpression(condition, alt1, alt2);            
        }
    }
}
