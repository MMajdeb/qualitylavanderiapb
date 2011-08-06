using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacoteEmpresa;
using ERP.Lavanderia.Module.PacoteColaborador;
using ERP.Lavanderia.Module.PacoteConfiguracoes;
using ERP.Lavanderia.Module.PacoteGeral;

namespace ERP.Lavanderia.Module.PacotePessoa
{
    /// <summary>
    /// Classe utilizada para manter as matrículas utilizadas pelos colaboradores e visitantes
    /// para acesso ao sistema.
    /// </summary>
    [DefaultClassOptions]
    [DefaultProperty("NumeroMatricula")]
    [RuleObjectExists("Matricula.RuleObjectExists.ExisteNumeroAtivoPorEmpresa", DefaultContexts.Save,
        "NumeroMatricula == '@NumeroMatricula' && Empresa == '@Empresa' && Situacao == 'Ativo'",
        InvertResult = true, CustomMessageTemplate = "Já existe matrícula ativa com esse número na empresa.")]
    public class Matricula : BaseObject
    {
        private string numeroMatricula;
        private SituacaoMatricula situacao;
        private Empresa empresa;
        private bool controlaPonto;
        private Colaborador colaborador;
        private TipoPessoaAcesso tipoPessoaAcesso;
        private bool panico;
        private bool naoValidarDuploRegistro;
        private DateTime? dataExpiracaoIdentificadorFisico;

        [Size(50)]
        [Indexed(Unique = false)]
        public string NumeroMatricula
        {
            get { return numeroMatricula; }
            set
            {
                SetPropertyValue("NumeroMatricula", ref numeroMatricula, value);
            }
        }

        [RuleRequiredField("RuleRequiredField.Matricula.Ativo", DefaultContexts.Save)]
        public SituacaoMatricula Situacao
        {
            get { return situacao; }
            set { SetPropertyValue("Situacao", ref situacao, value); }
        }

