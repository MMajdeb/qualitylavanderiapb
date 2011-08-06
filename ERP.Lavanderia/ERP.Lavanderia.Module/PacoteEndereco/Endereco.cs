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
    [DefaultProperty("EnderecoCompleto")]
    [CalculatedPersistentAliasAttribute("EnderecoCompleto", "FullAddressPersistentAlias")]
    public class Endereco : BaseObject
    {
        private const string formatoEnderecoCompletoPadrao = "{Logradouro}, {Numero}, {Bairro}, {Cidade.NomeCidade}, {Estado.NomeEstado}, {Pais.NomePais}, {Cep}";
        private const string aliasEnderecoCompletoPadrao = "concat(Logradouro, Numero, Bairro, Cidade.NomeCidade, Estado.NomeEstado, Pais.NomePais, Cep)";

        static Endereco()
        {
            formatoEnderecoCompleto = formatoEnderecoCompletoPadrao;
        }

        private string cep;
        private string logradouro;
        private string numero;
        private string complemento;
        private string bairro;

        private static string aliasEnderecoCompleto = aliasEnderecoCompletoPadrao;
        private static string formatoEnderecoCompleto = formatoEnderecoCompletoPadrao;

        public Endereco(Session session) : base(session) { }

        public static string FormatoEnderecoCompleto
        {
            get { return formatoEnderecoCompleto; }
            set { formatoEnderecoCompleto = value; }
        }

        public static string AliasEnderecoCompleto
        {
            get
            {
                return aliasEnderecoCompleto;
            }
        }

        public static void SetFormatoEnderecoCompleto(string format, string persistentAlias)
        {
            formatoEnderecoCompleto = format;
            aliasEnderecoCompleto = persistentAlias;
        }

        public string Cep
        {
            get { return cep; }
            set
            {
                cep = value;
                OnChanged("Cep");
            }
        }

        [RuleRequiredField("RuleRequiredField Endereco.Logradouro", DefaultContexts.Save)]
        public string Logradouro
        {
            get { return logradouro; }
            set
            {
                logradouro = value;
                OnChanged("Logradouro");
            }
        }

        [Size(20)]
        public string Numero
        {
            get { return numero; }
            set { SetPropertyValue("Numero", ref numero, value); }
        }

        [Size(50)]
        public string Complemento
        {
            get { return complemento; }
            set { SetPropertyValue("Complemento", ref complemento, value); }
        }

        [RuleRequiredField("RuleRequiredField Endereco.Bairro", DefaultContexts.Save)]
        public string Bairro
        {
            get { return bairro; }
            set
            {
                bairro = value;
                OnChanged("Bairro");
            }
        }

        public string EnderecoCompleto
        {
            get
            {
                return ObjectFormatter.Format(formatoEnderecoCompleto, this, EmptyEntriesMode.RemoveDelimeterWhenEntryIsEmpty);
            }
        }

        private Pais pais;
        [RuleRequiredField("RuleRequiredField Endereco.Pais", DefaultContexts.Save)]
        public Pais Pais
        {
            get
            {
                return pais;
            }
            set
            {
                SetPropertyValue("Pais", ref pais, value);
                //if (!IsLoading)
                //{
                //    Estado = null;
                //}
            }
        }

        private Estado estado;
        [DataSourceProperty("Pais.Estados")]
        [RuleRequiredField("RuleRequiredField Endereco.Estado", DefaultContexts.Save)]
        public Estado Estado
        {
            get
            {
                return estado;
            }
            set
            {
                SetPropertyValue("Estado", ref estado, value);
                //if (!IsLoading)
                //{
                //    Cidade = null;
                //}
            }
        }

        private Cidade cidade;
        [DataSourceProperty("Estado.Cidades")]
        [RuleRequiredField("RuleRequiredField Endereco.Cidade", DefaultContexts.Save)]
        public Cidade Cidade
        {
            get
            {
                return cidade;
            }
            set
            {
                SetPropertyValue("Cidade", ref cidade, value);
            }
        }
    }

}
