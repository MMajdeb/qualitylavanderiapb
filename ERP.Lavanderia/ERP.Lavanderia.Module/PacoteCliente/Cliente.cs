using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacotePessoa;
using System.Collections;
using DevExpress.Xpo.Metadata;

namespace ERP.Lavanderia.Module.PacoteCliente
{
    [DefaultClassOptions]
    public class Cliente : BaseObject
    {
        private Pessoa pessoa;
        private DateTime dataCadastro;
        private DateTime dataUltimaAtualizacao;
        private string observacoes;
        private string senhaAcesso;
        private bool alterarSenha;
    ﻿    private Classificacao classificacao;
        private string codigo;

        public Cliente(Session session)
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

        [RuleUniqueValue("Cliente.CodigoUnico", DefaultContexts.Save, @"""Código"" já existe.")]
        public string Codigo
        {
            get { return codigo; }
            set {
                SetPropertyValue("Codigo", ref codigo, value); 
            }
        }

        public Classificacao Classificacao
        {
            get { return classificacao; }
            set {
                SetPropertyValue("Classificacao", ref classificacao, value); 
            }
        }

        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(4096)]
        public string Observacoes
        {
            get { return observacoes; }
            set
            {
                SetPropertyValue("Observacoes", ref observacoes, value);
            }
        }

        [RuleRequiredField("RuleRequiredField Cliente.Pessoa", DefaultContexts.Save)]
        [ImmediatePostData]
        [Association("Pessoa-Cliente", typeof(Pessoa))]
        [ExpandObjectMembers(ExpandObjectMembers.InListView)]
        public Pessoa Pessoa
        {
            get { return pessoa; }
            set
            {
                SetPropertyValue("Pessoa", ref pessoa, value);
            }
        }

        [RuleFromBoolProperty("RuleFromBoolProperty.Cliente.PessoaUnicaPorEmpresa", DefaultContexts.Save,
CustomMessageTemplate = "Esta pessoa já está registrada como cliente.")]
        private bool PessoaUnicaPorEmpresa
        {
            get
            {
                Cliente col = RetornaClientePorPessoa(Pessoa, Session);
                return col == null || col.Equals(this);
            }
        }

        [RuleRequiredField("RuleRequiredField Cliente.DataCadastro", DefaultContexts.Save)]
        public DateTime DataCadastro
        {
            get { return dataCadastro; }
            set
            {
                SetPropertyValue("DataCadastro", ref dataCadastro, value);
            }
        }

        public DateTime DataUltimaAtualizacao
        {
            get
            {
                return dataUltimaAtualizacao;
            }
            set
            {
                SetPropertyValue("DataUltimaAtualizacao", ref dataUltimaAtualizacao, value);
            }
        }

        [Size(50)]
        public string SenhaAcesso
        {
            get { return senhaAcesso; }
            set { SetPropertyValue("SenhaAcesso", ref senhaAcesso, value); }
        }

        public bool AlterarSenhaAcesso
        {
            get { return alterarSenha; }
            set { SetPropertyValue("AlterarSenhaAcesso", ref alterarSenha, value); }
        }

        protected override void OnSaving()
        {
            this.DataUltimaAtualizacao = DateTime.Now;

            base.OnSaving();
        }

        [RuleFromBoolProperty("Cliente.RuleFromBoolProperty.ValidaDeletar", DefaultContexts.Delete,
            CustomMessageTemplate = "Esse cliente possui associações e não pode ser deletado")]
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

        #region Metodos

        public static XPCollection RetornaClientes(IObjectSpace objectSpace)
        {
            XPCollection clientes = objectSpace.CreateServerCollection(typeof(Cliente), null) as XPCollection;

            if (clientes != null)
            {
                clientes.DisplayableProperties = "Oid;Pessoa.Nome";
            }

            return clientes;
        }

        public static Cliente RetornaClientePorPessoa(Pessoa pessoa, Session session)
        {
            return session.FindObject<Cliente>(new OperandProperty("Pessoa") == pessoa);
        }

        #endregion
    }

    public enum Classificacao { 
        Bronze, Prata, Ouro
    }

}
