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
    [DefaultClassOptions]
    public class PacoteDeRoupa : BaseObject
    {
        private Lavagem lavagem;

        public PacoteDeRoupa(Session session)
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

        [Browsable(false)]
        [RuleRequiredField("RuleRequiredField PacoteDeRoupa.Lavagem", DefaultContexts.Save)]
        [Association("Lavagem-PacoteDeRoupa")]
        public Lavagem Lavagem
        {
            get { return lavagem; }
            set { SetPropertyValue("Lavagem", ref lavagem, value); }
        }

        [DataSourceCriteria("Lavagem = '@This.Lavagem'")]
        [Association("PacoteDeRoupa-RoupaLavagem", typeof(RoupaLavagem))]
        public XPCollection Roupas
        {
            get { return GetCollection("Roupas"); }
        }
    }

}
