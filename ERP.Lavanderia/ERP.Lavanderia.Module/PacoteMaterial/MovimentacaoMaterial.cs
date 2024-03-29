using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacoteColaborador;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ERP.Lavanderia.Module.PacoteMaterial
{
    [DefaultProperty("ToStringProperty")]
    [DefaultClassOptions]
    public class MovimentacaoMaterial : BaseObject
    {
        private DateTime data;
        private string observacoes;
        private Modo modo;
        private Material material;
        private int quantidade;
        private Colaborador colaborador;

        /// <summary>
        /// Usado para atualizacoes na quantidade
        /// </summary>
        private int quantidadeAnterior;
        private Modo? modoAnterior;

        public MovimentacaoMaterial(Session session)
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

            Data = DateTime.Now;
            Colaborador = Colaborador.RetornaColaboradorLogado(Session);
        }

        [RuleRequiredField("RuleRequiredField MovimentacaoMaterial.Colaborador", DefaultContexts.Save)]
        public Colaborador Colaborador
        {
            get { return colaborador; }
            set { SetPropertyValue("Colaborador", ref colaborador, value); }
        }

        [Association("Material-MovimentacaoMaterial", typeof(Material))]
        [RuleRequiredField("RuleRequiredField MovimentacaoMaterial.Material", DefaultContexts.Save)]
        public Material Material
        {
            get { return material; }
            set { SetPropertyValue("Material", ref material, value); }
        }

        [RuleRequiredField("RuleRequiredField MovimentacaoMaterial.Data", DefaultContexts.Save)]
        public DateTime Data
        {
            get { return data; }
            set { SetPropertyValue("Data", ref data, value); }
        }

        [RuleRequiredField("RuleRequiredField MovimentacaoMaterial.Modo", DefaultContexts.Save)]
        public Modo Modo
        {
            get { return modo; }
            set {
                if (modoAnterior == null) {
                    modoAnterior = modo;
                }

                SetPropertyValue("Modo", ref modo, value); 
            }
        }

        [RuleRequiredField("RuleRequiredField MovimentacaoMaterial.Quantidade", DefaultContexts.Save)]
        public int Quantidade
        {
            get { return quantidade; }
            set {
                if (quantidadeAnterior == 0)
                {
                    quantidadeAnterior = quantidade;
                }

                SetPropertyValue("Quantidade", ref quantidade, value); 
            }
        }

        public string Observacoes
        {
            get { return observacoes; }
            set { SetPropertyValue("Observacoes", ref observacoes, value); }
        }

        [NonPersistent]
        [System.ComponentModel.Browsable(false)]
        [RuleFromBoolProperty("MovimentacaoMaterial.QuantidadeMaiorQueZero", DefaultContexts.Save, @"""Quantidade"" deve ser maior que zero.")]
        public bool QuantidadeMaiorQueZero
        {
            get
            {
                return Quantidade > 0;
            }
        }

        [NonPersistent]
        [System.ComponentModel.Browsable(false)]
        [RuleFromBoolProperty("MovimentacaoMaterial.RetiradaNaoDeixaMaterialComQuantidadeNegativa", DefaultContexts.Save, 
            @"O material n�o tem quantidade suficiente para uma retirada nessa quantia.")]
        public bool RetiradaNaoDeixaMaterialComQuantidadeNegativa
        {
            get
            {
                if (Material == null) {
                    return true;
                }

                return Modo == Modo.Entrada || Material.QuantidadeEmCaixa - Quantidade >= 0;
            }
        }

        protected override void OnSaving()
        {
            if (quantidadeAnterior != 0) {
                Modo modoUtilizado = modoAnterior != null ? (Modo) modoAnterior : modo;

                Material.QuantidadeEmCaixa += modoUtilizado == Modo.Entrada ? -quantidadeAnterior : quantidadeAnterior;
            }

            Material.QuantidadeEmCaixa += Modo == Modo.Entrada ? Quantidade : -Quantidade;

            base.OnSaving();
        }

        protected override void OnDeleting()
        {
            Material.QuantidadeEmCaixa += Modo == Modo.Entrada ? -Quantidade : Quantidade;

            base.OnDeleting();
        }

        [Browsable(false)]
        [NonPersistent]
        public string ToStringProperty
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            try
            {
                return "(" + Material.Nome + ") " + Modo + "; " + Quantidade + "; " + Colaborador.Pessoa.Nome + "; " + Data;
            }
            catch {
                return "N�o foi poss�vel exibir a descri��o";
            }
        }

        #region Modulo Auditoria
        //M�dulo de auditoria
        private ReadOnlyCollection<AuditDataItemPersistent> changeHistory;
        public ReadOnlyCollection<AuditDataItemPersistent> ChangeHistory
        {
            get
            {
                if (changeHistory == null)
                {
                    IList<AuditDataItemPersistent> sourceCollection;
                    sourceCollection = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                    if (sourceCollection == null)
                    {
                        sourceCollection = new List<AuditDataItemPersistent>();
                    }
                    changeHistory = new ReadOnlyCollection<AuditDataItemPersistent>(sourceCollection);
                }
                return changeHistory;
            }
        }
        #endregion

    }

    public enum Modo
    {
        Entrada, Saida
    }

}
