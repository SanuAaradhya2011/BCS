using System;
using Hunt.EPIC.Logging;


namespace LandisGyr.AMI.Network.GPRS.Common
{
    public sealed class ServiceFactory
    {
        #region Loggers
        private static readonly IGeneralLog generalLog = LogFactory.CreateGeneralLogger(typeof(LandisGyr.AMI.Network.GPRS.Common.ServiceFactory).ToString());

        #endregion

        public static void Using<ServiceT>(Action<ServiceT> action, string serviceName) where ServiceT : class
        {
            DateTime startDate = DateTime.Now;
            try
            {
                    using (ServiceProvider<ServiceT> factory = new ServiceProvider<ServiceT>(serviceName))
                    {
                        ServiceT service = factory.GetService();
                        action(service);
                    }
            }
            catch (SystemException se)
            {
                generalLog.Log(LOGLEVELS.Error, "Service Factory failed to create instance of the passed NameSpace", se);
                HandleException<ServiceT>(action, se);
            }

            //// only log service calls as "info" if they take longer than 30 milliseconds
            //double elapsedMillis = (DateTime.Now - startDate).TotalMilliseconds;
            //if (elapsedMillis < 30)
            //{
            //}

        }

        private static void HandleException<ServiceT>(Action<ServiceT> action, SystemException ce) where ServiceT : class
        {
           generalLog.Log(LOGLEVELS.Error,
                 string.Format("SystemException of type {0} caught while calling action {1} of type {2}. Message: {3}", ce.GetType().ToString(),
                 action.ToString(), typeof(ServiceT).ToString(), ce.Message), ce); 
           throw ce;
        }
    }
}
