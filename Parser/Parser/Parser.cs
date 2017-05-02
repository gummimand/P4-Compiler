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
            var programNode = new ProgramNode();
            Decl decl;

            Decl typeDecl = ParseTypeDeclaration();
            Decl varDecl = ParseSingleVarDeclaration();
            Expression exp;

            if(TokenStream.peek().Type != TokenType.EOF)
                exp = ParseExpression(programNode);
            else
                exp = new EmptyExpression();

            ProgramAST prog = new ProgramAST(typeDecl, varDecl, exp);

            //new AST(programNode);   // parsetree

            return new AST(prog);
        }

        private void AcceptToken()
        {
            TokenStream.next();
        }
 

        private Decl ParseTypeDeclaration()
        {
            return new EmptyDecl();
        }

        private Decl ParseSingleVarDeclaration()
        {
            Node declNode = new Node("dummy"); //TOD remove
            Decl decl;
            var nextToken = TokenStream.peek();

            if (nextToken.content == "var")
                decl = VariableDeclaration(declNode);
            else if (nextToken.content == "funktion")
                decl = FunctionDeclaration(declNode);
            else
                decl = new EmptyDecl();
            
            return decl;
        }


        private Decl ParseDeclarations(Node Parent)  //todo
        {
            var declNode = new Node("Declarations");
            Parent.AddChild(declNode);

            Decl decl = ParseSingleVarDeclaration(declNode);

            while(TokenStream.peek().content == ";")
            {
                Parent.AddChild(new Leaf(TokenStream.next()));

                if (TokenStream.peek().Type == TokenType.decl)
                {
                    decl = new SeqDecl(decl, ParseDeclarations(declNode));
                }

            }
           
            return decl;
        }

        private Decl ParseSingleVarDeclaration(Node parent)
        {
            var declNode = new Node("Declaration");
            Decl decl;

            var nextToken = TokenStream.peek();
            if(nextToken.content == "var")
            {
                decl = VariableDeclaration(declNode);
            }
            else if(nextToken.content == "type")
            {
                decl = TypeDeclaration(declNode);
            }
            else if (nextToken.content == "funktion")
            {
                decl = FunctionDeclaration(declNode);
            }
            else
            {
                throw new ArgumentException("Syntax error for declaration");
            }
            parent.AddChild(declNode);

            return decl;
        }

        private VarDecl FunctionDeclaration(Node parent)
        {
            AcceptToken();
            // id
            Identifier id;
            Identifier arg;
            Expression body;
            Expression exp;

            var nextToken = TokenStream.peek();
            if (nextToken.Type == TokenType.identifier)
                id = new Identifier(TokenStream.next());
            else
                throw new ArgumentException("Syntax error, expected identifier.");

            // args
            nextToken = TokenStream.peek();
            if (nextToken.content == "(")
                AcceptToken();
            else
                throw new ArgumentException("Syntax error, expected '('.");

            if (TokenStream.peek().Type == TokenType.identifier)
                arg = new Identifier(TokenStream.next());
            else
                throw new ArgumentException("Syntax error, expected identifier.");

            if (TokenStream.peek().content == ")")
                AcceptToken();
            else
                throw new ArgumentException("Syntax error, expected ')'.");


            // clause

            body = ParseClauseExp();
            exp = new AnonFuncExpression(arg, body);

            if (TokenStream.peek().content == ";")
                AcceptToken();

            VarDecl decl = new VarDecl(id, exp, ParseSingleVarDeclaration());

            return decl;
        }

        private Expression ParseClauseExp()
        {
            Expression clause;
            
            var nextToken = TokenStream.peek();
            if (nextToken.content == "{")
            {
                AcceptToken();

                Expression condition = ParseExpression(new Node("dummy"));

                nextToken = TokenStream.peek();
                if (nextToken.content == "}")
                {
                    AcceptToken();
                }
                else
                {
                    throw new ArgumentException("Syntax error, expected '}'.");
                }

                nextToken = TokenStream.peek();
                if (nextToken.content == "=")
                {
                    AcceptToken();
                }
                else
                {
                    throw new ArgumentException("Syntax error, expected '='.");
                }

                Expression alt1 = ParseExpression(new Node("dummy"));

                nextToken = TokenStream.peek();
                if (nextToken.content == "|")
                {
                    AcceptToken();
                }
                else
                {
                    throw new ArgumentException("Syntax error, expected '|'.");
                }

                Expression alt2 = ParseClauseExp();
                clause = new IfExpression(condition, alt1, alt2);
            }
            else if (nextToken.content == "=")
            {
                AcceptToken();

                clause = ParseExpression(new Node("dummy"));

            }
            else
            {
                throw new ArgumentException("Syntax error, expected clause.");
            }
            return clause;
        }

        private Clause ParseClause(Node parent)
        {
            Node clNode = new Node("clause");

            Clause clause;
            Expression condition;
            Expression exp;
            Clause altClause;

            var nextToken = TokenStream.peek();
            if (nextToken.content == "{")
            {
                clNode.AddChild(new Leaf(TokenStream.next()));
                clNode = new ClauseNode();
                condition = ParseExpression(clNode);

                nextToken = TokenStream.peek();
                if (nextToken.content == "}") {
                    clNode.AddChild(new Leaf(TokenStream.next()));
                }
                else {
                    throw new ArgumentException("Syntax error, expected '}'.");
                }

                nextToken = TokenStream.peek();
                if (nextToken.content == "=") {
                    clNode.AddChild(new Leaf(TokenStream.next()));
                }
                else {
                    throw new ArgumentException("Syntax error, expected '='.");
                }

                exp = ParseExpression(clNode);

                nextToken = TokenStream.peek();
                if (nextToken.content == "|") {
                    clNode.AddChild(new Leaf(TokenStream.next()));
                }
                else {
                    throw new ArgumentException("Syntax error, expected '|'.");
                }

                altClause = ParseClause(clNode);
                clause = new ConditionalClause(condition, exp, altClause);
            }
            else if (nextToken.content == "=")
            {
                clNode.AddChild(new Leaf(TokenStream.next()));
                clNode = new DefaultClauseNode();
                exp = ParseExpression(clNode);

                clause = new DefaultClause(exp);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected clause.");
            }

            parent.AddChild(clNode);
            return clause;

        }

        private TypeDecl TypeDeclaration(Node parent)
        {
            var typeToken = TokenStream.next();
            var typeleaf = new Leaf(typeToken);
            parent.AddChild(typeleaf);
            Identifier id;
            DatatypeLabelPair[] labels;


            var nextToken = TokenStream.peek();
            if (nextToken.Type == TokenType.identifier)
            {
                var idLeaf = new Leaf(TokenStream.next());
                parent.AddChild(idLeaf);
                id = new Identifier(nextToken);
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
                //////////////////////////////////// Insert TypeDecl object, which gets labels from return from line below.
                labels = TypedDeclRow(parent);
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

            return new TypeDecl(id, ParseTypeDeclaration() , labels);
        }

        private DatatypeLabelPair[] TypedDeclRow(Node parent)//Shouldnt be recursive.. Should make labels siblings instead of parent/child.
        {
            
            var declRowNode = new Node("TypedDeclarationRow");
            parent.AddChild(declRowNode);

            List<DatatypeLabelPair> pairs = new List<DatatypeLabelPair>();

            pairs.Add(ParseDatatypeLabelPair(declRowNode));
            
            while (TokenStream.peek().content == ",") //While loop maybe?
            {
                declRowNode.AddChild(new Leaf(TokenStream.next()));

                pairs.Add(ParseDatatypeLabelPair(declRowNode));
            }

            return pairs.ToArray();
        }

        private DatatypeLabelPair ParseDatatypeLabelPair(Node parent)
        {
            Identifier label;
            Identifier type;

            var nextToken = TokenStream.peek();
            if (nextToken.Type == TokenType.datatype)
            {
                parent.AddChild(new Leaf(TokenStream.next()));
                type = new Identifier(nextToken);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected datatype.");
            }

            nextToken = TokenStream.peek();
            if (nextToken.Type == TokenType.identifier)
            {
                parent.AddChild(new Leaf(TokenStream.next()));
                label = new Identifier(nextToken);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected identifier.");
            }

            return new DatatypeLabelPair(label, type);
        }

        private Identifier[] DeclarationRow(Node parent)
        {
            var declRowNode = new Node("DeclarationRow");
            parent.AddChild(declRowNode);
            List<Identifier> args = new List<Identifier>();


            var nextToken = TokenStream.peek();
            if(nextToken.Type == TokenType.identifier)
            {
                declRowNode.AddChild(new Leaf(TokenStream.next()));
                args.Add(new Identifier(nextToken));
            }
            else
            {
                throw new ArgumentException("Syntax error, expected identifier.");
            }

            while(TokenStream.peek().content == ",") {
                nextToken = TokenStream.next();
                declRowNode.AddChild(new Leaf(nextToken));
                args.Add(new Identifier(nextToken));

            }
            //nextToken = TokenStream.peek();
            //if(nextToken.content ==",")//Better with while? then append declarations to decl list for AST
            //{
            //    declRowNode.AddChild(new Leaf(TokenStream.next()));
            //    DeclarationRow(declRowNode);
            //}

            return args.ToArray();
        }

        private VarDecl VariableDeclaration(Node parent)
        {
            var varToken = TokenStream.next();
            var varLeaf = new Leaf(varToken);
            parent.AddChild(varLeaf);
            Identifier id;
            

            var nextToken = TokenStream.peek();
            if(nextToken.Type == TokenType.identifier)
            {
                id = new Identifier(nextToken);
                var idLeaf = new Leaf(TokenStream.next());
                parent.AddChild(idLeaf);
            }
            else
            {
                throw new ArgumentException("Syntax error, expected identifier.");
            }

            nextToken = TokenStream.peek();
            
            if (nextToken.content == "=") {
                var eqLeaf = new Leaf(TokenStream.next());
                parent.AddChild(eqLeaf);
            }
            else {
                throw new ArgumentException("Syntax error, expected '='.");
            }

            Expression exp = ParseExpression(parent);

            if (TokenStream.peek().content == ";")
                AcceptToken();

            return new VarDecl(id, exp, ParseSingleVarDeclaration());
        }

        private Expression ParseExpression(Node parent)
        {

            var expNode = new ExpressionNode();
            parent.AddChild(expNode);

            Expression exp = ParseSimpleExpression(expNode);

            while(TokenStream.peek().Type != TokenType.EOF && !IsExpressionEnding(TokenStream.peek()))
            {
                Expression exp2 = ParseSimpleExpression(new Node("dummy"));
                exp = new ApplicationExpression(exp, exp2);
            }


            //var nextToken = TokenStream.peek();
            //if(nextToken.Type == TokenType.op)
            //{
            //    var operatorleaf = new Leaf(TokenStream.next());
            //    expNode.AddChild(operatorleaf);

            //    Operator op = new Operator(nextToken);

            //    exp = new OperatorExpression(op, exp, ParseExpression(expNode));
            //}
            //else if (nextToken.Type != TokenType.EOF && !IsExpressionEnding(nextToken) )
            //{


            //    exp = new ApplicationExpression(exp, ParseExpression(expNode)); 
            //}

            return exp;        
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

        private Expression ParseSimpleExpression(Node Parent)
        {
            Node simExpr = new Node("SimpleExpression");
            Expression exp;
            
            var nextToken = TokenStream.peek();

            if (IsConst(nextToken.Type) || nextToken.Type == TokenType.identifier)
            {
                simExpr = new ConstantExpressionNode();
                var leaf = new Leaf(TokenStream.next());
                simExpr.AddChild(leaf);

                //AST:
                if(nextToken.Type == TokenType.identifier) {
                    Identifier identifier = new Identifier(nextToken);
                    exp = new IdentifierExpression(identifier);
                }
                else {
                    Value value = new Value(nextToken);
                    exp = new ValueExpression(value);
                }
            }
            else if (nextToken.content == "hvis")
            {
                simExpr = new IfExpressionNode();
                exp = ifStatement(simExpr);
            }
            else if (nextToken.content == "lad")
            {
                simExpr = new LetExpressionNode();
                exp = letEnvironment(simExpr);
            }
            else if (nextToken.content == "fn")
            {
                simExpr = new AnonFuncNode();
                exp = AnonFunction(simExpr);
            }
            else if (nextToken.content == "[")
            {
                simExpr = new StructureExpressionNode();
                exp = Structure(simExpr);
            }
            else if (nextToken.content == "{")
            {
                simExpr = new ListExpressionNode();
                exp = ParseList(simExpr);
            }
            else if (nextToken.content == "(")
            {
                simExpr = new TupleExpressionNode();
                exp = ParseTuple(simExpr);
            }
            else
            {
                throw new ArgumentException($"Can't parse expression starting with {nextToken}.");
            }

            Parent.AddChild(simExpr);
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

        private TupleExpression ParseTuple(Node parent)
        {
            var nextToken = TokenStream.peek();
            var StartBracketLeaf = new Leaf(TokenStream.next());
            parent.AddChild(StartBracketLeaf);

            TupleExpression tupleexp;
            Expression[] exps;


            exps = ExprRow(parent);

            nextToken = TokenStream.peek();

            if (nextToken.content == ")") {
                var EndBracketLeaf = new Leaf(TokenStream.next());
                parent.AddChild(EndBracketLeaf);
            }
            else {
                throw new ArgumentException($"Expexted ')', was {nextToken.content}");
            }

            tupleexp = new TupleExpression(exps);
            return tupleexp;
        }

        private ListExpression ParseList(Node parent)
        {
            var nextToken = TokenStream.peek();
            var StartBracketLeaf = new Leaf(TokenStream.next());
            parent.AddChild(StartBracketLeaf);

            ListExpression listexp;
            Expression[] exps;


            exps = ExprRow(parent);

            nextToken = TokenStream.peek();

            if (nextToken.content == "}") {
                var EndBracketLeaf = new Leaf(TokenStream.next());
                parent.AddChild(EndBracketLeaf);
            }
            else {
                throw new ArgumentException($"Expexted '}}', was {nextToken.content}");
            }

            listexp = new ListExpression(exps);
            return listexp;
        }

        private StructureExpression Structure(Node parent)
        {
            var nextToken = TokenStream.peek();
            var StartBracketLeaf = new Leaf(TokenStream.next());
            parent.AddChild(StartBracketLeaf);

            StructureExpression structureexp;
            Expression[] exps;


            exps = ExprRow(parent);

            nextToken = TokenStream.peek();

            if (nextToken.content == "]") {
                var EndBracketLeaf = new Leaf(TokenStream.next());
                parent.AddChild(EndBracketLeaf);
            }
            else {
                throw new ArgumentException($"Expexted ']', was {nextToken.content}");
            }

            structureexp = new StructureExpression(exps);
            return structureexp;
        }

        private Expression[] ExprRow(Node parent)
        {
            var exprRow = new Node("EpressionRow");
            parent.AddChild(exprRow);

            List<Expression> exps = new List<Expression>();

            exps.Add(ParseExpression(exprRow));


            while (TokenStream.peek().content == ",") {
                var commaLeaf = new Leaf(TokenStream.next());
                exprRow.AddChild(commaLeaf);
                exps.Add(ParseExpression(exprRow));
            }

            return exps.ToArray();

        }

        private AnonFuncExpression AnonFunction(Node parent)//TODO
        {
            var nextToken = TokenStream.peek();
            var fnLeaf = new Leaf(TokenStream.next());
            parent.AddChild(fnLeaf);

            AnonFuncExpression anonexp;
            Expression exp;
            Identifier arg;

            if (TokenStream.peek().content == "(")
            {
                parent.AddChild(new Leaf(TokenStream.next()));
                
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
                parent.AddChild(new Leaf(TokenStream.next()));
            }
            else
            {
                throw new ArgumentException($"Expected ')', but was {TokenStream.peek().content}");
            }
            if (TokenStream.peek().content == "=>")
            {
                parent.AddChild(new Leaf(TokenStream.next()));
                exp = ParseExpression(parent);
            }
            else
            {
                throw new ArgumentException($"Expected '=>', but was {TokenStream.peek().content}");
            }


            anonexp = new AnonFuncExpression(arg, exp);
            return anonexp;

        }

        private LetExpression letEnvironment(Node parent)
        {
            
            AcceptToken();

            LetExpression letexp;
            Identifier id;
            Expression exp1;
            Expression exp2;

            if (TokenStream.peek().Type== TokenType.decl)
            {
                AcceptToken();
            }
            else
            {
                throw new ArgumentException($"Expected 'var', but was {TokenStream.peek().content}");
            }

            if (TokenStream.peek().Type == TokenType.identifier)
            {
                id = new Identifier(TokenStream.next());
            }
            else
            {
                throw new ArgumentException($"Expected identifier, but was {TokenStream.peek().content}");
            }

            if (TokenStream.peek().content == "=")
            {
                AcceptToken();
            }
            else
            {
                throw new ArgumentException($"Expected '=', but was {TokenStream.peek().content}");
            }

            exp1 = ParseExpression(new Node("dummy"));

            if(TokenStream.peek().content == "i")
            {
                var iLeaf = new Leaf(TokenStream.next());
                parent.AddChild(iLeaf);

                exp2 = ParseExpression(parent);
            }
            else
            {
                throw new ArgumentException($"Expexted keyword 'i', was {TokenStream.peek().content}");
            }


            if (TokenStream.peek().content == "slut") {
                var slutLeaf = new Leaf(TokenStream.next());
                parent.AddChild(slutLeaf);
            }
            else {
                throw new ArgumentException($"Expexted keyword 'slut', was {TokenStream.peek().content}");
            }

            letexp = new LetExpression(id, exp1, exp2);
            return letexp;
        }

        private IfExpression ifStatement(Node parent)
        {
            var nextToken = TokenStream.peek();
            var hvisLeaf = new Leaf(TokenStream.next());
            parent.AddChild(hvisLeaf);

            IfExpression ifexp;
            Expression condition;
            Expression alt1;
            Expression alt2;



            condition = ParseExpression(parent);

            nextToken = TokenStream.peek();
            if (nextToken.content == "så")
            {
                var saaLeaf = new Leaf(TokenStream.next());
                parent.AddChild(saaLeaf);
                alt1 = ParseExpression(parent);
            }
            else
            {
                throw new ArgumentException($"Expexted keyword 'så', was {nextToken.content}");
            }


            nextToken = TokenStream.peek();
            if (nextToken.content == "ellers")
            {
                var ellersLeaf = new Leaf(TokenStream.next());
                parent.AddChild(ellersLeaf);

                alt2 = ParseExpression(parent);
            }
            else
            {
                throw new ArgumentException($"Expexted keyword 'ellers', was {nextToken.content}");
            }

            ifexp = new IfExpression(condition, alt1, alt2);
            return ifexp;
            
        }
        

    }
}
