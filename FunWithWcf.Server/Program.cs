using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FunWithWcf.Contracts.Utils;
using FunWithWcf.Contracts;
using System.Threading;
using FunWithWcf.Server.Services;

namespace FunWithWcf.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WcfHost();
            host.CreateHost(ConfigurationFactory.GetEndpointAddress<IFunService>("127.0.0.1"), new FunService(), typeof(IFunService));
            host.OpenHost();

            var key = "";
            while (key != "q")
            {
                Thread.Sleep(1000);
                key = Console.ReadLine();
            }
        }
    }
}
