using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using FunWithWcf.Contracts;
using FunWithWcf.Contracts.Utils;

namespace FunWithWcf.Server.Services
{
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.Single)]
    public class FunService : IFunService
    {
        private readonly Dictionary<string, IFunServiceCallback> callbacks =
            new Dictionary<string, IFunServiceCallback>();

        private readonly Contracts.Utils.SynchronizedList<string> users = new SynchronizedList<string>();
        private readonly SynchronizedDictionary<object, object> config = new SynchronizedDictionary<object, object>();

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
            users.Add(user);
        }

        public void AddUser(string user)
        {
            this.users.Add(user);
        }

        public void AddConfig(object key, object value)
        {
            this.config[key] = value;
        }

        public Contracts.Utils.SynchronizedList<string> GetUsers()
        {
            try
            {
                Console.WriteLine("Returning users");
                return users;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
        }


        public SynchronizedDictionary<object, object> GetConfiguration()
        {
            return this.config;
        }
    }
}