        [RuleRequiredField("RuleRequiredField.Matricula.Empresa", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public Empresa Empresa
        {
            get { return empresa; }
            set
            {
                empresa = value;
                SetPropertyValue("Empresa", ref empresa, value);
            }
        }

        [NoForeignKey]
        public Colaborador Colaborador
        {
            get
            {
                return colaborador;
            }
            set
            {
                BaseObjectExtension.SetPropertyValue1to1(this, "Colaborador", ref colaborador, value, "Matricula");
            }
        }

        [NonPersistent]
        public Pessoa Pessoa
        {
            get
            {
                Pessoa pessoa = null;

                if (colaborador != null)
                {
                    pessoa = colaborador.Pessoa;
                }

                return pessoa;
            }
        }

        [Browsable(false)]
        public TipoPessoaAcesso TipoPessoaAcesso
        {
            get
            {
                return tipoPessoaAcesso;
            }
            set
            {
                SetPropertyValue("TipoPessoaAcesso", ref tipoPessoaAcesso, value);
            }
        }

        public Matricula(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.situacao = SituacaoMatricula.Ativo;

            Empresa empresa = Empresa.RetornaEmpresaAtiva(Session);
            if (empresa != null)
            {
                this.empresa = empresa;
            }
        }

        protected override void OnSaving()
        {
                TipoPessoaAcesso = TipoPessoaAcesso.Colaborador;

            base.OnSaving();
        }

        #region Metodos


        public static XPCollection PesquisaMatriculasPeloDepartamento(IObjectSpace objectSpace, string oid)
        {
            XPCollection matriculas = objectSpace.CreateCollection(typeof(Colaborador), new BinaryOperator("Departamento.Oid", oid)) as XPCollection;
            matriculas.DisplayableProperties = "Pessoa.Nome";

            //SortingCollection sortCollection = new SortingCollection();
            //sortCollection.Add(new SortProperty("Pessoa.Nome", SortingDirection.Ascending));
            //matriculas.Sorting = sortCollection;           

            return matriculas;
        }


        public static Matricula RetornaMatriculaAtivaPeloNumero(string numeroMatricula, Empresa empresa, IObjectSpace objectSpace)
        {
            CriteriaOperator crit = CriteriaOperator.Parse("NumeroMatricula = " + numeroMatricula + " and Departamento.Empresa = " + empresa);
            return objectSpace.FindObject<Matricula>(crit);
        }

        public static Matricula RetornaMatriculaAtivaPeloNumero(string numeroMatricula, Empresa empresa, Session session)
        {
            CriteriaOperator crit = CriteriaOperator.Parse("NumeroMatricula = " + numeroMatricula + " and Departamento.Empresa = " + empresa);
            return session.FindObject<Matricula>(crit);
        }

        public static Matricula RetornaMatriculaPeloNumero(string numeroMatricula, Session session)
        {
            return session.FindObject<Matricula>(new OperandProperty("NumeroMatricula") == new OperandValue(numeroMatricula));
        }

        public static Matricula RetornaMatriculaPeloNumero(string numeroMatricula, IObjectSpace objectSpace)
        {
            return objectSpace.FindObject<Matricula>(new OperandProperty("NumeroMatricula") == new OperandValue(numeroMatricula));
        }

        public static Matricula RetornaMatriculaAtivaPeloIdentificador(string identificadorFisico, Empresa empresa, Session session)
        {
            CriteriaOperator crit = new OperandProperty("IdentificadorFisicoMatricula.IdentificadorFisico") == new OperandValue(identificadorFisico) &
                new OperandProperty("Empresa") == new OperandValue(empresa);
            return session.FindObject<Matricula>(crit);
        }

        public static Matricula RetornaMatriculaAtivaPeloIdentificador(string identificadorFisico, Empresa empresa, IObjectSpace objectSpace)
        {
            CriteriaOperator crit = new OperandProperty("IdentificadorFisicoMatricula.IdentificadorFisico") == new OperandValue(identificadorFisico) &
                new OperandProperty("Empresa") == new OperandValue(empresa);

            return objectSpace.FindObject<Matricula>(crit);
        }

        public static Matricula RetornaMatriculaAtivaPeloIdentificador(string identificadorFisico, Session session)
        {
            return session.FindObject<Matricula>(
                new OperandProperty("Situacao") == new OperandValue(SituacaoMatricula.Ativo) &
                new OperandProperty("IdentificadorFisicoMatricula.IdentificadorFisico") == new OperandValue(identificadorFisico));
        }

        public static Matricula RetornaMatriculaAtivaPeloIdentificador(string identificadorFisico, IObjectSpace objectSpace)
        {
            return objectSpace.FindObject<Matricula>(
                new OperandProperty("Situacao") == new OperandValue(SituacaoMatricula.Ativo) &
                new OperandProperty("IdentificadorFisicoMatricula.IdentificadorFisico") == new OperandValue(identificadorFisico));
        }

        public static Matricula RetornaMatriculaPanico(string matricula, Session session)
        {
            return session.FindObject<Matricula>(new OperandProperty("NumeroMatricula") == new OperandValue(matricula) &
                new OperandProperty("Panico") == new OperandValue(true));
        }

        public static Matricula RetornaMatriculaPanico(string matricula, IObjectSpace objectSpace)
        {
            return objectSpace.FindObject<Matricula>(new OperandProperty("NumeroMatricula") == new OperandValue(matricula) &
                new OperandProperty("Panico") == new OperandValue(true));
        }

        public static bool VerificaSeExisteMatriculaAtivaPorPessoa(Pessoa pessoa, IObjectSpace objectSpace)
        {
            CriteriaOperator crit = GroupOperator.And(new BinaryOperator("Visitante.Pessoa", pessoa), new BinaryOperator("Situacao", SituacaoMatricula.Ativo));
            Matricula mat = objectSpace.FindObject<Matricula>(crit);

            if (mat == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }

    public enum SituacaoMatricula { Ativo = 0, Inativo = 1, Afastado = 2, Temporario = 3, Demitido = 4 };

}
