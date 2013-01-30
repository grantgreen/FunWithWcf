using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace FunWithWcf.Contracts.Utils
{
    public class ConfigurationFactory
    {
        private static readonly int Port = 8008;
        private static readonly string AddressPostfix = "net.tcp://{0}:{1}/FunWithWcf/services/";

        public static Binding CreateBinding()
        {
            var httpBinding = new BasicHttpBinding
                                  {
                                      ReceiveTimeout = new TimeSpan(0, 0, 5, 0),
                                      SendTimeout = new TimeSpan(0, 0, 5, 0),
                                      MaxReceivedMessageSize = 2147483647,
                                      MaxBufferSize = 2147483647,
                                      MaxBufferPoolSize = 2147483647,
                                      ReaderQuotas =
                                          {
                                              MaxArrayLength = 2147483647,
                                              MaxBytesPerRead = 2147483647,
                                              MaxStringContentLength = 2147483647,
                                              MaxDepth = 2147483647
                                          }
                                  };
            return httpBinding;


        }

        public static Binding CreateDualBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.None, true)
                              {
                                  ReceiveTimeout = new TimeSpan(0, 0, 1, 0),
                                  SendTimeout = new TimeSpan(0, 0, 1, 0),
                                  OpenTimeout = new TimeSpan(0,0,0,10),
                                  MaxReceivedMessageSize = 2147483647,
                                  MaxBufferPoolSize = 2147483647,
                                  ReliableSession = new OptionalReliableSession() { InactivityTimeout = TimeSpan.FromSeconds(10), Enabled = true },
                                  ReaderQuotas =
                                      {
                                          MaxArrayLength = 2147483647,
                                          MaxBytesPerRead = 2147483647,
                                          MaxStringContentLength = 2147483647,
                                          MaxDepth = 2147483647
                                      }
                              };
            
            var s = binding.ReliableSession;
            return binding;
        }

        public static Uri GetEndpointAddress<TInterface>(string ipAddress)
        {
            if (typeof(TInterface) == typeof(IFunService))
                return new Uri(string.Format(AddressPostfix, ipAddress, Port) + "FunService/");

            throw new ArgumentException(typeof(TInterface).ToString() + " was not found");
        }
    }


}
