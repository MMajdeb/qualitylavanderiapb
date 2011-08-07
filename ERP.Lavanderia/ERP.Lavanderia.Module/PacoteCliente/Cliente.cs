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
using DevExpress.ExpressApp.ConditionalEditorState;
using ERP.Lavanderia.Module.PacoteSeguranca;

namespace ERP.Lavanderia.Module.PacoteCliente
{
    [DefaultProperty("Pessoa.Nome")]
    [DefaultClassOptions]
    [EditorStateRuleAttribute("Cliente.DesativaUltimaAlteracaoEDataCadastro", "DataUltimaAtualizacao,DataCadastro", EditorState.Disabled, "true", ViewType.DetailView)]
    public class Cliente : BaseObject
    {
        private Pessoa pessoa;
        private DateTime dataCadastro;
        private DateTime dataUltimaAtualizacao;
        private string observacoes;
    ﻿    private Classificacao classificacao;
        private string codigo;
        private Usuario usuario;
        private string senha;
        private string nomeDeUsuario;

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

            DataCadastro = System.DateTime.Now;

            Usuario = new Usuario(Session);
            Usuario.UserName = DateTime.Now.ToString();
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


        [RuleRequiredField("Cliente.RuleRequiredField.Senha", DefaultContexts.Save)]
        public string Senha
        {
            get { return senha; }
            set
            {
                SetPropertyValue("Senha", ref senha, value);
            }
        }

        [RuleRequiredField("Cliente.RuleRequiredField.NomeDeUsuario", DefaultContexts.Save)]
        public string NomeDeUsuario
        {
            get { return nomeDeUsuario; }
            set
            {
                SetPropertyValue("NomeDeUsuario", ref nomeDeUsuario, value);
            }
        }

        [Browsable(false)]
        [RuleUniqueValue("Cliente.Usuario", DefaultContexts.Save, @"""Usuário"" já cadastrada a outra pessoa.")]
        [RuleRequiredField("Cliente.RuleRequiredField.Usuario", DefaultContexts.Save)]
        public Usuario Usuario
        {
            get { return usuario; }
            set { SetPropertyValue("Usuario", ref usuario, value); }
        }

        protected override void OnSaving()
        {
            this.DataUltimaAtualizacao = DateTime.Now;

            Usuario.SetPassword(Senha);
            Usuario.UserName = NomeDeUsuario;

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

        [RuleFromBoolProperty("Cliente.RuleFromBoolProperty.ValidaUsuario", DefaultContexts.Save,
    CustomMessageTemplate = "Já existe um usuario com esse nome de usuário")]
        [Browsable(false)]
        public bool ValidaUsuario
        {
            get
            {
                var user = Usuario.RetornaUsuarioPorNomeDeUsuario(Session, NomeDeUsuario);

                return user == null || user.Equals(Usuario);
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
