using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacoteCliente;
using System.Drawing;

namespace ERP.Lavanderia.Module.PacoteRoupa
{
    [DefaultProperty("ToStringProperty")]
    [DefaultClassOptions]
    public class Roupa : BaseObject
    {

        private Tipo tipo;
        private Tecido tecido;
        private Tamanho tamanho;
        private Marca marca;
        private string observacao;
        private string codigo;
        private Cliente cliente;

        public Roupa(Session session)
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

        [RuleRequiredField("RuleRequiredField Roupa.Cliente", DefaultContexts.Save)]
        public Cliente Cliente
        {
            get { return cliente; }
            set {
                SetPropertyValue("Cliente", ref cliente, value); 
            }
        }

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        [RuleRequiredField("RuleRequiredField Roupa.Tipo", DefaultContexts.Save)]
        public Tipo Tipo
        {
            get { return tipo; }
            set {
                SetPropertyValue("Tipo", ref tipo, value); 
            }
        }

        [Association("Roupa-Cores")]
        public XPCollection<Cor> Cores
        {
            get { return GetCollection<Cor>("Cores"); }
        }

        public Tecido Tecido
        {
            get { return tecido; }
            set {
                SetPropertyValue("Tecido", ref tecido, value);  
            }
        }

        public Marca Marca
        {
            get { return marca; }
            set {
                SetPropertyValue("Marca", ref marca, value);  
            }
        }

        public Tamanho Tamanho
        {
            get { return tamanho; }
            set {
                SetPropertyValue("Tamanho", ref tamanho, value);  
            }
        }

        public string Observacao
        {
            get { return observacao; }
            set {
                SetPropertyValue("Observacao", ref observacao, value); 
            }
        }

        [NonPersistent]
        [System.ComponentModel.Browsable(false)]
        [RuleFromBoolProperty("RuleFromBoolProperty Roupa.ValidaCodigo", DefaultContexts.Save, @"O código da roupa deve ser único entre todas as outras ou igual ao código de seu dono.")]
        public bool ValidaRoupasDoCliente
        {
            get
            {
                if (Codigo == null || Cliente == null)
                    return true;

                if (Codigo.Equals(Cliente.Codigo)) {
                    return true;
                }

                CriteriaOperator crit = new OperandProperty("This") != new OperandValue(this) & new BinaryOperator("Codigo", Codigo);

                return new XPCollection<Roupa>(Session, crit).Count == 0;
            }
        }

        private string ToStringCores() {
            string coresString = "";

            if (Cores != null && Cores.Count > 0)
            {
                coresString = Cores[0].Descricao;

                for (int i = 1; i < Cores.Count; i++) {
                    coresString += ", " + Cores[i].Descricao;
                }
            }

            return coresString;

        }

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
            return "(" + Codigo + ") " + Tipo + "; " + Tecido + "; " + Marca + "; " +
                Tamanho + "; " + ToStringCores() + "; " + Observacao;
        }
    }

}
