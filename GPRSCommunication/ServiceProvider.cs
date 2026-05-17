using System;
using System.Reflection;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Channels;

namespace LandisGyr.AMI.Network.GPRS.Common
{
    internal class ServiceProvider<T> : IDisposable where T : class
    {
        #region Loggers
       // private static readonly IGeneralLog generalLog = LogFactory.CreateGeneralLogger(typeof(ServiceProvider<T>).ToString());
        #endregion

        private string assemblyName = null;
        private string typeName = null;
        private ChannelFactory<T> channelFactory = null;
        private T service = null;

        // used for locking, especially when creating objects using reflection
        private static object syncObject = new object();

        public ServiceProvider(string serviceName)
        {

            typeName = ConfigurationManager.AppSettings.Get(string.Format("{0}ServiceType", serviceName));

            if (string.IsNullOrEmpty(typeName))
            {
                channelFactory = new ChannelFactory<T>(serviceName);
            }
            else
            {
                assemblyName = ConfigurationManager.AppSettings.Get(string.Format("{0}AssemblyName", serviceName));
            } 
        }

        public ServiceProvider(string serviceName, Binding binding, Uri uri)
        {
            typeName = ConfigurationManager.AppSettings.Get(string.Format("{0}ServiceType", serviceName));

            if (string.IsNullOrEmpty(typeName))
            {
                EndpointAddress address = new EndpointAddress(uri);
                channelFactory = new ChannelFactory<T>(binding, address); 
            }
            else
            {
                assemblyName = ConfigurationManager.AppSettings.Get(string.Format("{0}AssemblyName", serviceName));
            } 
           
        }

        public T GetService()
        {

            if (service == null)
            {
                service = default(T);
                if (channelFactory != null)
                {
                    service = channelFactory.CreateChannel();
                    IClientChannel serviceClientChannel = service as IClientChannel;
                    if (serviceClientChannel != null)
                    {
                        serviceClientChannel.Faulted += Faulted;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
                    {
                        // CreateInstance uses a static cache that is not thread-safe
                        lock (syncObject)
                        {
                            Assembly assembly = Assembly.Load(assemblyName);
                            service = (T)assembly.CreateInstance(typeName);
                        }
                    }
                }
            }

            return service;
        }

        private void Faulted(Object sender, EventArgs e)
        {
            CloseChannel();
        }

        #region IDisposable Members

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these 
            // operations, as well as in your methods that use the resource.
            if (!_disposed)
            {
                if (disposing)
                {
                    if (channelFactory != null)
                    {
                        CloseChannel();
                        // should we still dispose of the channel factory? This really just
                        // calls close on the channel factory under the covers. 
                        ((IDisposable)channelFactory).Dispose();
                    }
                }

                // Indicate that the instance has been disposed.
                channelFactory = null;
                _disposed = true;
            }
        }

        #endregion

        private void CloseChannel()
        {
            if (service != null)
            {
                IClientChannel channel = service as IClientChannel;
                if (channel != null)
                {
                    try
                    {
                        channel.Faulted -= Faulted;
                        if (channel.State == CommunicationState.Faulted)
                        {
                            // in production app use logger to record fault
                            //generalLog.Log(LOGLEVELS.Debug, "Channel state is faulted in CloseChannel");
                            channel.Abort();
                        }
                        else
                        {
                            channel.Close();
                        }
                    }
                    catch (CommunicationException exc)
                    {
                        // in production log the exception.
                       // generalLog.Log(LOGLEVELS.Error, string.Format("CommunicationException caught - Message:{0}, StackTrace{1}", exc.Message, exc.StackTrace)); 
                        channel.Abort();
                    }
                    catch (TimeoutException exc)
                    {
                        // in production log the exception
                       // generalLog.Log(LOGLEVELS.Error, string.Format("TimeoutException caught - Message:{0}, StackTrace{1}", exc.Message, exc.StackTrace)); 
                        channel.Abort();
                    }
                    catch (Exception exc)
                    {
                        // in production log the exception.
                       // generalLog.Log(LOGLEVELS.Error, string.Format("Exception caught - Message:{0}, StackTrace{1}", exc.Message, exc.StackTrace)); 
                        channel.Abort();
                    }
                    finally
                    {
                        service = null;
                    }
                }
            }
        }
    }
}
