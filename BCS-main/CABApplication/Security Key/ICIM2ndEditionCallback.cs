using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace LandisGyr.GSIS.CIM2ndEd.Service
{
    /// <summary>
    /// Callback interface, exists to provide injection to classes that require the callback
    /// </summary>
    public interface ICIM2ndEditionCallback
    {
        void Open(string baseEndpointConfigurationName, string remoteAddress, SoapClientCredentialType credentialType);

        PublishEventResponse PublishEvent(PublishEventRequest request, SoapClientCredentialType credentialType, string username, string password);

        PublishEventResponse Request(RequestRequest request, SoapClientCredentialType credentialType, string username, string password);

        PublishEventResponse Response(PublishEventResponse request, SoapClientCredentialType credentialType, string username, string password);

        void Close();

        CommunicationState State
        {
            get;
        }
    }
}
