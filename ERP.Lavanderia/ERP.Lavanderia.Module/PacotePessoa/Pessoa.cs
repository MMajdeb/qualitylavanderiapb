using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalEditorState;
using ERP.Lavanderia.Module.PacoteEndereco;
using ERP.Lavanderia.Module.RecursosHumanos;
using ERP.Lavanderia.Module.PacoteUtil;
using System.Drawing;
using ERP.Lavanderia.Module.PacoteEmpresa;
using ERP.Lavanderia.Module.PacoteColaborador;
using System.Collections;
using DevExpress.Xpo.Metadata;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ERP.Lavanderia.Module.PacoteCliente;

namespace ERP.Lavanderia.Module.PacotePessoa
{
    /// <summary>
    /// Classe utilizada para manter as pessoas físicas ou jurídicas de forma genérica no sistema.
    /// </summary>
    [DefaultProperty("Nome")]
    [DefaultClassOptions]
    [EditorStateRuleAttribute("DesativaDadosPessoaJuridica", "NomeFantasia;InscricaoEstadual;InscricaoMunicipal;DataAbertura", EditorState.Disabled, "TipoPessoa='Fisica'", ViewType.DetailView)]
    [EditorStateRuleAttribute("DesativaDadosPessoaFisica", "Rg;OrgaoExpeditorRg;DataNascimento;Sexo;EstadoCivil;Profissao;NomePai;NomeMae;Pis", EditorState.Disabled, "TipoPessoa='Juridica'", ViewType.DetailView)]
    public class Pessoa : BaseObject
    {
        private TipoPessoa tipoPessoa;
        private string nome;
        private string nomeFantasia;
        private string cpfCnpj;
        private string rg;
        private string orgaoExpeditorRg;
        private string inscricaoEstadual;
        private string inscricaoMunicipal;
        private DateTime dataNascimento;
        private DateTime dataAbertura;
        private Endereco enderecoPrincipal;
        private string enderecoWeb;
        private string email;
        private string email2;
        private string telefoneConvencional1;
        private string telefoneConvencional2;
        private string telefoneMovel;
        private string fax;
        private string enderecoIm;
        private SexoPessoa sexo;
        private EstadoCivilPessoa estadoCivil;
        private Profissao profissao;
        private string nomePai;
        private string nomeMae;
        private string pis;
        private string cei;

        public Pessoa(Session session) : base(session) { }

        [ImmediatePostData]
        public TipoPessoa TipoPessoa
        {
            get { return tipoPessoa; }
            set
            {
                SetPropertyValue("TipoPessoa", ref tipoPessoa, value);
            }
        }

        [Size(100)]
        [RuleRequiredField("RuleRequiredField Pessoa.Nome", DefaultContexts.Save)]
        public string Nome
        {
            get { return nome; }
            set
            {
                SetPropertyValue("Nome", ref nome, value);
            }
        }

        [Size(100)]
        public string NomeFantasia
        {
            get { return nomeFantasia; }
            set
            {
                SetPropertyValue("NomeFantasia", ref nomeFantasia, value);
            }
        }

