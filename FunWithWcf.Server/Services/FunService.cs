using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using FunWithWcf.Contracts;

namespace FunWithWcf.Server.Services
{
    [ServiceBehavior(
 ConcurrencyMode = ConcurrencyMode.Single,
 InstanceContextMode = InstanceContextMode.Single)]
    public class FunService : IFunService
    {
        private readonly Dictionary<string, IFunServiceCallback> callbacks = new Dictionary<string, IFunServiceCallback>();

        public void Write(string user, string message)
        {
            Console.WriteLine(user + ": " + message);

            var deadClients = new List<string>();
            foreach (var callback in callbacks)
            {
                try
                {
                    if (!callback.Key.Equals(user, StringComparison.InvariantCultureIgnoreCase))
                    {
                        callback.Value.Update(user, message);
                    }
                }
                catch (Exception)
                {
                    deadClients.Add(callback.Key);
                }
            }


            foreach (var deadClient in deadClients)
            {
                Console.WriteLine("Removing client: " + deadClient);
                this.callbacks.Remove(deadClient);
            }
        }


        public void Connect(string user)
        {
            Console.WriteLine(user + " connected");
            var callback = OperationContext.Current.GetCallbackChannel<IFunServiceCallback>();
            callbacks[user] = callback;
        }
    }
}
