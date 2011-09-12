using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using ERP.Lavanderia.Module.PacoteLavagem;
using DevExpress.ExpressApp.Reports;
using DevExpress.Data.Filtering;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Web;
using DevExpress.XtraReports.UI;
using ERP.Lavanderia.Module.PacoteConfiguracoes;
using System.IO;

namespace ERP.Lavanderia.Module.PacoteControladoress.ControladoresLavagem
{
    public partial class ImprimirLavagemViewController : ViewController
    {

        public ImprimirLavagemViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        private void saImprimir_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Lavagem lavagemSelecionada = null;

            if (View is ListView) {
                ListView listView = View as ListView;

                try
                {
                    lavagemSelecionada = listView.SelectedObjects[0] as Lavagem;
                }
                catch {
                    throw new UserFriendlyException("Selecione uma lavagem.");
                }
            }
            else {
                DetailView detailView = View as DetailView;
                lavagemSelecionada = detailView.CurrentObject as Lavagem;
            }

            try {
                ReportData reportdata = ObjectSpace.FindObject<ReportData>(new BinaryOperator("Name", ConfiguracaoGeral.NOME_RELATORIO_LAVAGEM));
                var xtraReport = reportdata.LoadXtraReport(ObjectSpace);
                xtraReport.FilterString = new BinaryOperator("Oid", lavagemSelecionada.Oid).ToString();
                xtraReport.CreateDocument();

                var info = Directory.CreateDirectory(ConfiguracaoGeral.DIRETORIO_PARA_TEMPORARIOS);
                ConfiguracaoGeral cfg = ConfiguracaoGeral.RetornaConfiguracaoGeral(View.ObjectSpace);

                string reportPath = ConfiguracaoGeral.DIRETORIO_PARA_TEMPORARIOS + "\\Test.pdf";

                reportPath = reportPath.Replace(" ", "-");

                xtraReport.ExportToPdf(reportPath);
                StartProcess(reportPath);

                /*** Essa porra so funciona pra Windows ***/
                //ReportPrintTool pt = new ReportPrintTool(xtraReport);
                //pt.ShowPreviewDialog();
                
            }
            catch (Exception ex){
                throw new UserFriendlyException("Erro ao imprimir a lavagem: " + ex.Message);
            }
        }

        public void StartProcess(string path)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.FileName = path;
                process.Start();
                process.WaitForInputIdle();
            }
            catch { }
        }

    }
}
