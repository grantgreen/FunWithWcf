using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;

namespace FunWithWcf.Contracts.Utils
{
    public class WcfHost
    {
        private ServiceHost host = null;

        public void CreateHost(Uri address, object instance, Type interfaceType)
        {
            this.host = new ServiceHost(instance, address);
            this.host.Faulted += new EventHandler(host_Faulted);
            Binding httpBinding = ConfigurationFactory.CreateDualBinding();
            this.host.AddServiceEndpoint(interfaceType, httpBinding, address);

            //ServiceMetadataBehavior metadataBehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            //if (metadataBehavior == null)
            //{
            //    metadataBehavior = new ServiceMetadataBehavior();
            //    metadataBehavior.HttpGetEnabled = true;
            //    metadataBehavior.HttpGetUrl = new Uri(address + "mex/");
            //    host.Description.Behaviors.Add(metadataBehavior);
            //}
            //else
            //{
            //    metadataBehavior.HttpGetEnabled = true;
            //    metadataBehavior.HttpGetUrl = new Uri(address + "mex/");
            //}
            var debugBehavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (debugBehavior == null)
            {

                debugBehavior = new ServiceDebugBehavior {IncludeExceptionDetailInFaults = true};
                host.Description.Behaviors.Add(debugBehavior);
            }
            else
            {
                debugBehavior.IncludeExceptionDetailInFaults = true;
            }

            //host.AddServiceEndpoint(typeof(IMetadataExchange), httpBinding, new Uri(address + "mex/"));
        }


        void host_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Host faulted");
        }

        public void OpenHost()
        {
            if (host != null)
            {
                try
                {
                    this.host.Open();
                    Console.WriteLine("Host opened...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Opening host failed", ex);
                }
            }
        }

        public void CloseHost()
        {
            if (this.host != null)
            {
                try
                {
                    this.host.Close();
                    Console.WriteLine("Host closed...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Closing host failed", ex);
                }
            }
        }
    }

}
