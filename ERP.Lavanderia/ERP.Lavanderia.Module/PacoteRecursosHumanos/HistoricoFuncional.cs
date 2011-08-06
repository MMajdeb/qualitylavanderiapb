using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacoteColaborador;

namespace ERP.Lavanderia.Module.PacoteRecursosHumanos
{
    [DefaultClassOptions]
    public class HistoricoFuncional : BaseObject
    {
        private Colaborador colaborador;
        private DateTime dataInicial;
        private DateTime dataFinal;
        private string observacoes;

        public HistoricoFuncional(Session session) : base(session) { }

        [RuleRequiredField("RuleRequiredField HistoricoFuncional.Colaborador", DefaultContexts.Save)]
        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public Colaborador Colaborador
        {
            get { return colaborador; }
            set { SetPropertyValue("Colaborador", ref colaborador, value); }
        }


        public DateTime DataInicial
        {
            get { return dataInicial; }
            set { SetPropertyValue("DataInicial", ref dataInicial, value); }
        }


        public DateTime DataFinal
        {
            get { return dataFinal; }
            set { SetPropertyValue("DataFinal", ref dataFinal, value); }
        }


        public string Observacoes
        {
            get { return observacoes; }
            set { SetPropertyValue("Observacoes", ref observacoes, value); }
        }
    }

}
