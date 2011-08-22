using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.Base.General;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ERP.Lavanderia.Module.PacoteSeguranca;
using ERP.Lavanderia.Module.PacoteColaborador;

namespace ERP.Lavanderia.Module.PacoteEmpresa
{
    /// <summary>
    /// Classe utilizada para manter os departamentos de uma determinada empresa.
    /// </summary>
    [DefaultClassOptions]
    [DefaultProperty("Nome")]
    [DeferredDeletion(false)]
    public class Departamento : BaseObject, ITreeNode
    {
        private string nome;
        private Departamento departamentoPai;
        private Empresa empresa;

        public Departamento(Session session) : base(session) { }

        [RuleUniqueValue("RuleUniqueValue Departamento.Nome", DefaultContexts.Save, @"""Nome"" já existe.")]
        [RuleRequiredField("RuleRequiredField Departamento.Nome", DefaultContexts.Save)]
        public string Nome
        {
            get { return nome; }
            set { SetPropertyValue<string>("Nome", ref nome, value); }
        }

        [Association("Empresa-Departamento", typeof(Empresa))]
        [RuleRequiredField("RuleRequiredField Departamento.Empresa", DefaultContexts.Save)]
        public Empresa Empresa
        {
            get { return empresa; }
            set { SetPropertyValue<Empresa>("Empresa", ref empresa, value); }
        }

        [Browsable(false)]
        [Association("Departamento-Departamento")]
        public Departamento DepartamentoPai
        {
            get { return departamentoPai; }
            set { SetPropertyValue<Departamento>("DepartamentoPai", ref departamentoPai, value); }
        }

        [Association("Usuario-Departamento")]
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

        [Association("Departamento-Departamento")]
        public XPCollection<Departamento> DepartamentosFilhos
        {
            get { return GetCollection<Departamento>("DepartamentosFilhos"); }
        }

        [NonPersistent]
        [Browsable(false)]
        public XPCollection<Departamento> DepartamentosFamilia
        {
            get
            {
                var ret = new XPCollection<Departamento>(this.Session);
                ret.LoadingEnabled = false;
                ret.Add(this);
                foreach (var filho in DepartamentosFilhos)
                    ret.AddRange(filho.DepartamentosFamilia);
                return ret;
            }
        }

        [NonPersistent]
        [Browsable(false)]
        public XPCollection<Departamento> EsteEOsAntepassados
        {
            get
            {
                var ret = new XPCollection<Departamento>(this.Session);
                ret.LoadingEnabled = false;
                ret.Add(this);
                if (DepartamentoPai != null)
                    ret.AddRange(DepartamentoPai.EsteEOsAntepassados);

                return ret;
            }
        }

        protected override void OnSaving()
        {
            Empresa empresa = Empresa.RetornaEmpresaAtiva(Session);
            if (empresa != null)
            {
                this.empresa = empresa;
            }
            base.OnSaving();
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Empresa empresa = Empresa.RetornaEmpresaAtiva(Session);
            if (empresa != null)
            {
                this.empresa = empresa;
            }
        }

        [RuleFromBoolProperty("Departamento.RuleFromBoolProperty.ValidaNomeUnicoEmEmpresa", DefaultContexts.Save,
    CustomMessageTemplate = "Já existe um departamento com este nome nessa empresa.")]
        [Browsable(false)]
        public bool ValidaNomeUnicoEmEmpresa
        {
            get
            {
                CriteriaOperator crit = new OperandProperty("This") != new OperandValue(this) &
                    new OperandProperty("Empresa") == new OperandValue(Empresa) & new BinaryOperator("Nome", Nome);

                return new XPCollection<Departamento>(Session, crit).Count == 0;
            }
        }

        [RuleFromBoolProperty("Departamento.RuleFromBoolProperty.ValidaDeletar", DefaultContexts.Delete,
            CustomMessageTemplate = "Esse departamento possui pessoas associados a ele e não pode ser deletado")]
        [Browsable(false)]
        public bool ValidaDeletar
        {
            get
            {
                XPCollection<Usuario> usuarios = Usuarios;
                if (usuarios != null && usuarios.Count > 0)
                {
                    return false;
                }

                XPCollection colaboradores = Colaborador.RetornaColaboradoresPorDepartamento(this.Session, this);

                if (colaboradores != null && colaboradores.Count > 0)
                {
                    return false;
                }

                return true;
            }
        }

        protected override void OnDeleting()
        {
            Departamento pai = DepartamentoPai;

            XPCollection<Departamento> filhos = DepartamentosFilhos;

            if (filhos != null)
            {

                for (int i = 0; i < filhos.Count; i++)
                {
                    Departamento filho = filhos[i];
                    filho.DepartamentoPai = pai;
                }
            }
        }

        #region ITreeNode Members
        IBindingList ITreeNode.Children
        {
            get { return DepartamentosFilhos; }
        }
        string ITreeNode.Name
        {
            get { return Nome; }
        }
        ITreeNode ITreeNode.Parent
        {
            get { return DepartamentoPai; }
        }
        #endregion

        #region Metodos

        public static XPCollection RetornaListaDepartamentosPorEmpresa(IObjectSpace objectSpace, Empresa empresa)
        {
            XPCollection departamentos = objectSpace.CreateCollection(typeof(Departamento), new BinaryOperator("Empresa", empresa)) as XPCollection;
            departamentos.DisplayableProperties = "Oid;Nome;DepartamentoPai.Oid";
            return departamentos;
        }

        public static Departamento RetornaDepartamentoPeloIdentificador(string oid, IObjectSpace objectSpace)
        {
            Departamento departamento = objectSpace.FindObject<Departamento>(new BinaryOperator("Oid", oid));
            return departamento;
        }

        public static Departamento RetornaDepartamentoPorNomeEEmpresa(string nome, Empresa empresa, IObjectSpace objectSpace)
        {
            Departamento departamento = objectSpace.FindObject<Departamento>(new BinaryOperator("Nome", nome) & new BinaryOperator("Empresa.Oid", empresa.Oid));
            return departamento;
        }

        public static Departamento RetornaDepartamentoPorNomeEEmpresa(string nome, Empresa empresa, Session session)
        {
            Departamento departamento = session.FindObject<Departamento>(new BinaryOperator("Nome", nome) & new BinaryOperator("Empresa.Oid", empresa.Oid));
            return departamento;
        }

        #endregion
    }

}
