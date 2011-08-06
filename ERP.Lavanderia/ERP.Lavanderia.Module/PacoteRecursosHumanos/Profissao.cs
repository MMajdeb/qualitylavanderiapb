using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace ERP.Lavanderia.Module.RecursosHumanos
{
    [DefaultClassOptions]
    [DefaultProperty("Nome")]
    public class Profissao : BaseObject
    {
        private string nome;

        public Profissao(Session session) : base(session) { }

        public override string ToString()
        {
            return Nome;
        }

        [Size(50)]
        [RuleRequiredField("RuleRequiredField Profissao.Nome", DefaultContexts.Save)]
        [RuleUniqueValue("Profissao.NomeUnico", DefaultContexts.Save, @"""Profissão"" já está cadastrada.")]
        public string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
                OnChanged("Nome");
            }
        }
    }

}
