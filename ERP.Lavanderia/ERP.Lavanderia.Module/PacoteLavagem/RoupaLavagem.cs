using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacoteRoupa;

namespace ERP.Lavanderia.Module.PacoteLavagem
{
    [DefaultProperty("ToStringProperty")]
    [DefaultClassOptions]
    public class RoupaLavagem : BaseObject
    {
        private int quantidade;
        private Roupa roupa;
        private Lavagem lavagem;
        private PacoteDeRoupa pacoteDeRoupa;

        public RoupaLavagem(Session session)
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

        [RuleRequiredField("RuleRequiredField RoupaLavagem.Quantidade", DefaultContexts.Save)]
        public int Quantidade
        {
            get { return quantidade; }
            set {
                SetPropertyValue("Quantidade", ref quantidade, value);
            }
        }

        [DataSourceCriteria("Cliente = '@This.Lavagem.Cliente'")]
        [RuleRequiredField("RuleRequiredField RoupaLavagem.Roupa", DefaultContexts.Save)]
        public Roupa Roupa
        {
            get { return roupa; }
            set {
                SetPropertyValue("Roupa", ref roupa, value);
            }
        }

        [Browsable(false)]
        [Association("Lavagem-RoupaLavagem", typeof(Lavagem))]
        public Lavagem Lavagem
        {
            get { return lavagem; }
            set {
                SetPropertyValue("Lavagem", ref lavagem, value);
            }
        }

        [Association("PacoteDeRoupa-RoupaLavagem")]
        public PacoteDeRoupa PacoteDeRoupa
        {
            get { return pacoteDeRoupa; }
            set { SetPropertyValue("PacoteDeRoupa", ref pacoteDeRoupa, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public string ToStringProperty
        {
            get {
                return "(" + Quantidade + ") " + Roupa;
            }
        }

        [NonPersistent]
        [System.ComponentModel.Browsable(false)]
        [RuleFromBoolProperty("RuleFromBoolProperty RoupaLavagem.ValidaQuantidade", DefaultContexts.Save, @"Quantidade deve ser maior que zero.")]
        public bool ValidaQuantidade
        {
            get
            {
                return Quantidade > 0;
            }
        }
    }

}
