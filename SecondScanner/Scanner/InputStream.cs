using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstScanner
{
    class InputStream
    {
        private StreamReader streamreader;

        public InputStream(string path)
        {
            streamreader = new StreamReader(path);
        }

        public char peek()
        {
            return (char)streamreader.Peek();
        }

        public void advance()
        {
            streamreader.Read();
        }

        public char add()
        {
            return (char)streamreader.Read();
        }

        public bool EOF()
        {
            return streamreader.EndOfStream;
        }
    }
}