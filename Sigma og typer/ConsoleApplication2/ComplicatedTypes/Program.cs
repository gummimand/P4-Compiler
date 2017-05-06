using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Nogle typer at arbejde med
            HeltalType et   = new HeltalType("1");
            HeltalType to   = new HeltalType("2");
            HeltalType tre  = new HeltalType("3");
            HeltalType fire = new HeltalType("4");
            HeltalType fem  = new HeltalType("5");
            HeltalType seks = new HeltalType("6");
            HeltalType syv  = new HeltalType("7");
            HeltalType otte = new HeltalType("8");
            HeltalType ni   = new HeltalType("9");

            //Bygger en liste
            ListType List = new ListType(et, to);

            //Tilføjer til listen
            List.AddToBasicList(tre);
            List.AddToBasicList(fire);
            List.AddToBasicList(fem);
            List.AddToBasicList(seks);
            List.AddToBasicList(syv);
            List.AddToBasicList(otte);
            List.AddToBasicList(ni);

            //Printer for at teste
            List.Print();
        }
    }
}
