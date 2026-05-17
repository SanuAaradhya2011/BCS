using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;

namespace LandisGyr.GSIS.CIM2ndEd.Service
{
    /// <summary>
    /// Wraps the true WCF proxy to send responses
    /// </summary>
    public class CIM2ndEditionCallbackProxyImpl : ICIM2ndEditionCallback
    {

        private OperationsClient Client
        {
            get;
            set;
        }

        public CIM2ndEditionCallbackProxyImpl()
        {
        }

        public void Open(string baseEndpointConfigurationName, string remoteAddress, SoapClientCredentialType credentialType)
        {
            // short circuit the credentials if the "use basic authentication" setting is enabled
            if (UseBasicAuthentication())
            {
                credentialType = SoapClientCredentialType.Basic;
            }

            string endpointConfigurationName = baseEndpointConfigurationName;
            if (credentialType == SoapClientCredentialType.WSOasis10)
            {
                endpointConfigurationName = string.Format("{0}Oasis", endpointConfigurationName);
            }
            else if (credentialType == SoapClientCredentialType.Basic)
            {
                // since Basic can support either http or https. We need to lookup the right binding.
                string protocolNameAppend = string.Empty;
                if (remoteAddress.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    protocolNameAppend = "SSL";
                }

                endpointConfigurationName = string.Format("{0}Basic{1}", endpointConfigurationName, protocolNameAppend);
            }

            //generalLog.Log(LOGLEVELS.Debug, string.Format("endpointConfigurationName:{0},remoteAddress:{1}", endpointConfigurationName, remoteAddress));
            Client = new OperationsClient(endpointConfigurationName, remoteAddress);
        }

        public PublishEventResponse PublishEvent(PublishEventRequest request, SoapClientCredentialType credentialType, string username, string password)
        {
            // short circuit the credentials if the "use basic authentication" setting is enabled
            if (UseBasicAuthentication())
            {
                credentialType = SoapClientCredentialType.Basic;
            }

            PublishEventResponse response = null;
            if (credentialType == SoapClientCredentialType.cabcon)
            {
                request.UserName = username;
                request.Password = password;
                response = Client.PublishEvent(request);
            }
            else if (credentialType == SoapClientCredentialType.WSOasis10)
            {
                UserNamePasswordClientCredential credentials = Client.ClientCredentials.UserName;
                credentials.UserName = username;
                credentials.Password = password;
                response = Client.PublishEvent(request);
            }
            else if (credentialType == SoapClientCredentialType.Basic)
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();

                Encoding encoding = Encoding.GetEncoding("iso-8859-1");

                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Basic " +
                    Convert.ToBase64String(encoding.GetBytes(username + ":" + password));

                using (OperationContextScope scope = new OperationContextScope(Client.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
                        httpRequestProperty;

                    // This code is here to put the URL in the operation context so that the client message inspector has it for logging. 
                    //    so please do not remove or change it. If this code is copied and the destination requires the message inspector
                    //    then copy this also. If not, then this can be removed
                    OperationContext.Current.OutgoingMessageProperties.Add("LGDestinationURL", Client.Endpoint.Address.Uri.AbsoluteUri);

                    response = Client.PublishEvent(request);
                }
            }

            return response;

        }

        public PublishEventResponse Request(RequestRequest request, SoapClientCredentialType credentialType, string username, string password)
        {
            // short circuit the credentials if the "use basic authentication" setting is enabled
            if (UseBasicAuthentication())
            {
                credentialType = SoapClientCredentialType.Basic;
            }

            PublishEventResponse response = null;
            if (credentialType == SoapClientCredentialType.cabcon)
            {
                request.UserName = username;
                request.Password = password;
                response = Client.Request(request);
            }
            else if (credentialType == SoapClientCredentialType.WSOasis10)
            {
                UserNamePasswordClientCredential credentials = Client.ClientCredentials.UserName;
                credentials.UserName = username;
                credentials.Password = password;
                response = Client.Request(request);
            }
            else if (credentialType == SoapClientCredentialType.Basic)
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();

                Encoding encoding = Encoding.GetEncoding("iso-8859-1");

                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Basic " +
                    Convert.ToBase64String(encoding.GetBytes(username + ":" + password));

                using (OperationContextScope scope = new OperationContextScope(Client.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
                        httpRequestProperty;

                    // This code is here to put the URL in the operation context so that the client message inspector has it for logging. 
                    //    so please do not remove or change it. If this code is copied and the destination requires the message inspector
                    //    then copy this also. If not, then this can be removed
                    OperationContext.Current.OutgoingMessageProperties.Add("LGDestinationURL", Client.Endpoint.Address.Uri.AbsoluteUri);


                    response = Client.Request(request);
                }
            }

            return response;
        }

        public PublishEventResponse Response(PublishEventResponse request, SoapClientCredentialType credentialType, string username, string password)
        {
            // short circuit the credentials if the "use basic authentication" setting is enabled
            if (UseBasicAuthentication())
            {
                credentialType = SoapClientCredentialType.Basic;
            }

            PublishEventResponse response = null;
            if (credentialType == SoapClientCredentialType.cabcon)
            {
                request.UserName = username;
                request.Password = password;
                response = Client.Response(request);
            }
            else if (credentialType == SoapClientCredentialType.WSOasis10)
            {
                UserNamePasswordClientCredential credentials = Client.ClientCredentials.UserName;
                credentials.UserName = username;
                credentials.Password = password;
                response = Client.Response(request);
            }
            else if (credentialType == SoapClientCredentialType.Basic)
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();

                Encoding encoding = Encoding.GetEncoding("iso-8859-1");

                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Basic " +
                    Convert.ToBase64String(encoding.GetBytes(username + ":" + password));

                using (OperationContextScope scope = new OperationContextScope(Client.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
                        httpRequestProperty;

                    // This code is here to put the URL in the operation context so that the client message inspector has it for logging. 
                    //    so please do not remove or change it. If this code is copied and the destination requires the message inspector
                    //    then copy this also. If not, then this can be removed
                    OperationContext.Current.OutgoingMessageProperties.Add("LGDestinationURL", Client.Endpoint.Address.Uri.AbsoluteUri);


                    response = Client.Response(request);
                }
            }

            return response;
        }

        public void Close()
        {
            if (State != CommunicationState.Closed)
            {
                try
                {
                    Client.Close();
                }
                catch(Exception ex)
                {
                    try
                    {
                        //generalLog.Log(LOGLEVELS.Error, string.Format("Exception closing Client: {0}, StackTrace: {1}", ex.Message, ex.StackTrace));
                    }
                    catch { } // don't re-throw as it could trigger retrying the message
                }
            }
        }

        public System.ServiceModel.CommunicationState State
        {
            get
            {
                if (Client == null) return CommunicationState.Closed;

                return Client.State;
            }
        }

        private bool UseBasicAuthentication()
        {
            bool useBasicAuth = false;
            try
            {
                useBasicAuth = true; // Convert.ToBoolean(ConfigurationManager.AppSettings["UseBasicAuthentication"]);
            }
            catch
            {
                useBasicAuth = false;
            }
            return useBasicAuth;
        }
    }
}
