using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Class1
    {
        delegate TOut ParamsFunc<TIn, TIn, TOut>(params TIn[] args);

        Dictionary<string, ParamsFunc<string, decimal>> Commands = new Dictionary<string, ParamsFunc<string,decimal>>();

        public void quit()
        {
            Console.WriteLine("close");
        }

        public void activate(string produkt)
        {
            Console.WriteLine(produkt + " er aktiveret" );
        }

        public void AddCredits(string bruger, decimal mængde)
        {
            Console.WriteLine(bruger + " har fået " + mængde + " på sin konto");
        }

        public void addmethods()
        {
            Commands.Add(":quit", quit);
        }


    }
}
