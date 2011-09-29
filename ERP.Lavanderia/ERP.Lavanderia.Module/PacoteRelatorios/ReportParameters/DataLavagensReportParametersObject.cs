using System;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Reports;
using ERP.Lavanderia.Module.PacoteConfiguracoes;

namespace ERP.Lavanderia.Module.PacoteRelatorios.ReportParameters
{
    [NonPersistent]
    public class DataLavagensReportParametersObject : ReportParametersObjectBase
    {
        private DateTime data;

        public DataLavagensReportParametersObject(Session session) : base(session) { }

        [RuleRequiredField("RuleRequiredField DataLavagensReportParametersObject.Data", "PreviewReport", "Data não pode ser vazia")]
        public DateTime Data
        {
            get { return data; }
            set { SetPropertyValue("Data", ref data, value); }
        }

        public override void AfterConstruction()
        {
            Data = ConfiguracaoGeral.RetornaConfiguracaoGeral(Session).DataHoraAtual;

            base.AfterConstruction();
        }

        public override CriteriaOperator GetCriteria()
        {
            DateTime dataInicio = new DateTime(data.Year, data.Month, data.Day, 0, 0, 0);
            DateTime dataFim = new DateTime(data.Year, data.Month, data.Day, 23, 59, 59);

            return new BetweenOperator("Lavagem.DataHoraDeRecebimento", dataInicio, dataFim);
        }

        public override SortingCollection GetSorting()
        {
            SortingCollection sorting = new SortingCollection();
            return sorting;
        }
    }

}
