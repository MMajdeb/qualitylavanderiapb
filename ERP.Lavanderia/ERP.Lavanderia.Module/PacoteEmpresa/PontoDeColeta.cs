using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacoteEndereco;

namespace ERP.Lavanderia.Module.PacoteEmpresa
{
    [DefaultProperty("Nome")]
    [DefaultClassOptions]
    public class PontoDeColeta : BaseObject
    {
        private string nome;
        private Endereco endereco;
        private string telefoneConvencional;
        private string telefoneMovel;

        public PontoDeColeta(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        [Size(100)]
        [RuleRequiredField("RuleRequiredField PontoDeColeta.Nome", DefaultContexts.Save)]
        public string Nome
        {
            get { return nome; }
            set
            {
                SetPropertyValue("Nome", ref nome, value);
            }
        }

        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public Endereco Endereco
        {
            get { return endereco; }
            set
            {
                SetPropertyValue("Endereco", ref endereco, value);
            }
        }

        [Size(16)]
        public string TelefoneConvencional
        {
            get { return telefoneConvencional; }
            set
            {
                SetPropertyValue("TelefoneConvencional", ref telefoneConvencional, value);
            }
        }

        [Size(16)]
        public string TelefoneMovel
        {
            get { return telefoneMovel; }
            set
            {
                SetPropertyValue("TelefoneMovel", ref telefoneMovel, value);
            }
        }
    }

}
