using CIPLibrary.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CoopMilWService
{
    public partial class CoopMilService : ServiceBase
    {
        ServiceHost host;

        public CoopMilService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
//            LogHelper.AddLog("Inicializando serviço");
            try
            {
                host = new ServiceHost(typeof(CIPLibrary.WCF.Classes.ASCC013Service));
                host.Open();
//                LogHelper.AddLog("Serviço iniciado com sucesso");
            }
            catch (Exception erro)
            {
                LogHelper.AddLog("Erro ao iniciar o serviço: " + erro.ToString());
            }
        }

        protected override void OnStop()
        {
//            LogHelper.AddLog("Parando serviço");
            host.Close();
//            LogHelper.AddLog("Serviço parado com sucesso");
        }
    }
}
