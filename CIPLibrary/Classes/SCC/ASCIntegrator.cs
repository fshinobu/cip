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

using CIPLibrary.SCC005Reference;
using CIPLibrary.SCC006Reference;
using CIPLibrary.SCC007Reference;
using CIPLibrary.SCC011Reference;
using CIPLibrary.SCC012Reference;
using CIPLibrary.SCC013Reference;
using CIPLibrary.SCC016Reference;
using CIPLibrary.SCC022Reference;



namespace CIPLibrary.Classes.SCC
{
    public static class Ambiente
    {
        //homologação 
        //public static string url = @"https://www.hext.portaldoconsignado.org.br:443/ws/";
        //public static string dns = @"scc-t004.cip-bancos.org.br";
        //public static string server = @"C:\Temp\CIP\CERTS\SERVER.cer";
        //public static string client = @"C:\Temp\CIP\CERTS\CLIENT.pfx";
        //public static string key = "s3nhac00p_Valid";

        //produção
        public static string url = @"https://www.portaldoconsignado.org.br/ws/";
        public static string dns = @"scc-p004.cip-bancos.org.br";
        public static string server = @"C:\CERTS\CIP\SERVER.cer";
        public static string client = @"C:\CERTS\CIP\CLIENT.pfx";
        public static string key = "s3nhac00p";


        public static System.ServiceModel.Channels.Binding GetCustomBinding()
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

        public static void SetClientCredentialsSecurity(ClientCredentials clientCredentials)
        {
            clientCredentials.ServiceCertificate.DefaultCertificate = new X509Certificate2(Ambiente.server);
            clientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.PeerTrust;
            clientCredentials.ClientCertificate.Certificate = new X509Certificate2(Ambiente.client, Ambiente.key);
        }

    }

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

