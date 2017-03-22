using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner scanner = new Scanner();

            scanner.Scan();
            scanner.printTokens();
        }
    }
}
