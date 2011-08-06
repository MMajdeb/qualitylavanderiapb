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
    [DefaultProperty("NomeCidade")]
    public class Cidade : BaseObject
    {
        public Cidade(Session session) : base(session) { }
        private String nomeCidade;
        public String NomeCidade
        {
            get
            {
                return nomeCidade;
            }
            set
            {
                SetPropertyValue("NomeCidade", ref nomeCidade, value);
            }
        }
        private Estado estado;
        [Association("Estado-Cidades")]
        public Estado Estado
        {
            get
            {
                return estado;
            }
            set
            {
                SetPropertyValue("Estado", ref estado, value);
            }
        }

        /// <summary>
        /// Retorna um objeto Cidade de acordo com o nome passado.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        public static Cidade RetornaCidadePorNome(IObjectSpace objectSpace, string nome)
        {
            Cidade cidade = objectSpace.FindObject<Cidade>(new BinaryOperator("NomeCidade", nome));
            return cidade;
        }
    }

}
