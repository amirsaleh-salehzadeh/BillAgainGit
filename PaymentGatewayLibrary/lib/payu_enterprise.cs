using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Services;

namespace TCG.PaymentGatewayLibrary
{
    public class PayUEnterprise
    {
        public static TCG.PaymentGatewayLibrary.PayUEnterpriseService.EnterpriseAPISoapClient getClient(string username, string password, string url)
        {
            
            //do bindings to avoid app config entries
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Name = "EnterpriseAPISoapServiceSoapBinding";
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            
            EndpointAddress endpoint = new EndpointAddress(url);
            
            PayUEnterpriseService.EnterpriseAPISoapClient client = new PayUEnterpriseService.EnterpriseAPISoapClient(binding, endpoint);
            client.Endpoint.Behaviors.Add(new InspectorBehavior(new ClientInspector(new SecurityHeader(username, password))));
            //client.ClientCredentials.ClientCertificate.Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(;
            return client;
        }

        private class ClientInspector : IClientMessageInspector
        {
            public MessageHeader[] Headers { get; set; }
            public ClientInspector(params MessageHeader[] headers)
            {
                Headers = headers;
            }
            public object BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                if (Headers != null)
                {
                    for (int i = Headers.Length - 1; i >= 0; i--)
                        request.Headers.Insert(0, Headers[i]);
                }
                return request;
            }
            public void AfterReceiveReply(ref Message reply, object correlationState)
            {
            }
        }

        private class InspectorBehavior : IEndpointBehavior
        {
            public ClientInspector ClientInspector { get; set; }
            public InspectorBehavior(ClientInspector clientInspector)
            {
                ClientInspector = clientInspector;
            }
            public void Validate(ServiceEndpoint endpoint)
            { }
            public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {
            }
            public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
            { }
            public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                if (this.ClientInspector == null) throw new InvalidOperationException("Caller must supply ClientInspector.");
                clientRuntime.MessageInspectors.Add(ClientInspector);
            }
        }

        private class SecurityHeader : MessageHeader
        {
            public string SystemUser { get; set; }
            public string SystemPassword { get; set; }
            public SecurityHeader(string systemUser, string systemPassword)
            {
                SystemUser = systemUser;
                SystemPassword = systemPassword;
            }
            public override string Name
            {
                get { return "Security"; }
            }
            public override string Namespace
            {
                get { return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"; }
            }

            public override bool MustUnderstand
            {
                get { return true; }
            }

            protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
            {
                WriteHeader(writer);
            }
            private void WriteHeader(XmlDictionaryWriter writer)
            {
                writer.WriteStartElement("wsse", "UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                writer.WriteXmlnsAttribute("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
                writer.WriteStartElement("wsse", "Username", null);
                writer.WriteString(SystemUser);
                writer.WriteEndElement();//End Username 
                writer.WriteStartElement("wsse", "Password", null);
                writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
                writer.WriteString(SystemPassword);
                writer.WriteEndElement();//End Password 
                writer.WriteEndElement();//End UsernameToken
                writer.Flush();
            }
        }
    }
}
