using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class AST
    {
        public ASTNode Root { get; set; }
       


        public AST(ASTNode root)
        {
            Root = root;
        }
        /*
        public static void AddSymbol(string identifier, string type, string value) {
            if (table.ContainsKey(identifier)) {
                throw new Exception("Symbolet eksistrer allerede!");
            }
            table.Add(identifier, Tuple.Create(type, value));
        }
        public static void RemoveSymbol(string key) {
            table.Remove(key);
        }
        */
        public void print()
        {
            Root.PrintPretty("",true);
        }

        public override bool Equals(object obj)
        {
            AST other = obj as AST;
            if (other != null)
                return Root.Equals(other.Root);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
