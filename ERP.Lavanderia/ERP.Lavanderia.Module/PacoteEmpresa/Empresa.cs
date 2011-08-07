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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ERP.Lavanderia.Module.PacotePessoa;
using ERP.Lavanderia.Module.PacoteConfiguracoes;
using ERP.Lavanderia.Module.PacoteSeguranca;
using ERP.Lavanderia.Module.PacoteControladoress.ControladoresEmpresa;

namespace ERP.Lavanderia.Module.PacoteEmpresa
{
    /// <summary>
    /// Classe utilizada para manter as Empresas (usuárias) do sistema.
    /// </summary>
    [DefaultClassOptions]
    [DefaultProperty("Pessoa.Nome")]
    public class Empresa : BaseObject
    {
        private Pessoa pessoa;
        private DateTime dataCadastro;
        private string observacoes;
        private string cabecalhoRelatorio;
        private ConfiguracaoEmpresa configuracao;

        public Empresa(Session session) : base(session) { }

        [RuleUniqueValue("Empresa.PessoaUnica", DefaultContexts.Save, @"Esta pessoa já está registrada como empresa.")]
        [RuleRequiredField("RuleRequiredField Empresa.Pessoa", DefaultContexts.Save)]
        [ImmediatePostData]
        [Association("Pessoa-Empresa", typeof(Pessoa))]
        [ExpandObjectMembers(ExpandObjectMembers.InListView)]
        public Pessoa Pessoa
        {
            get { return pessoa; }
            set
            {
                SetPropertyValue("Pessoa", ref pessoa, value);
                OnChanged("Pessoa");
            }
        }

        public DateTime DataCadastro
        {
            get { return dataCadastro; }
            set
            {
                SetPropertyValue("DataCadastro", ref dataCadastro, value);
                OnChanged("DataCadastro");
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
                OnChanged("Observacoes");
            }
        }

        /// <summary>
        /// Coleção de departamentos da empresa.
        /// </summary>
        [Association("Empresa-Departamento", typeof(Departamento))]
        [Browsable(false)]
        public XPCollection Departamentos
        {
            get { return GetCollection("Departamentos"); }
        }

        [Size(4096)]
        public string CabecalhoRelatorio
        {
            get { return cabecalhoRelatorio; }
            set
            {
                SetPropertyValue("CabecalhoRelatorio", ref cabecalhoRelatorio, value);
                OnChanged("CabecalhoRelatorio");
            }
        }

        /// <summary>
        /// Define o objeto ConfiguracaoEmpresa para manter as configurações específicas de uma empresa.
        /// </summary>
        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public ConfiguracaoEmpresa Configuracao
        {
            get { return configuracao; }
            set
            {
                SetPropertyValue("Configuracao", ref configuracao, value);
                OnChanged("Configuracao");
            }
        }

        [Association("Usuario-Empresas")]
        public XPCollection<Usuario> Usuarios
        {
            get { return GetCollection<Usuario>("Usuarios"); }
        }

        #region Modulo Auditoria
        //Módulo de auditoria
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

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //Define o valor padrão da data de cadastro
            dataCadastro = System.DateTime.Now;
            this.Configuracao = new ConfiguracaoEmpresa(Session);
        }

        /// <summary>
        /// Exclui os objetos relacionados se for uma coleção ou associação.
        /// </summary>
        protected override void OnDeleting()
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
                                //throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                                Session.Delete(obj);
                            }
                        }
                    }
                }
            }
            base.OnDeleting();
        }

        #region Metodos
        /// <summary>
        /// Retona a empresa configurada como ativa na sessão atual.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static Empresa RetornaEmpresaAtiva(IObjectSpace os)
        {
            string empresaOid = SelecaoEmpresaController.Instance().EmpresaPadraoOid;
            if (empresaOid != "")
            {
                Empresa empresa = os.FindObject<Empresa>(new BinaryOperator("Oid", empresaOid));
                return empresa;
            }
            else
            {
                return null;
            }
        }

        public static Empresa RetornaEmpresaAtiva(Session session)
        {

            //string empresaOid = "";
            //if (SelecaoEmpresaController.Instance() != null)
            //    empresaOid = SelecaoEmpresaController.Instance().EmpresaPadraoOid;

            //if (empresaOid != "")
            //{
            //    Empresa empresa = session.FindObject<Empresa>(new BinaryOperator("Oid", empresaOid));
            //    return empresa;
            //}
            //else
            //{
            //    return null;
            //}

            return RetornaEmpresa(session);
        }
        #endregion

        internal static Empresa RetornaEmpresa(DevExpress.Xpo.Session session)
        {
            var col = new XPCollection<Empresa>(session);

            if (col == null || col.Count == 0)
                return null;

            return col[0];
        }
    }

}
