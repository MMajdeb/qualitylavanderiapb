using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace ERP.Lavanderia.Module.PacoteCaixa
{
    [DefaultProperty("Nome")]
    [DefaultClassOptions]
    public class Caixa : BaseObject
    {
        private string nome;

        public Caixa(Session session)
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
        [RuleUniqueValue("Caixa.NomeUnico", DefaultContexts.Save, @"""Nome"" já existe.")]
        [RuleRequiredField("RuleRequiredField Caixa.Nome", DefaultContexts.Save)]
        public string Nome
        {
            get { return nome; }
            set
            {
                SetPropertyValue("Nome", ref nome, value);
            }
        }

        public static Caixa RetornaCaixa(Session session, string nome)
        {
            return session.FindObject<Caixa>(new BinaryOperator("Nome", nome));
        }

        public static XPCollection RetornaCaixas(Session session)
        {
            return new XPCollection(session, typeof(Caixa));
        }
    }

}