        [Size(18)]
        [RuleUniqueValue("Pessoa.CpfCnpjUnico", DefaultContexts.Save, @"""CPF/CNPJ"" já existe.")]
        public string CpfCnpj
        {
            get { return cpfCnpj; }
            set
            {
                SetPropertyValue("CpfCnpj", ref cpfCnpj, value);
            }
        }

        /// <summary>
        /// Propriedade utilizada para validar um CPF ou CNPJ dependendo do tipo de Pessoa.
        /// </summary>
        [NonPersistent]
        [System.ComponentModel.Browsable(false)]
        [RuleFromBoolProperty("Pessoa.CpfCnpjValido", DefaultContexts.Save, @"""CPF/CNPJ"" inválido.")]
        public bool CpfCnpjValido
        {
            get
            {
                if (tipoPessoa == TipoPessoa.Fisica)
                {
                    if (cpfCnpj != null)
                    {
                        if (StringUtils.CleanCpfCnpj(cpfCnpj) != "")
                        {
                            return Validacoes.ValidateCpf(cpfCnpj);
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (cpfCnpj != null)
                    {
                        if (StringUtils.CleanCpfCnpj(cpfCnpj) != "")
                        {
                            return Validacoes.ValidateCnpj(cpfCnpj);
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        [Size(30)]
        public string Rg
        {
            get { return rg; }
            set
            {
                SetPropertyValue("Rg", ref rg, value);
            }
        }

        [Size(50)]
        public string OrgaoExpeditorRg
        {
            get { return orgaoExpeditorRg; }
            set
            {
                SetPropertyValue("OrgaoExpeditorRg", ref orgaoExpeditorRg, value);
            }
        }

        [Size(25)]
        public string InscricaoEstadual
        {
            get { return inscricaoEstadual; }
            set
            {
                SetPropertyValue("InscricaoEstadual", ref inscricaoEstadual, value);
            }
        }

        [Size(25)]
        public string InscricaoMunicipal
        {
            get { return inscricaoMunicipal; }
            set
            {
                SetPropertyValue("InscricaoMunicipal", ref inscricaoMunicipal, value);
            }
        }
        
        public DateTime DataNascimento
        {
            get { return dataNascimento; }
            set
            {
                SetPropertyValue("DataNascimento", ref dataNascimento, value);
            }
        }

        public DateTime DataAbertura
        {
            get { return dataAbertura; }
            set
            {
                SetPropertyValue("DataAbertura", ref dataAbertura, value);
            }
        }

        [ValueConverter(typeof(DevExpress.Xpo.Metadata.ImageValueConverter)), Delayed]
        public Image Imagem
        {
            get { return GetDelayedPropertyValue<Image>("Imagem"); }
            set
            {
                SetDelayedPropertyValue<Image>("Imagem", value);
            }
        }

        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public Endereco EnderecoPrincipal
        {
            get { return enderecoPrincipal; }
            set
            {
                SetPropertyValue("EnderecoPrincipal", ref enderecoPrincipal, value);
            }
        }

        [Size(255)]
        public string EnderecoWeb
        {
            get { return enderecoWeb; }
            set
            {
                SetPropertyValue("EnderecoWeb", ref enderecoWeb, value);
            }
        }

        [Size(255)]
        public string Email
        {
            get { return email; }
            set
            {
                SetPropertyValue("Email", ref email, value);
            }
        }

        [Size(255)]
        public string Email2
        {
            get { return email2; }
            set
            {
                SetPropertyValue("Email2", ref email2, value);
            }
        }

        [Size(16)]
        public string TelefoneConvencional1
        {
            get { return telefoneConvencional1; }
            set
            {
                SetPropertyValue("TelefoneConvencional1", ref telefoneConvencional1, value);
            }
        }

        [Size(16)]
        public string TelefoneConvencional2
        {
            get { return telefoneConvencional2; }
            set
            {
                SetPropertyValue("TelefoneConvencional2", ref telefoneConvencional2, value);
            }
        }

        [Size(16)]
        public string TelefoneMovel
        {
            get { return telefoneMovel; }
            set
            {
                SetPropertyValue("TelefoneMovel", ref telefoneMovel, value);
            }
        }

        [Size(16)]
        public string Fax
        {
            get { return fax; }
            set
            {
                SetPropertyValue("Fax", ref fax, value);
            }
        }

        [Size(255)]
        public string EnderecoIm
        {
            get { return enderecoIm; }
            set
            {
                SetPropertyValue("EnderecoIm", ref enderecoIm, value);
            }
        }

        public SexoPessoa Sexo
        {
            get { return sexo; }
            set
            {
                SetPropertyValue("Sexo", ref sexo, value);
            }
        }

        public EstadoCivilPessoa EstadoCivil
        {
            get { return estadoCivil; }
            set
            {
                SetPropertyValue("EstadoCivil", ref estadoCivil, value);
            }
        }

        public Profissao Profissao
        {
            get { return profissao; }
            set
            {
                SetPropertyValue("Profissao", ref profissao, value);
            }
        }

        [Size(100)]
        public string NomePai
        {
            get { return nomePai; }
            set
            {
                SetPropertyValue("NomePai", ref nomePai, value);
            }
        }

        [Size(100)]
        public string NomeMae
        {
            get { return nomeMae; }
            set
            {
                SetPropertyValue("NomeMae", ref nomeMae, value);
            }
        }

        [Size(14)]
        [RuleUniqueValue("Pessoa.Pis", DefaultContexts.Save, @"""PIS"" já existe.")]
        [Indexed(Unique = false)]
        public string Pis
        {
            get { return pis; }
            set
            {
                SetPropertyValue("Pis", ref pis, value);
            }
        }

        /// <summary>
        /// Número CEI (Cadastro Específico do INSS) da empressa/pessoa física.
        /// Formatação padrão: ##.###.#####/##
        /// </summary>
        [Size(15)]
        [RuleUniqueValue("Pessoa.Cei", DefaultContexts.Save, @"""CEI"" já existe.")]
        public string Cei
        {
            get { return cei; }
            set
            {
                SetPropertyValue("Cei", ref cei, value);
            }
        }

        /// <summary>
        /// Propriedade utilizada para validar o número do PIS.
        /// </summary>
        [NonPersistent]
        [System.ComponentModel.Browsable(false)]
        [RuleFromBoolProperty("Pessoa.PisValido", DefaultContexts.Save, @"""PIS"" inválido.")]
        public bool PisValido
        {
            get
            {
                if (pis != null)
                {
                    if (StringUtils.CleanCpfCnpj(pis) != "")
                    {
                        return Validacoes.ValidatePis(pis);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Propriedade utilizada para validar o número do CEI.
        /// </summary>
        [NonPersistent]
        [System.ComponentModel.Browsable(false)]
        [RuleFromBoolProperty("Pessoa.CeiValido", DefaultContexts.Save, @"""CEI"" inválido.")]
        public bool CeiValido
        {
            get
            {
                if (tipoPessoa == TipoPessoa.Juridica && cei != null && !cei.Trim().Equals(""))
                {
                    if (StringUtils.CleanCpfCnpj(cei).Trim() != "")
                    {
                        return Validacoes.ValidateCei(cei);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        [Browsable(false)]
        [Association("Pessoa-Empresa", typeof(Empresa))]
        public XPCollection Empresas
        {
            get { return GetCollection("Empresas"); }
        }

        [Browsable(false)]
        [Association("Pessoa-Colaborador", typeof(Colaborador))]
        public XPCollection Colaboradores
        {
            get { return GetCollection("Colaboradores"); }
        }

        [Browsable(false)]
        [Association("Pessoa-Cliente", typeof(Cliente))]
        public XPCollection Clientes
        {
            get { return GetCollection("Clientes"); }
        }

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

        #region Metodos

        /// <summary>
        /// Retorna um objeto Pessoa pelo CPF/CNPJ passado.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="strCpfCnpj"></param>
        /// <returns></returns>
        public static Pessoa RetornaPessoaPorCpfCnpj(Session session, string strCpfCnpj)
        {
            Pessoa result = session.FindObject<Pessoa>(new BinaryOperator("CpfCnpj", strCpfCnpj));

            return result;
        }

        #endregion
    }

    public enum TipoPessoa { Fisica, Juridica };

    public enum SexoPessoa { Masculino, Feminino };

    public enum EstadoCivilPessoa { Solteiro, Casado, Separado, Divorciado, Viuvo };

    /// <summary>
    /// Define o tipo de pessoa em um determinado evento ou ocorrência.
    /// </summary>
    public enum TipoPessoaAcesso
    {
        Colaborador = 1,
        Visitante = 2
    }

}
