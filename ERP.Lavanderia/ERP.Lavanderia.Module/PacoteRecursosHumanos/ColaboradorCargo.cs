using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace ERP.Lavanderia.Module.PacoteRecursosHumanos
{
    [DefaultClassOptions]
    [DefaultProperty("Descricao")]
    public class ColaboradorCargo : BaseObject
    {
        private string descricao;
        private string observacoes;

        public ColaboradorCargo(Session session) : base(session) { }

        [RuleRequiredField("RuleRequiredField ColaboradorCargo.Descricao", DefaultContexts.Save)]
        [RuleUniqueValue("ColaboradorCargo.DescricaoUnica", DefaultContexts.Save, @"Descrição já cadastrada")]
        [Size(100)]
        public string Descricao
        {
            get { return descricao; }
            set { SetPropertyValue("Descricao", ref descricao, value); }
        }

        [Size(4096)]
        public string Observacoes
        {
            get { return observacoes; }
            set { SetPropertyValue("Observacoes", ref observacoes, value); }
        }

        public static ColaboradorCargo RetornaCargo(Session session, string descricao)
        {
            return session.FindObject<ColaboradorCargo>(new BinaryOperator("Descricao", descricao));
        }
    }

}
