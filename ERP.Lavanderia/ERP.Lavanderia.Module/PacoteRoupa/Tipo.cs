using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace ERP.Lavanderia.Module.PacoteRoupa
{
    [DefaultProperty("Descricao")]
    [DefaultClassOptions]
    public class Tipo : BaseObject
    {
        private string descricao;

        public Tipo(Session session)
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

        [RuleUniqueValue("Tipo.DescricaoUnica", DefaultContexts.Save, @"""Descrição"" já existe.")]
        public string Descricao
        {
            get { return descricao; }
            set {
                SetPropertyValue("Descricao", ref descricao, value);
            }
        }
    }

}
