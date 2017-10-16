using CIPLibrary.SCC013Reference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CIPLibrary.Classes.SCC013
{
    public static class Extensions
    {
        public static string ToXml(this object obj)
        {
            XmlSerializer s = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                s.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static T FromXml<T>(this string data)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(data))
            {
                object obj = s.Deserialize(reader);
                return (T)obj;
            }
        }
    }

    public class ASCC013Integrator
    {
        public WSCC013Response WSCC013(WSCC013Request Request)
        {
            WSCC013Request re = new WSCC013Request();
            try
            {
//                LogHelper.AddLog("Preenchendo os dados");

//                LogHelper.AddLog("1");
                re.CabecalhoReq = new CabecalhoReqComplexType();
//                LogHelper.AddLog("2");
                re.CabecalhoReq.DtHrEmis = DateTime.Now;
//                LogHelper.AddLog("2.1");
                re.CabecalhoReq.DtRef = DateTime.Now;
//                LogHelper.AddLog("3");
                re.CabecalhoReq.ISPBDestinatario = Request.CabecalhoReq.ISPBDestinatario;
//                LogHelper.AddLog("4");
                re.CabecalhoReq.ISPBEmissor = Request.CabecalhoReq.ISPBEmissor;
//                LogHelper.AddLog("5");
                re.CabecalhoReq.NumCtrlEmis = Request.CabecalhoReq.NumCtrlEmis;
//                LogHelper.AddLog("6");
                re.CabecalhoReq.DomSist = Request.CabecalhoReq.DomSist;
//                LogHelper.AddLog("7");
                re.CabecalhoReq.NUOp = Request.CabecalhoReq.NUOp;
//                LogHelper.AddLog("8");
                re.Corpo = new WSCC013CorpoComplexType();
//                LogHelper.AddLog("9");
                re.Corpo.ASCC013 = new Grupo_ASCC013_MargServdrComplexType[1];
//                LogHelper.AddLog("10");
                re.Corpo.ASCC013[0] = new Grupo_ASCC013_MargServdrComplexType();
//                LogHelper.AddLog("11");
                re.Corpo.ASCC013[0].IdentdPartAdmdo = new CNPJBase_CodErro { Value = Request.Corpo.ASCC013[0].IdentdPartAdmdo.Value }; // FIXO
//                LogHelper.AddLog("12");
                re.Corpo.ASCC013[0].CNPJBaseEnte = new CNPJBase_CodErro() { Value = Request.Corpo.ASCC013[0].CNPJBaseEnte.Value }; // FIXO
//                LogHelper.AddLog("13");
                re.Corpo.ASCC013[0].NumConsigrioEnte = new NumConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC013[0].NumConsigrioEnte.Value };
//                LogHelper.AddLog("14");
                re.Corpo.ASCC013[0].NumDigtConsigrioEnte = new NumDigtConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC013[0].NumDigtConsigrioEnte.Value };
//                LogHelper.AddLog("15");
                re.Corpo.ASCC013[0].NumCtrlConsigrio = new ControleIF_CodErro() { Value = Request.Corpo.ASCC013[0].NumCtrlConsigrio.Value };
//                LogHelper.AddLog("16");
                re.Corpo.ASCC013[0].NumCPFServdr = new CPF_CodErro() { Value = Request.Corpo.ASCC013[0].NumCPFServdr.Value };
//                LogHelper.AddLog("17");
                re.Corpo.ASCC013[0].IdentcServdr = new IdentcServdr_CodErro() { Value = Request.Corpo.ASCC013[0].IdentcServdr.Value };
//                LogHelper.AddLog("18");

                //re.Corpo.ASCC013[0].NumCPFServdr = new ServiceReference1.CPF_CodErro() { Value = "03690042488" };
                //re.Corpo.ASCC013[0].IdentcServdr = new ServiceReference1.IdentcServdr_CodErro() { Value = "105764" };

                re.Corpo.ASCC013[0].IdentcOrgao = new IdentcOrgao_CodErro() { Value = Request.Corpo.ASCC013[0].IdentcOrgao.Value };
//                LogHelper.AddLog("19");
                re.Corpo.ASCC013[0].CNPJOrgaoPagdrServdr = new CNPJ_CodErro() { Value = Request.Corpo.ASCC013[0].CNPJOrgaoPagdrServdr.Value };
//                LogHelper.AddLog("20");
                re.Corpo.ASCC013[0].IdentcEsp = new IdentcEsp_CodErro() { Value = Request.Corpo.ASCC013[0].IdentcEsp.Value };
//                LogHelper.AddLog("21");
            }
            catch(Exception erro)
            {
                LogHelper.AddLog("Erro ao preencher os dados " + erro.ToString());
                throw;
            }

            using (WSCC013Client client = GetCredentialingClient())
            {
                LogHelper.AddLog("Abrindo canal de comunicação");
                try
                {
                    client.Open();
                    //LogHelper.AddLog("Conexão aberta");
                    WSCC013Response retorno = client.WSCC013(re);
                    //LogHelper.AddLog("Respondeu OK: " + retorno.ToXml());
                    return retorno;
                }
                catch (Exception erro)
                {
                    LogHelper.AddLog("Erro: " + erro.ToString());
                    throw;
                }
            }
        }


        private WSCC013Client GetCredentialingClient()
        {

            //var url = @"https://www.hext.portaldoconsignado.org.br:443/ws/ASCC013/";
            //WSCC013Client client = new WSCC013Client(GetCustomBinding(), new EndpointAddress(new Uri(url), new DnsEndpointIdentity(@"scc-t004.cip-bancos.org.br"), new AddressHeaderCollection()));

            var url = @"https://www.portaldoconsignado.org.br/ws/ASCC013/";
            WSCC013Client client = new WSCC013Client(GetCustomBinding(), new EndpointAddress(new Uri(url), new DnsEndpointIdentity(@"scc-p004.cip-bancos.org.br"), new AddressHeaderCollection()));

            client.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            SetClientCredentialsSecurity(client.ClientCredentials);

            var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
            if (vs != null)
            {
                client.Endpoint.Behaviors.Remove(vs);
            }

            return client;
        }

        private System.ServiceModel.Channels.Binding GetCustomBinding()
        {
            //System.ServiceModel.Channels.AsymmetricSecurityBindingElement asbe = new AsymmetricSecurityBindingElement();
            //asbe.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12;

            //AsymmetricSecurityBindingElement asbe = (AsymmetricSecurityBindingElement)SecurityBindingElement.CreateMutualCertificateBindingElement(MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10);
            var asbe = (AsymmetricSecurityBindingElement)SecurityBindingElement.CreateMutualCertificateDuplexBindingElement();

            asbe.InitiatorTokenParameters = new System.ServiceModel.Security.Tokens.X509SecurityTokenParameters { InclusionMode = SecurityTokenInclusionMode.Once, RequireDerivedKeys = true };
            asbe.RecipientTokenParameters = new System.ServiceModel.Security.Tokens.X509SecurityTokenParameters { InclusionMode = SecurityTokenInclusionMode.Once, RequireDerivedKeys = true };
            asbe.MessageProtectionOrder = System.ServiceModel.Security.MessageProtectionOrder.SignBeforeEncrypt;
            
            asbe.EnableUnsecuredResponse = true;
            asbe.AllowSerializedSigningTokenOnReply = true;
            asbe.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10;
            asbe.IncludeTimestamp = true;
            asbe.SecurityHeaderLayout = SecurityHeaderLayout.Lax;
            asbe.DefaultAlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.TripleDesSha256; // Basic256; // Basic128Sha256; // Basic256Sha256; // Basic128Rsa15;
            asbe.ProtectTokens = true;

            asbe.RequireSignatureConfirmation = true;

            asbe.SetKeyDerivation(false);
            asbe.EndpointSupportingTokenParameters.Signed.Add(new X509SecurityTokenParameters() { InclusionMode = SecurityTokenInclusionMode.Once, RequireDerivedKeys = true, X509ReferenceStyle = X509KeyIdentifierClauseType.Any });

            CustomBinding myBinding = new CustomBinding();

            myBinding.Elements.Add(asbe);
            myBinding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8));

            HttpsTransportBindingElement httpsBindingElement = new HttpsTransportBindingElement();
            httpsBindingElement.RequireClientCertificate = true;
            myBinding.Elements.Add(httpsBindingElement);

            return myBinding;
        }

        private static void SetClientCredentialsSecurity(ClientCredentials clientCredentials)
        {
            //clientCredentials.ServiceCertificate.DefaultCertificate = new X509Certificate2(@"C:\Temp\CIP\CERTS\SERVER.cer");
            //clientCredentials.ClientCertificate.Certificate = new X509Certificate2(@"C:\Temp\CIP\CERTS\CLIENT.pfx", "s3nhac00p_Valid");

            clientCredentials.ServiceCertificate.DefaultCertificate = new X509Certificate2(@"C:\CERTS\CIP\SERVER.cer");
            
            clientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.PeerTrust;


            clientCredentials.ClientCertificate.Certificate = new X509Certificate2(@"C:\CERTS\CIP\CLIENT.pfx", "s3nhac00p");
           

        }

    }
}
