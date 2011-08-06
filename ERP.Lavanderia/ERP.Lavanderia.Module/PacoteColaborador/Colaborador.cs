using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalEditorState;
using ERP.Lavanderia.Module.RecursosHumanos;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Xpo.Metadata;
using ERP.Lavanderia.Module.PacotePessoa;
using ERP.Lavanderia.Module.PacoteRecursosHumanos;
using ERP.Lavanderia.Module.PacoteEmpresa;
using ERP.Lavanderia.Module.PacoteGeral;

namespace ERP.Lavanderia.Module.PacoteColaborador
{
    /// <summary>
    /// Classe utilizada para manter os colaboradores (funcionários).
    /// </summary>
    [DefaultClassOptions]
    [DefaultProperty("Pessoa.Nome")]
    [EditorStateRuleAttribute("DesativaDadosAdmitido", "DataDemissao", EditorState.Disabled, "Matricula.Situacao != 4", ViewType.DetailView)]
    public class Colaborador : BaseObject
    {
        private Pessoa pessoa;
        private DateTime dataCadastro;
        private DateTime dataUltimaAtualizacao;
        private ColaboradorCargo cargo;
        private Matricula matricula;
        private string observacoes;
        private Departamento departamento;
        private string senhaAcesso;
        private bool alterarSenha;
        private bool desativapainel;
        private DateTime dataAdmissao;
        private DateTime? dataDemissao;

        public Colaborador(Session session) : base(session) { }

        [RuleFromBoolProperty("RuleFromBoolProperty.Colaborador.PessoaUnicaPorEmpresa", DefaultContexts.Save,
    CustomMessageTemplate = "Esta pessoa já está registrada como colaborador.")]
        private bool PessoaUnicaPorEmpresa
        {
            get
            {
                Colaborador col = RetornaColaboradorPorPessoaEEmpresa(Pessoa, Departamento.Empresa, Session);
                return col == null || col.Equals(this);
            }
        }

        [RuleRequiredField("RuleRequiredField Colaborador.Pessoa", DefaultContexts.Save)]
        [DataSourceCriteria("TipoPessoa=0")]
        [ImmediatePostData]
        [Association("Pessoa-Colaborador", typeof(Pessoa))]
        [ExpandObjectMembers(ExpandObjectMembers.InListView)]
        public Pessoa Pessoa
        {
            get { return pessoa; }
            set
            {
                SetPropertyValue("Pessoa", ref pessoa, value);
            }
        }