    //Reserva de Averbação Financeira
    public class ASCC005Integrator
    {
        public WSCC005Response WSCC005(WSCC005Request Request)
        {
            WSCC005Request re = new WSCC005Request();
            try
            {
                re.CabecalhoReq = new SCC005Reference.CabecalhoReqComplexType();
                re.CabecalhoReq.NumCtrlEmis = Request.CabecalhoReq.NumCtrlEmis;
                re.CabecalhoReq.ISPBEmissor = Request.CabecalhoReq.ISPBEmissor;
                re.CabecalhoReq.ISPBDestinatario = Request.CabecalhoReq.ISPBDestinatario;
                re.CabecalhoReq.DtHrEmis = DateTime.Now;
                re.CabecalhoReq.DtRef = DateTime.Now;
                re.CabecalhoReq.DomSist = Request.CabecalhoReq.DomSist;
                re.CabecalhoReq.NUOp = Request.CabecalhoReq.NUOp;

                re.Corpo = new WSCC005CorpoComplexType();
                re.Corpo.ASCC005 = new SCC005Reference.Grupo_ASCC005_ConsigrioComplexType[1];
                re.Corpo.ASCC005[0] = new SCC005Reference.Grupo_ASCC005_ConsigrioComplexType();
                re.Corpo.ASCC005[0].IdentdPartAdmdo = new SCC005Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC005[0].IdentdPartAdmdo.Value };
                re.Corpo.ASCC005[0].CNPJBaseEnte = new SCC005Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC005[0].CNPJBaseEnte.Value };
                re.Corpo.ASCC005[0].NumConsigrioEnte = new SCC005Reference.NumConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC005[0].NumConsigrioEnte.Value };
                re.Corpo.ASCC005[0].NumDigtConsigrioEnte = new SCC005Reference.NumDigtConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC005[0].NumDigtConsigrioEnte.Value };
                re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc = new SCC005Reference.Grupo_ASCC005_ConsigncComplexType();
                re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.NumCtrlAvebcIF = new SCC005Reference.ControleIF_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.NumCtrlAvebcIF.Value };
                re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.NumCPFServdr = new SCC005Reference.CPF_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.NumCPFServdr.Value };
                re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.IdentcServdr = new SCC005Reference.IdentcServdr_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.IdentcServdr.Value };
                //re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.DtNascmtoServdr = new SCC005Reference.Data_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.DtNascmtoServdr.Value };
                //re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.IdentcOrgao = new SCC005Reference.IdentcOrgao_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.IdentcOrgao.Value };
                //re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.CNPJOrgaoPagdrServdr = new SCC005Reference.CNPJ_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.CNPJOrgaoPagdrServdr.Value };
                re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.IdentcEsp = new SCC005Reference.IdentcEsp_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.IdentcEsp.Value };
                re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.QtdTotParcl = new SCC005Reference.Qtd_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.QtdTotParcl.Value };
                //re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.VencPrimroParcl = new SCC005Reference.MesAno_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.VencPrimroParcl.Value };
                re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.DiaVencParcl = new SCC005Reference.Dia_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.DiaVencParcl.Value };
                re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.DtIniAvebc = new SCC005Reference.Data_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.DtIniAvebc.Value };
                re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.DtFimAvebc = new SCC005Reference.Data_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.DtFimAvebc.Value };
                re.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.VlrParcl = new SCC005Reference.Valor_CodErro() { Value = Request.Corpo.ASCC005[0].Grupo_ASCC005_Consignc.VlrParcl.Value };
            }
            catch (Exception erro)
            {
                LogHelper.AddLog("Erro ao preencher os dados " + erro.ToString());
                throw;
            }

            using (WSCC005Client client = GetCredentialingClient())
            {
                try
                {
                    client.Open();
                    WSCC005Response retorno = client.WSCC005(re);
                    return retorno;
                }
                catch (Exception erro)
                {
                    LogHelper.AddLog("Erro: " + erro.ToString());
                    throw;
                }
            }
        }


        private WSCC005Client GetCredentialingClient()
        {
            WSCC005Client client = new WSCC005Client(Ambiente.GetCustomBinding(), new EndpointAddress(new Uri(Ambiente.url + "ASCC005/"), new DnsEndpointIdentity(Ambiente.dns), new AddressHeaderCollection()));

            client.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            Ambiente.SetClientCredentialsSecurity(client.ClientCredentials);

            var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
            if (vs != null)
            {
                client.Endpoint.Behaviors.Remove(vs);
            }

            return client;
        }


    }

    //Finalização da Averbação Financeira
    public class ASCC006Integrator
    {
        public WSCC006Response WSCC006(WSCC006Request Request)
        {
            WSCC006Request re = new WSCC006Request();
            try
            {
                re.CabecalhoReq = new SCC006Reference.CabecalhoReqComplexType();
                re.CabecalhoReq.NumCtrlEmis = Request.CabecalhoReq.NumCtrlEmis;
                re.CabecalhoReq.ISPBEmissor = Request.CabecalhoReq.ISPBEmissor;
                re.CabecalhoReq.ISPBDestinatario = Request.CabecalhoReq.ISPBDestinatario;
                re.CabecalhoReq.DtHrEmis = DateTime.Now;
                re.CabecalhoReq.DtRef = DateTime.Now;
                re.CabecalhoReq.DomSist = Request.CabecalhoReq.DomSist;
                re.CabecalhoReq.NUOp = Request.CabecalhoReq.NUOp;

                re.Corpo = new WSCC006CorpoComplexType();
                re.Corpo.ASCC006 = new SCC006Reference.Grupo_ASCC006_ConsigrioComplexType[1];
                re.Corpo.ASCC006[0] = new SCC006Reference.Grupo_ASCC006_ConsigrioComplexType();
                re.Corpo.ASCC006[0].IdentdPartAdmdo = new SCC006Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC006[0].IdentdPartAdmdo.Value };
                re.Corpo.ASCC006[0].CNPJBaseEnte = new SCC006Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC006[0].CNPJBaseEnte.Value };
                re.Corpo.ASCC006[0].NumConsigrioEnte = new SCC006Reference.NumConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC006[0].NumConsigrioEnte.Value };
                //re.Corpo.ASCC006[0].NumDigtConsigrioEnte = new SCC006Reference.NumDigtConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC006[0].NumDigtConsigrioEnte.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc = new SCC006Reference.Grupo_ASCC006_ConsigncComplexType();
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.NumCPFServdr = new SCC006Reference.CPF_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.NumCPFServdr.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.NUAvebcSCC = new SCC006Reference.NU_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.NUAvebcSCC.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.NumContrtoIF = new SCC006Reference.NumContrtoIF_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.NumContrtoIF.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.IndrAlongmto = new SCC006Reference.Indr_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.IndrAlongmto.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.IndrAvebcParclFlexvl = new SCC006Reference.Indr_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.IndrAvebcParclFlexvl.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.Grupo_ASCC006_ParclFlex[0] = new SCC006Reference.Grupo_ASCC006_ParclFlexComplexType();
                //re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.Grupo_ASCC006_ParclFlex[0].MesAnoParcl = new SCC006Reference.MesAno_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.Grupo_ASCC006_ParclFlex[0].MesAnoParcl.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.TaxNomlContrto = new SCC006Reference.Taxa_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.TaxNomlContrto.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.TaxCETContrto = new SCC006Reference.Taxa_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.TaxCETContrto.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.VlrLibdConsignc = new SCC006Reference.Valor_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.VlrLibdConsignc.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.TarContrto = new SCC006Reference.Valor_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.TaxCETContrto.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.TribContrto = new SCC006Reference.Valor_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.TribContrto.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.OtrEncargoContrto = new SCC006Reference.OtrEncargoContrto_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.OtrEncargoContrto.Value };
                re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.VlrTotContrto = new SCC006Reference.Valor_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.VlrTotContrto.Value };
                //re.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.VlrParclFinlAvebc = new SCC006Reference.Valor_CodErro() { Value = Request.Corpo.ASCC006[0].Grupo_ASCC006_Consignc.VlrParclFinlAvebc.Value };



            }
            catch (Exception erro)
            {
                LogHelper.AddLog("Erro ao preencher os dados " + erro.ToString());
                throw;
            }

            using (WSCC006Client client = GetCredentialingClient())
            {
                try
                {
                    client.Open();
                    WSCC006Response retorno = client.WSCC006(re);
                    return retorno;
                }
                catch (Exception erro)
                {
                    LogHelper.AddLog("Erro: " + erro.ToString());
                    throw;
                }
            }
        }


        private WSCC006Client GetCredentialingClient()
        {
            WSCC006Client client = new WSCC006Client(Ambiente.GetCustomBinding(), new EndpointAddress(new Uri(Ambiente.url+ "ASCC006/"), new DnsEndpointIdentity(Ambiente.dns), new AddressHeaderCollection()));

            client.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            Ambiente.SetClientCredentialsSecurity(client.ClientCredentials);

            var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
            if (vs != null)
            {
                client.Endpoint.Behaviors.Remove(vs);
            }

            return client;
        }


    }

    //Desaverbação Financeira
    public class ASCC007Integrator
    {
        public WSCC007Response WSCC007(WSCC007Request Request)
        {
            WSCC007Request re = new WSCC007Request();
            try
            {
                re.CabecalhoReq = new SCC007Reference.CabecalhoReqComplexType();
                re.CabecalhoReq.NumCtrlEmis = Request.CabecalhoReq.NumCtrlEmis;
                re.CabecalhoReq.ISPBEmissor = Request.CabecalhoReq.ISPBEmissor;
                re.CabecalhoReq.ISPBDestinatario = Request.CabecalhoReq.ISPBDestinatario;
                re.CabecalhoReq.DtHrEmis = DateTime.Now;
                re.CabecalhoReq.DtRef = DateTime.Now;
                re.CabecalhoReq.DomSist = Request.CabecalhoReq.DomSist;
                re.CabecalhoReq.NUOp = Request.CabecalhoReq.NUOp;

                re.Corpo = new WSCC007CorpoComplexType();
                re.Corpo.ASCC007 = new SCC007Reference.Grupo_ASCC007_DesavebcComplexType[1];
                re.Corpo.ASCC007[0].IdentdPartAdmdo = new SCC007Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC007[0].IdentdPartAdmdo.Value };
                re.Corpo.ASCC007[0].NumCtrlConsigrio = new SCC007Reference.ControleIF_CodErro() { Value = Request.Corpo.ASCC007[0].NumCtrlConsigrio.Value };
                re.Corpo.ASCC007[0].CNPJBaseEnte = new SCC007Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC007[0].CNPJBaseEnte.Value };
                re.Corpo.ASCC007[0].NumConsigrioEnte = new SCC007Reference.NumConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC007[0].NumConsigrioEnte.Value };
                //re.Corpo.ASCC007[0].NumDigtConsigrioEnte = new SCC007Reference.NumDigtConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC007[0].NumDigtConsigrioEnte.Value };
                re.Corpo.ASCC007[0].Grupo_ASCC007_DesavebcContrto = new SCC007Reference.Grupo_ASCC007_DesavebcContrtoComplexType();
                re.Corpo.ASCC007[0].Grupo_ASCC007_DesavebcContrto.NUAvebcSCC = new SCC007Reference.NU_CodErro() { Value = Request.Corpo.ASCC007[0].Grupo_ASCC007_DesavebcContrto.NUAvebcSCC.Value };
                re.Corpo.ASCC007[0].Grupo_ASCC007_DesavebcContrto.IdentcMotvDesavebcContrto = new SCC007Reference.IdentcMotvDesavebcContrto_CodErro() { Value = Request.Corpo.ASCC007[0].Grupo_ASCC007_DesavebcContrto.IdentcMotvDesavebcContrto.Value };
                re.Corpo.ASCC007[0].Grupo_ASCC007_DesavebcContrto.Grupo_ASCC007_ParclContrtoSCC = new SCC007Reference.Grupo_ASCC007_ParclContrtoSCCComplexType[0];
                //re.Corpo.ASCC007[0].Grupo_ASCC007_DesavebcContrto.Grupo_ASCC007_ParclContrtoSCC[0].NumParcl = new SCC007Reference.NumParcl_CodErro() { Value = Request.Corpo.ASCC007[0].Grupo_ASCC007_DesavebcContrto.Grupo_ASCC007_ParclContrtoSCC[0].NumParcl.Value };
                //re.Corpo.ASCC007[0].Grupo_ASCC007_DesavebcContrto.Grupo_ASCC007_ParclContrtoSCC[0].VencParcl = new SCC007Reference.MesAno_CodErro() { Value = Request.Corpo.ASCC007[0].Grupo_ASCC007_DesavebcContrto.Grupo_ASCC007_ParclContrtoSCC[0].VencParcl.Value };

            }
            catch (Exception erro)
            {
                LogHelper.AddLog("Erro ao preencher os dados " + erro.ToString());
                throw;
            }

            using (WSCC007Client client = GetCredentialingClient())
            {
                try
                {
                    client.Open();
                    WSCC007Response retorno = client.WSCC007(re);
                    return retorno;
                }
                catch (Exception erro)
                {
                    LogHelper.AddLog("Erro: " + erro.ToString());
                    throw;
                }
            }
        }


        private WSCC007Client GetCredentialingClient()
        {
            WSCC007Client client = new WSCC007Client(Ambiente.GetCustomBinding(), new EndpointAddress(new Uri(Ambiente.url + "ASCC007/"), new DnsEndpointIdentity(Ambiente.dns), new AddressHeaderCollection()));

            client.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            Ambiente.SetClientCredentialsSecurity(client.ClientCredentials);

            var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
            if (vs != null)
            {
                client.Endpoint.Behaviors.Remove(vs);
            }

            return client;
        }


    }
    
    //Averbação Consignatário não Financeiro
    public class ASCC011Integrator
    {
        public WSCC011Response WSCC011(WSCC011Request Request)
        {
            WSCC011Request re = new WSCC011Request();
            try
            {
                re.CabecalhoReq = new SCC011Reference.CabecalhoReqComplexType();
                re.CabecalhoReq.DtHrEmis = DateTime.Now;
                re.CabecalhoReq.DtRef = DateTime.Now;
                re.CabecalhoReq.ISPBDestinatario = Request.CabecalhoReq.ISPBDestinatario;
                re.CabecalhoReq.ISPBEmissor = Request.CabecalhoReq.ISPBEmissor;
                re.CabecalhoReq.NumCtrlEmis = Request.CabecalhoReq.NumCtrlEmis;
                re.CabecalhoReq.DomSist = Request.CabecalhoReq.DomSist;
                re.CabecalhoReq.NUOp = Request.CabecalhoReq.NUOp;
                re.Corpo = new WSCC011CorpoComplexType();
                re.Corpo.ASCC011 = new Grupo_ASCC011_ConsigrioComplexType[1];
                re.Corpo.ASCC011[0].IdentdPartAdmdo = new SCC011Reference.CNPJBase_CodErro { Value = Request.Corpo.ASCC011[0].IdentdPartAdmdo.Value };
                re.Corpo.ASCC011[0].NumCtrlConsigrio = new SCC011Reference.ControleIF_CodErro() { Value = Request.Corpo.ASCC011[0].NumCtrlConsigrio.Value };
                re.Corpo.ASCC011[0].CNPJBaseEnte = new SCC011Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC011[0].CNPJBaseEnte.Value }; 
                re.Corpo.ASCC011[0].NumConsigrioEnte = new SCC011Reference.NumConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC011[0].NumConsigrioEnte.Value };
                //re.Corpo.ASCC011[0].NumDigtConsigrioEnte = new SCC011Reference.NumDigtConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC011[0].NumDigtConsigrioEnte.Value };
                re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc = new SCC011Reference.Grupo_ASCC011_ConsigncComplexType();
                re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.NumCPFServdr = new SCC011Reference.CPF_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.NumCPFServdr.Value };
                re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.IdentcServdr = new SCC011Reference.IdentcServdr_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.IdentcServdr.Value };
                //re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.IdentcOrgao = new SCC011Reference.IdentcOrgao_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.IdentcOrgao.Value };
                //re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.CNPJBaseOrgaoPagdr = new SCC011Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.CNPJBaseOrgaoPagdr.Value };
                re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.IdentcEsp = new SCC011Reference.IdentcEsp_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.IdentcEsp.Value };
                re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.TpParcmnt = new SCC011Reference.TpParcmnt_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.TpParcmnt.Value };
                re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.QtdTotParcl = new SCC011Reference.Qtd_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.QtdTotParcl.Value };
                re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.DiaVencParcl = new SCC011Reference.Dia_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.DiaVencParcl.Value };
                re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.DtIniAvebc = new SCC011Reference.Data_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.DtIniAvebc.Value };
                //re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.DtFimAvebcDtIniAvebc = new SCC011Reference.Data_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.DtFimAvebc.Value };
                re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.VlrParcl = new SCC011Reference.Valor_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.VlrParcl.Value };
                //re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.VlrTotAvebc = new SCC011Reference.Valor_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.VlrTotAvebc.Value };
                re.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.NumOpConsigncConsigrio = new SCC011Reference.NumOpConsigncConsigrio_CodErro() { Value = Request.Corpo.ASCC011[0].Grupo_ASCC011_Consignc.NumOpConsigncConsigrio.Value };
            }
            catch (Exception erro)
            {
                LogHelper.AddLog("Erro: " + erro.ToString());
                throw;
            }

            using (WSCC011Client client = GetCredentialingClient())
            {
                try
                {
                    client.Open();
                    WSCC011Response retorno = client.WSCC011(re);
                    return retorno;
                }
                catch (Exception erro)
                {
                    LogHelper.AddLog("Erro: " + erro.ToString());
                    throw;
                }
            }
        }


        private WSCC011Client GetCredentialingClient()
        {
            WSCC011Client client = new WSCC011Client(Ambiente.GetCustomBinding(), new EndpointAddress(new Uri(Ambiente.url + "ASCC011/"), new DnsEndpointIdentity(Ambiente.dns), new AddressHeaderCollection()));

            client.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            Ambiente.SetClientCredentialsSecurity(client.ClientCredentials);

            var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
            if (vs != null)
            {
                client.Endpoint.Behaviors.Remove(vs);
            }

            return client;
        }





    }

    //Desaverbação Consignatário não Financeiro
    public class ASCC012Integrator
    {
        public WSCC012Response WSCC012(WSCC012Request Request)
        {
            WSCC012Request re = new WSCC012Request();
            try
            {
                re.CabecalhoReq = new SCC012Reference.CabecalhoReqComplexType();
                re.CabecalhoReq.DtHrEmis = DateTime.Now;
                re.CabecalhoReq.DtRef = DateTime.Now;
                re.CabecalhoReq.ISPBDestinatario = Request.CabecalhoReq.ISPBDestinatario;
                re.CabecalhoReq.ISPBEmissor = Request.CabecalhoReq.ISPBEmissor;
                re.CabecalhoReq.NumCtrlEmis = Request.CabecalhoReq.NumCtrlEmis;
                re.CabecalhoReq.DomSist = Request.CabecalhoReq.DomSist;
                re.CabecalhoReq.NUOp = Request.CabecalhoReq.NUOp;
                re.Corpo = new WSCC012CorpoComplexType();
                re.Corpo.ASCC012 = new Grupo_ASCC012_ConsigrioComplexType[1];
                re.Corpo.ASCC012[0].IdentdPartAdmdo = new SCC012Reference.CNPJBase_CodErro { Value = Request.Corpo.ASCC012[0].IdentdPartAdmdo.Value };
                re.Corpo.ASCC012[0].NumCtrlConsigrio = new SCC012Reference.ControleIF_CodErro() { Value = Request.Corpo.ASCC012[0].NumCtrlConsigrio.Value };
                re.Corpo.ASCC012[0].CNPJBaseEnte = new SCC012Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC012[0].CNPJBaseEnte.Value };
                re.Corpo.ASCC012[0].NumConsigrioEnte = new SCC012Reference.NumConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC012[0].NumConsigrioEnte.Value };
                //re.Corpo.ASCC012[0].NumDigtConsigrioEnte = new SCC012Reference.NumDigtConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC012[0].NumDigtConsigrioEnte.Value };
                re.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc = new SCC012Reference.Grupo_ASCC012_DesavebcComplexType();
                re.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.NumCPFServdr = new SCC012Reference.CPF_CodErro() { Value = Request.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.NumCPFServdr.Value };
                //re.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.IdentcServdr = new SCC012Reference.IdentcServdr_CodErro() { Value = Request.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.IdentcServdr.Value };
                //re.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.IdentcOrgao = new SCC012Reference.IdentcOrgao_CodErro() { Value = Request.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.IdentcOrgao.Value };
                //re.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.CNPJBaseOrgaoPagdr = new SCC012Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.CNPJBaseOrgaoPagdr.Value };
                re.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.NUAvebcSCC = new SCC012Reference.NU_CodErro() { Value = Request.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.NUAvebcSCC.Value };
                re.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.IdentcMotvDesavebcContrto = new SCC012Reference.IdentcMotvDesavebcContrto_CodErro() { Value = Request.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.IdentcMotvDesavebcContrto.Value };
                //re.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.Grupo_ASCC012_ParclContrtoSCC = new SCC012Reference.Grupo_ASCC012_ParclContrtoSCComplexType();
                //re.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.Grupo_ASCC012_ParclContrtoSCC.NumParcl = new SCC012Reference.NumParcl_CodErro() { Value = Request.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.Grupo_ASCC012_ParclContrtoSCC.NumParcl.Value };
                //re.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.Grupo_ASCC012_ParclContrtoSCC.VencParcl = new SCC012Reference.MesAno_CodErro() { Value = Request.Corpo.ASCC012[0].Grupo_ASCC012_Desavebc.Grupo_ASCC012_ParclContrtoSCC.VencParcl.Value };
            }
            catch (Exception erro)
            {
                LogHelper.AddLog("Erro: " + erro.ToString());
                throw;
            }

            using (WSCC012Client client = GetCredentialingClient())
            {
                try
                {
                    client.Open();
                    WSCC012Response retorno = client.WSCC012(re);
                    return retorno;
                }
                catch (Exception erro)
                {
                    LogHelper.AddLog("Erro: " + erro.ToString());
                    throw;
                }
            }
        }


        private WSCC012Client GetCredentialingClient()
        {
            WSCC012Client client = new WSCC012Client(Ambiente.GetCustomBinding(), new EndpointAddress(new Uri(Ambiente.url + "ASCC012/"), new DnsEndpointIdentity(Ambiente.dns), new AddressHeaderCollection()));

            client.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            Ambiente.SetClientCredentialsSecurity(client.ClientCredentials);

            var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
            if (vs != null)
            {
                client.Endpoint.Behaviors.Remove(vs);
            }

            return client;
        }





    }

    //Consulta Margem
    public class ASCC013Integrator
    {
        public WSCC013Response WSCC013(WSCC013Request Request)
        {
            WSCC013Request re = new WSCC013Request();
            try
            {
                re.CabecalhoReq = new SCC013Reference.CabecalhoReqComplexType();
                re.CabecalhoReq.NumCtrlEmis = Request.CabecalhoReq.NumCtrlEmis;
                re.CabecalhoReq.ISPBEmissor = Request.CabecalhoReq.ISPBEmissor;
                re.CabecalhoReq.ISPBDestinatario = Request.CabecalhoReq.ISPBDestinatario;
                re.CabecalhoReq.DtHrEmis = DateTime.Now;
                re.CabecalhoReq.DtRef = DateTime.Now;
                re.CabecalhoReq.DomSist = Request.CabecalhoReq.DomSist;
                re.CabecalhoReq.NUOp = Request.CabecalhoReq.NUOp;
                re.Corpo = new WSCC013CorpoComplexType();
                re.Corpo.ASCC013 = new Grupo_ASCC013_MargServdrComplexType[1];
                re.Corpo.ASCC013[0] = new Grupo_ASCC013_MargServdrComplexType();
                re.Corpo.ASCC013[0].IdentdPartAdmdo = new SCC013Reference.CNPJBase_CodErro { Value = Request.Corpo.ASCC013[0].IdentdPartAdmdo.Value }; 
                re.Corpo.ASCC013[0].NumCtrlConsigrio = new SCC013Reference.ControleIF_CodErro() { Value = Request.Corpo.ASCC013[0].NumCtrlConsigrio.Value };
                re.Corpo.ASCC013[0].CNPJBaseEnte = new SCC013Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC013[0].CNPJBaseEnte.Value }; 
                re.Corpo.ASCC013[0].NumConsigrioEnte = new SCC013Reference.NumConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC013[0].NumConsigrioEnte.Value };
                //re.Corpo.ASCC013[0].NumDigtConsigrioEnte = new SCC013Reference.NumDigtConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC013[0].NumDigtConsigrioEnte.Value };
                re.Corpo.ASCC013[0].NumCPFServdr = new SCC013Reference.CPF_CodErro() { Value = Request.Corpo.ASCC013[0].NumCPFServdr.Value };
                //re.Corpo.ASCC013[0].IdentcServdr = new SCC013Reference.IdentcServdr_CodErro() { Value = Request.Corpo.ASCC013[0].IdentcServdr.Value };
                //re.Corpo.ASCC013[0].IdentcOrgao = new SCC013Reference.IdentcOrgao_CodErro() { Value = Request.Corpo.ASCC013[0].IdentcOrgao.Value };
                re.Corpo.ASCC013[0].CNPJOrgaoPagdrServdr = new SCC013Reference.CNPJ_CodErro() { Value = Request.Corpo.ASCC013[0].CNPJOrgaoPagdrServdr.Value };
                re.Corpo.ASCC013[0].IdentcEsp = new SCC013Reference.IdentcEsp_CodErro() { Value = Request.Corpo.ASCC013[0].IdentcEsp.Value };
            }
            catch(Exception erro)
            {
                LogHelper.AddLog("Erro: " + erro.ToString());
                throw;
            }

            using (WSCC013Client client = GetCredentialingClient())
            {
                try
                {
                    client.Open();
                    WSCC013Response retorno = client.WSCC013(re);
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
            WSCC013Client client = new WSCC013Client(Ambiente.GetCustomBinding(), new EndpointAddress(new Uri(Ambiente.url + "ASCC013/"), new DnsEndpointIdentity(Ambiente.dns), new AddressHeaderCollection()));

            client.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            Ambiente.SetClientCredentialsSecurity(client.ClientCredentials);

            var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
            if (vs != null)
            {
                client.Endpoint.Behaviors.Remove(vs);
            }

            return client;
        }


        


    }

    //Cancelamento da Reserva de Averbação
    public class ASCC016Integrator
    {
        public WSCC016Response WSCC016(WSCC016Request Request)
        {
            WSCC016Request re = new WSCC016Request();
            try
            {
                re.CabecalhoReq = new SCC016Reference.CabecalhoReqComplexType();
                re.CabecalhoReq.DtHrEmis = DateTime.Now;
                re.CabecalhoReq.DtRef = DateTime.Now;
                re.CabecalhoReq.ISPBDestinatario = Request.CabecalhoReq.ISPBDestinatario;
                re.CabecalhoReq.ISPBEmissor = Request.CabecalhoReq.ISPBEmissor;
                re.CabecalhoReq.NumCtrlEmis = Request.CabecalhoReq.NumCtrlEmis;
                re.CabecalhoReq.DomSist = Request.CabecalhoReq.DomSist;
                re.CabecalhoReq.NUOp = Request.CabecalhoReq.NUOp;
                re.Corpo = new WSCC016CorpoComplexType();
                re.Corpo.ASCC016 = new Grupo_ASCC016_ConsigrioComplexType[1];
                re.Corpo.ASCC016[0].IdentdPartAdmdo = new SCC016Reference.CNPJBase_CodErro { Value = Request.Corpo.ASCC016[0].IdentdPartAdmdo.Value };
                re.Corpo.ASCC016[0].NumCtrlConsigrio = new SCC016Reference.ControleIF_CodErro() { Value = Request.Corpo.ASCC016[0].NumCtrlConsigrio.Value };
                re.Corpo.ASCC016[0].CNPJBaseEnte = new SCC016Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC016[0].CNPJBaseEnte.Value };
                re.Corpo.ASCC016[0].NumConsigrioEnte = new SCC016Reference.NumConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC016[0].NumConsigrioEnte.Value };
                //re.Corpo.ASCC016[0].NumDigtConsigrioEnte = new SCC016Reference.NumDigtConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC016[0].NumDigtConsigrioEnte.Value };
                re.Corpo.ASCC016[0].Grupo_ASCC016_ResConsignc = new SCC016Reference.Grupo_ASCC016_ResConsigncComplexType();
                re.Corpo.ASCC016[0].Grupo_ASCC016_ResConsignc.NUAvebcSCC = new SCC016Reference.NU_CodErro() { Value = Request.Corpo.ASCC016[0].Grupo_ASCC016_ResConsignc.NUAvebcSCC.Value };

            }
            catch (Exception erro)
            {
                LogHelper.AddLog("Erro: " + erro.ToString());
                throw;
            }

            using (WSCC016Client client = GetCredentialingClient())
            {
                try
                {
                    client.Open();
                    WSCC016Response retorno = client.WSCC016(re);
                    return retorno;
                }
                catch (Exception erro)
                {
                    LogHelper.AddLog("Erro: " + erro.ToString());
                    throw;
                }
            }
        }


        private WSCC016Client GetCredentialingClient()
        {
            WSCC016Client client = new WSCC016Client(Ambiente.GetCustomBinding(), new EndpointAddress(new Uri(Ambiente.url + "ASCC016/"), new DnsEndpointIdentity(Ambiente.dns), new AddressHeaderCollection()));

            client.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            Ambiente.SetClientCredentialsSecurity(client.ClientCredentials);

            var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
            if (vs != null)
            {
                client.Endpoint.Behaviors.Remove(vs);
            }

            return client;
        }





    }

    //Consulta Averbação
    public class ASCC022Integrator
    {
        public WSCC022Response WSCC022(WSCC022Request Request)
        {
            WSCC022Request re = new WSCC022Request();
            try
            {
                re.CabecalhoReq = new SCC022Reference.CabecalhoReqComplexType();
                re.CabecalhoReq.DtHrEmis = DateTime.Now;
                re.CabecalhoReq.DtRef = DateTime.Now;
                re.CabecalhoReq.ISPBDestinatario = Request.CabecalhoReq.ISPBDestinatario;
                re.CabecalhoReq.ISPBEmissor = Request.CabecalhoReq.ISPBEmissor;
                re.CabecalhoReq.NumCtrlEmis = Request.CabecalhoReq.NumCtrlEmis;
                re.CabecalhoReq.DomSist = Request.CabecalhoReq.DomSist;
                re.CabecalhoReq.NUOp = Request.CabecalhoReq.NUOp;
                re.Corpo = new WSCC022CorpoComplexType();
                re.Corpo.ASCC022 = new Grupo_ASCC022_ConsigrioComplexType[1];
                re.Corpo.ASCC022[0].IdentdPartAdmdo = new SCC022Reference.CNPJBase_CodErro { Value = Request.Corpo.ASCC022[0].IdentdPartAdmdo.Value };
                //re.Corpo.ASCC022[0].CNPJBaseEnte = new SCC022Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC022[0].CNPJBaseEnte.Value };
                re.Corpo.ASCC022[0].NumConsigrioEnte = new SCC022Reference.NumConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC022[0].NumConsigrioEnte.Value };
                //re.Corpo.ASCC022[0].NumDigtConsigrioEnte = new SCC022Reference.NumDigtConsigrioEnte_CodErro() { Value = Request.Corpo.ASCC022[0].NumDigtConsigrioEnte.Value };
                re.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc = new SCC022Reference.Grupo_ASCC022_ConsAvebcComplexType();
                //re.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.NumCPFServdr = new SCC022Reference.CPF_CodErro() { Value = Request.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.NumCPFServdr.Value };
                //re.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.IdentcServdr = new SCC022Reference.IdentcServdr_CodErro() { Value = Request.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.IdentcServdr.Value };
                //re.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.IdentcOrgao = new SCC022Reference.IdentcOrgao_CodErro() { Value = Request.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.IdentcOrgao.Value };
                //re.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.CNPJBaseOrgaoPagdr = new SCC022Reference.CNPJBase_CodErro() { Value = Request.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.CNPJBaseOrgaoPagdr.Value };
                //re.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.NUAvebcSCC = new SCC022Reference.NU_CodErro() { Value = Request.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.NUAvebcSCC.Value };
                //re.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.DtIniConsAvebc = new SCC022Reference.Data_CodErro() { Value = Request.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.DtIniConsAvebc.Value };
                //re.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.DtFimConsAvebc = new SCC022Reference.Data_CodErro() { Value = Request.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.DtFimConsAvebc.Value };
                //re.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.SitAvebc = new SCC022Reference.SitAvebc_CodErro() { Value = Request.Corpo.ASCC022[0].Grupo_ASCC022_ConsAvebc.SitAvebc.Value };

            }
            catch (Exception erro)
            {
                LogHelper.AddLog("Erro: " + erro.ToString());
                throw;
            }

            using (WSCC022Client client = GetCredentialingClient())
            {
                try
                {
                    client.Open();
                    WSCC022Response retorno = client.WSCC022(re);
                    return retorno;
                }
                catch (Exception erro)
                {
                    LogHelper.AddLog("Erro: " + erro.ToString());
                    throw;
                }
            }
        }


        private WSCC022Client GetCredentialingClient()
        {
            WSCC022Client client = new WSCC022Client(Ambiente.GetCustomBinding(), new EndpointAddress(new Uri(Ambiente.url + "ASCC022/"), new DnsEndpointIdentity(Ambiente.dns), new AddressHeaderCollection()));

            client.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            Ambiente.SetClientCredentialsSecurity(client.ClientCredentials);

            var vs = client.Endpoint.EndpointBehaviors.FirstOrDefault((i) => i.GetType().Namespace == "Microsoft.VisualStudio.Diagnostics.ServiceModelSink");
            if (vs != null)
            {
                client.Endpoint.Behaviors.Remove(vs);
            }

            return client;
        }





    }
















}
