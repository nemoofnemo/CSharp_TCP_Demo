using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Communication
{
    class MainClass
    {
        static void Main(string[] args)
        {
            ServerModule mod = new ServerModule();
            mod.run();
        }
    }
}
