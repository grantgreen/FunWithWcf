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
            var service = new FunService();
            host.CreateHost(ConfigurationFactory.GetEndpointAddress<IFunService>("127.0.0.1"), service, typeof(IFunService));
            host.OpenHost();

            var key = "";
            int index = 0;
            while (key != "q")
            {
                Thread.Sleep(1000);
                service.GetUsers();
                service.AddConfig("Key" + index, "Value" + index);
                index++;
                //service.AddUser("User" + index++);
                //key = Console.ReadLine();
            }
        }
    }
}
