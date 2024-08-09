using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace injectDelayFunc
{
    public class MyService2 : IMyService2
    {
        public void MyServiceMethod2()
        {
            Console.WriteLine("Doing Something");
        }
    }
}
