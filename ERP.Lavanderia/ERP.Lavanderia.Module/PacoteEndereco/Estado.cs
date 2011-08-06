using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace ERP.Lavanderia.Module.PacoteEndereco
{
    [DefaultProperty("NomeEstado")]
    public class Estado : BaseObject
    {
        public Estado(Session session) : base(session) { }
        private String nomeEstado;

        [RuleRequiredField("RuleRequiredField Estado.NomeEstado", DefaultContexts.Save)]
        public String NomeEstado
        {
            get
            {
                return nomeEstado;
            }
            set
            {
                SetPropertyValue("NomeEstado", ref nomeEstado, value);
            }
        }

        private String sigla;
        [RuleRequiredField("RuleRequiredField Estado.Sigla", DefaultContexts.Save)]
        [Size(2)]
        public String Sigla
        {
            get
            {
                return sigla;
            }
            set
            {
                SetPropertyValue("Sigla", ref sigla, value);
            }
        }

        private Pais pais;
        [Association("Pais-Estados")]
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

        [Association("Estado-Cidades")]
        public XPCollection<Cidade> Cidades
        {
            get { return GetCollection<Cidade>("Cidades"); }
        }

        /// <summary>
        /// Retorna um objeto Estado pela sigla passada.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sigla"></param>
        /// <returns></returns>
        public static Estado RetornaEstadoPorSigla(IObjectSpace objectSpace, string sigla)
        {
            Estado estado = objectSpace.FindObject<Estado>(new BinaryOperator("Sigla", sigla));
            return estado;
        }
    }

}
