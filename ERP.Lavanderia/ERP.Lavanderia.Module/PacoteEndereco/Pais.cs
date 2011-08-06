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
    [DefaultProperty("NomePais")]
    public class Pais : BaseObject
    {
        public Pais(Session session) : base(session) { }
        private String nomePais;
        private String codigoTelefone;
        public String NomePais
        {
            get
            {
                return nomePais;
            }
            set
            {
                SetPropertyValue("NomePais", ref nomePais, value);
            }
        }

        public String CodigoTelefone
        {
            get
            {
                return codigoTelefone;
            }
            set
            {
                SetPropertyValue("CodigoTelefone", ref codigoTelefone, value);
            }
        }

        [Association("Pais-Estados")]
        public XPCollection<Estado> Estados
        {
            get { return GetCollection<Estado>("Estados"); }
        }

        /// <summary>
        /// Retorna um objeto Pais pelo nome passado.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        public static Pais RetornaPaisPorNome(IObjectSpace objectSpace, string nome)
        {
            Pais pais = objectSpace.FindObject<Pais>(new BinaryOperator("NomePais", nome));
            return pais;
        }
    }

}
