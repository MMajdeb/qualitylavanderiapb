using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Collections;
using DevExpress.Xpo.Metadata;

namespace ERP.Lavanderia.Module.PacoteMaterial
{
    [DefaultProperty("Nome")]
    [DefaultClassOptions]
    public class Material : BaseObject
    {
        private string nome;
        private int quantidadeEmCaixa;
        private string descricao;

        public Material(Session session)
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
        [RuleUniqueValue("Material.NomeUnico", DefaultContexts.Save, @"""Nome"" já existe.")]
        [RuleRequiredField("RuleRequiredField Material.Nome", DefaultContexts.Save)]
        public string Nome
        {
            get { return nome; }
            set
            {
                SetPropertyValue("Nome", ref nome, value);
            }
        }

        public string Descricao
        {
            get { return descricao; }
            set
            {
                SetPropertyValue("Descricao", ref descricao, value);
            }
        }

        public int QuantidadeEmCaixa
        {
            get { return quantidadeEmCaixa; }
            set {
                SetPropertyValue("QuantidadeEmCaixa", ref quantidadeEmCaixa, value);
            }
        }

        [Association("Material-MovimentacaoMaterial", typeof(MovimentacaoMaterial)), Aggregated]
        public XPCollection Movimentacoes
        {
            get
            {
                return GetCollection("Movimentacoes");
            }
        }

        [RuleFromBoolProperty("Material.RuleFromBoolProperty.ValidaDeletar", DefaultContexts.Delete,
    CustomMessageTemplate = "Esse material possui associações e não pode ser deletado")]
        [Browsable(false)]
        public bool ValidaDeletar
        {
            get
            {
                ICollection objs = Session.CollectReferencingObjects(this);
                if (objs.Count > 0)
                {
                    foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
                    {
                        //if (mi.IsAggregated && mi.IsCollection && mi.IsAssociation)
                        if (mi.IsCollection && mi.IsAssociation)
                        {
                            foreach (IXPObject obj in objs)
                            {
                                if (((XPBaseCollection)mi.GetValue(this)).BaseIndexOf(obj) < 0)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
        }
    }

}
