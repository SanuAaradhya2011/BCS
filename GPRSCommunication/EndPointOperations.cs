using System;
using Hunt.EPIC.Logging;
using LandisGyr.AMI.Network.GPRS.Common;

namespace GPRSCommunication
{
    /// <summary>
    ///  Class to provide endpoint operations support for GPRS Adapter
    /// </summary>
    public class EndPointOperations
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(EndPointOperations).ToString());
        /// <summary>
        /// To add endpoints to GPRS adapter layer
        /// </summary>
        public static void AddEndpoints(LandisGyr.AMI.Layers.Endpoint[] newEndpoints)
        {
            try
            {
                ServiceFactory.Using<IEndpointOperations>(delegate(IEndpointOperations endpointOperations)
                {
                    endpointOperations.AddEndpoints(newEndpoints);
                }, Constants.GPRSAdapterEndPointOperations);
            }
         
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Fatal error occured while adding endpoints to GPRS Adapter.", ex);
            }
        }

        /// <summary>
        /// To update endpoints to GPRS adapter layer
        /// </summary>
        public static void UpdateEndpoints(LandisGyr.AMI.Layers.Endpoint[] endpoints)
        {
            try
            {
                ServiceFactory.Using<IEndpointOperations>(delegate(IEndpointOperations endpointOperations)
                {
                    endpointOperations.UpdateEndpoints(endpoints);

                }, Constants.GPRSAdapterEndPointOperations);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Fatal error occured while updating endpoints to GPRS Adapter.", ex);
            }
        }

        /// <summary>
        /// To remove endpoints to GPRS adapter layer
        /// </summary>
        public static void RemoveEndpoints(LandisGyr.AMI.Layers.Endpoint[] endpoints)
        {
            try
            {
                ServiceFactory.Using<IEndpointOperations>(delegate(IEndpointOperations endpointOperations)
                {
                    endpointOperations.RemoveEndpoints(endpoints);
                }, Constants.GPRSAdapterEndPointOperations);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Fatal error occured while removing endpoints to GPRS Adapter.", ex);
            }
        }

        /// <summary>
        /// To update change in endpoints status to GPRS adapter layer
        /// </summary>
        public static void EndpointStateChangeEvent(LandisGyr.AMI.Layers.Endpoint[] endpoints)
        {
            try
            {
                ServiceFactory.Using<IEndpointOperations>(delegate(IEndpointOperations endpointOperations)
                {
                    endpointOperations.EndpointStateChangeEvent(endpoints);
                }, Constants.GPRSAdapterEndPointOperations);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Fatal error occured while updating the endpoint state change to GPRS Adapter.", ex);
            }
        }

        /// <summary>
        /// To add endpoints to GPRS adapter layer
        /// </summary>
        public static void ConfigurationChangeEvent(LandisGyr.AMI.Layers.Endpoint[] endpoints)
        {
            try
            {
                ServiceFactory.Using<IEndpointOperations>(delegate(IEndpointOperations endpointOperations)
                {
                    endpointOperations.ConfigurationChangeEvent(endpoints);
                }, Constants.GPRSAdapterEndPointOperations);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Fatal error occured while updating the endpoint configuration changes to GPRS Adapter.", ex);
            }
        }

    }
}
