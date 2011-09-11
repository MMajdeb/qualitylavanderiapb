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

namespace ERP.Lavanderia.Module.PacoteControladoress.ControladoresLavagem
{
    public partial class ImprimirLavagemViewController : ViewController
    {
        public readonly static string NOME_RELATORIO_LAVAGEM = "Lavagem";

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
                ReportData reportdata = ObjectSpace.FindObject<ReportData>(new BinaryOperator("Name", NOME_RELATORIO_LAVAGEM));
                var xtraReport = reportdata.LoadXtraReport(ObjectSpace);
                xtraReport.FilterString = new BinaryOperator("Oid", lavagemSelecionada.Oid).ToString();
                xtraReport.ShowPreviewDialog();
            }
            catch (Exception ex){
                throw new UserFriendlyException("Erro ao imprimir a lavagem: " + ex.Message);
            }
        }

    }
}