        [NonPersistent]
        [System.ComponentModel.Browsable(false)]
        [RuleFromBoolProperty("Colaborador.PessoaValida", DefaultContexts.Save, @"Colaborador deve ser pessoa física.")]
        public bool PessoaValida
        {
            get
            {
                if (pessoa != null)
                {
                    if (pessoa.TipoPessoa == TipoPessoa.Fisica)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Always)]
        [RuleRequiredField("Colaborador.RuleRequiredField.Matricula", DefaultContexts.Save)]
        public Matricula Matricula
        {
            get
            {
                return matricula;
            }
            set
            {
                BaseObjectExtension.SetPropertyValue1to1(this, "Matricula", ref matricula, value, "Colaborador");
            }
        }

        [RuleRequiredField("RuleRequiredField Colaborador.DataCadastro", DefaultContexts.Save)]
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

        /// <summary>
        /// Define o cargo do colaborador.
        /// </summary>
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public ColaboradorCargo ColaboradorCargo
        {
            get { return cargo; }
            set
            {
                SetPropertyValue("ColaboradorCargo", ref cargo, value);
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

        [RuleRequiredField("RuleRequiredField.Colaborador.Departamento", DefaultContexts.Save)]
        public Departamento Departamento
        {
            get { return departamento; }
            set
            {
                SetPropertyValue("Departamento", ref departamento, value);
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

        public bool DesativarPainelColaborador
        {
            get { return desativapainel; }
            set { SetPropertyValue("DesativarPainelColaborador", ref desativapainel, value); }
        }

        [RuleRequiredField("Colaborador.RuleRequiredField.DataAdmissao", DefaultContexts.Save)]
        public DateTime DataAdmissao
        {
            get { return dataAdmissao; }
            set { SetPropertyValue("DataAdmissao", ref dataAdmissao, value); }
        }

        public DateTime? DataDemissao
        {
            get { return dataDemissao; }
            set { SetPropertyValue("DataDemissao", ref dataDemissao, value); }
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
        }

        protected override void OnSaving()
        {
            this.DataUltimaAtualizacao = DateTime.Now;

            base.OnSaving();
        }

        [RuleFromBoolProperty("Colaborador.RuleFromBoolProperty.ValidaDeletar", DefaultContexts.Delete,
            CustomMessageTemplate = "Esse colaborador possui associações e não pode ser deletado")]
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

        public static XPCollection RetornaColaboradoresComPIS(Session session, Guid oidEmpresa)
        {
            XPCollection colaboradores = new XPCollection(typeof(Colaborador),
                new OperandProperty("Matricula.Empresa.Oid") == new OperandValue(oidEmpresa) &
                !new FunctionOperator(FunctionOperatorType.IsNullOrEmpty, new OperandProperty("Pessoa.Pis")));

            return colaboradores;
        }

        public static XPCollection RetornaColaboradoresComPIS(Session session, Empresa empresa)
        {
            XPCollection colaboradores = new XPCollection(typeof(Colaborador),
                new OperandProperty("Matricula.Empresa") == new OperandValue(empresa) &
                !new FunctionOperator(FunctionOperatorType.IsNullOrEmpty, new OperandProperty("Pessoa.Pis")));

            return colaboradores;
        }

        public static XPCollection RetornaColaboradoresComPIS(IObjectSpace objectSpace, Empresa empresa)
        {
            XPCollection colaboradores = objectSpace.CreateCollection(typeof(Colaborador),
                new OperandProperty("Matricula.Empresa") == new OperandValue(empresa) &
                !new FunctionOperator(FunctionOperatorType.IsNullOrEmpty, new OperandProperty("Pessoa.Pis")))
                as XPCollection;

            return colaboradores;
        }

        public static XPCollection RetornaColaboradores(IObjectSpace objectSpace, Empresa empresa)
        {
            XPCollection colaboradores = objectSpace.CreateServerCollection(typeof(Colaborador), new BinaryOperator("Matricula.Empresa", empresa)) as XPCollection;

            if (colaboradores != null)
            {
                colaboradores.DisplayableProperties = "Oid;Pessoa.Nome";
            }

            return colaboradores;
        }

        public static XPCollection RetornaColaboradoresPorDepartamento(IObjectSpace objectSpace, Departamento departamento)
        {
            return objectSpace.CreateServerCollection(typeof(Colaborador), new BinaryOperator("Departamento", departamento)) as XPCollection;
        }

        public static XPCollection RetornaColaboradoresPorDepartamento(Session session, Departamento departamento)
        {
            return new XPCollection(session, typeof(Colaborador), new BinaryOperator("Departamento", departamento));
        }

        public static Colaborador RetornaColaboradorPorMatricula(Matricula matricula, IObjectSpace objectSpace)
        {
            return objectSpace.FindObject<Colaborador>(new OperandProperty("Matricula") == matricula);
        }

        public static Colaborador RetornaColaboradorPorMatricula(Matricula matricula, Session session)
        {
            return session.FindObject<Colaborador>(new OperandProperty("Matricula") == matricula);
        }

        public static Colaborador RetornaColaboradorPorPessoaEEmpresa(Pessoa pessoa, Empresa empresa, Session session)
        {
            return session.FindObject<Colaborador>(new OperandProperty("Pessoa") == pessoa
                & new OperandProperty("Departamento.Empresa") == empresa);
        }

        #endregion
    }

}
