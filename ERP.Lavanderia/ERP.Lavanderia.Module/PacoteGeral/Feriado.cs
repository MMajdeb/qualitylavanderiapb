using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacoteEmpresa;
using ERP.Lavanderia.Module.PacoteEndereco;

namespace ERP.Lavanderia.Module.PacoteGeral
{
    [DefaultClassOptions]
    public class Feriado : BaseObject
    {
        private string descricao;
        private int dia;
        private int mes;
        private int ano;
        private Empresa empresa;
        private Pais pais;

        public Feriado(Session session) : base(session) { }

        [Size(100)]
        [RuleRequiredField("RuleRequiredField Feriado.Descricao", DefaultContexts.Save)]
        [RuleUniqueValue("Feriado.DescricaoUnica", DefaultContexts.Save, @"""Feriado"" já está cadastrado")]
        public string Descricao
        {
            get { return descricao; }
            set { SetPropertyValue("Descricao", ref descricao, value); }
        }

        [RuleRequiredField("RuleRequiredField Feriado.Dia", DefaultContexts.Save)]
        public int Dia
        {
            get { return dia; }
            set { SetPropertyValue("Dia", ref dia, value); }
        }

        [RuleRequiredField("RuleRequiredField Feriado.Mes", DefaultContexts.Save)]
        public int Mes
        {
            get { return mes; }
            set { SetPropertyValue("Mes", ref mes, value); }
        }

        public int Ano
        {
            get { return ano; }
            set { SetPropertyValue("Ano", ref ano, value); }
        }

        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public Empresa Empresa
        {
            get { return empresa; }
            set { SetPropertyValue("Empresa", ref empresa, value); }
        }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public Pais Pais
        {
            get
            {
                return pais;
            }
            set
            {
                SetPropertyValue("Pais", ref pais, value);
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            this.pais = Session.FindObject<Pais>(new BinaryOperator("NomePais", "Brasil"));
        }

        #region Metodos



        public static Feriado RetornaFeriadoPorDiaeMes(int dia, int mes, Session session)
        {

            return session.FindObject<Feriado>(new BinaryOperator("dia", dia) & new BinaryOperator("mes", mes));

        }

        public static bool VerificaSeEFeriado(DateTime data, Session session) {
            return RetornaFeriadoPorDiaeMes(data.Day, data.Month, session) != null;
        }
        #endregion

    }

}
