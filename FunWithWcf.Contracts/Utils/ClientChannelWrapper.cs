using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace FunWithWcf.Contracts.Utils
{
    public class ServiceAgentLocator
    {
        public static ClientChannelWrapper<TInterface> CreateServiceInstance<TInterface>(object callbackInstance, string ipAddress) where TInterface : class
        {
            var endPoint = ConfigurationFactory.GetEndpointAddress<TInterface>(ipAddress);
            if (endPoint == null)
            {
                throw new ArgumentException("No endpoint of type: " + typeof(TInterface) + " found");
            }
            return new ClientChannelWrapper<TInterface>(callbackInstance,
              ConfigurationFactory.CreateDualBinding(),
              new EndpointAddress(endPoint));
        }
    }

    /// <summary>
    /// Wrapper class for the channelfactory. Exposes the service and the channel.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ClientChannelWrapper<T> : IDisposable where T : class
    {
        private readonly ChannelFactory<T> factory;
        private T service;
        private readonly object syncRoot = new object();

        public ClientChannelWrapper(object callbackObject, Binding binding, EndpointAddress endpointAddress)
        {
            factory = new DuplexChannelFactory<T>(callbackObject, binding, endpointAddress);
        }
        public T Service
        {
            get
            {
                if (this.service == null)
                {
                    CreateChannel();
                }
                return this.service;
            }
        }
        public IClientChannel Channel { get { return this.service as IClientChannel; } }

        void ClientChannelWrapperFaulted(object sender, EventArgs e)
        {
            Console.WriteLine("Connection to server lost");
            CloseChannel();
        }

        private void CreateChannel()
        {
            lock (syncRoot)
            {
                if (this.service == null)
                {
                    this.service = this.factory.CreateChannel();
                    this.Channel.OperationTimeout = new TimeSpan(0, 0, 0, 5);
                    ((IClientChannel)service).Faulted += new EventHandler(ClientChannelWrapperFaulted);
                }
            }
        }
        private void CloseChannel()
        {
            if (service != null)
            {
                lock (syncRoot)
                {
                    IClientChannel channel = this.service as IClientChannel;
                    if (channel != null)
                    {
                        try
                        {
                            channel.Faulted -= ClientChannelWrapperFaulted;
                            if (channel.State == CommunicationState.Faulted)
                                channel.Abort();
                            else
                                channel.Close();
                        }
                        catch (CommunicationException) { channel.Abort(); }
                        catch (TimeoutException) { channel.Abort(); }
                        catch (Exception) { channel.Abort(); throw; }
                        finally { this.service = null; }
                    }
                }
            }
        }
        #region IDisposable Members
        private bool isDisposed = false;
        public void Dispose()
        {
            if (isDisposed)
                throw new ObjectDisposedException("ClientChannelWrapper");

            try
            {
                this.CloseChannel();
            }
            finally
            {
                isDisposed = true;
            }

        }

        #endregion
    }
}
