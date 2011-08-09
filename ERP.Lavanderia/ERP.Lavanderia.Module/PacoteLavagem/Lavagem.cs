using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacoteCliente;
using ERP.Lavanderia.Module.PacoteEmpresa;
using ERP.Lavanderia.Module.PacoteColaborador;
using ERP.Lavanderia.Module.PacoteCaixa;

namespace ERP.Lavanderia.Module.PacoteLavagem
{
    [DefaultProperty("ToStringProperty")]
    [DefaultClassOptions]
    public class Lavagem : BaseObject
    {
        private Cliente cliente;
        private DateTime dataHoraDeRecebimento;
        private DateTime dataHoraPreferivelParaEntrega;
        private DateTime dataHoraEntrega;
        private bool entregaEmCasa;
        private Colaborador recebedor;
        private Colaborador anotador;
        private Colaborador lavador;
        private Colaborador passador;
        private Colaborador entregador;
        private PontoDeColeta pontoDeColetaDeRecebimento;
        private PontoDeColeta pontoDeColetaParaEntrega;
        private decimal valor;
        private MovimentacaoCaixa movimentacaoCaixa;

        public Lavagem(Session session)
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

            DataHoraDeRecebimento = DateTime.Now;
            DataHoraPreferivelParaEntrega = DataHoraDeRecebimento.AddDays(2);
        }

        [RuleRequiredField("RuleRequiredField Lavagem.Cliente", DefaultContexts.Save)]
        public Cliente Cliente
        {
            get { return cliente; }
            set
            {
                SetPropertyValue("Cliente", ref cliente, value);
            }
        }

        [ImmediatePostData]
        public Colaborador Recebedor
        {
            get { return recebedor; }
            set {
                SetPropertyValue("Recebedor", ref recebedor, value);
            }
        }

        [ImmediatePostData]
        public Colaborador Anotador
        {
            get { return anotador; }
            set
            {
                SetPropertyValue("Anotador", ref anotador, value);
            }
        }

        [ImmediatePostData]
        public Colaborador Lavador
        {
            get { return lavador; }
            set
            {
                SetPropertyValue("Lavador", ref lavador, value);
            }
        }

        [ImmediatePostData]
        public Colaborador Passador
        {
            get { return passador; }
            set
            {
                SetPropertyValue("Passador", ref passador, value);
            }
        }

        [ImmediatePostData]
        public Colaborador Entregador
        {
            get { return entregador; }
            set
            {
                SetPropertyValue("Entregador", ref entregador, value);
            }
        }


        public bool EntregaEmCasa
        {
            get { return entregaEmCasa; }
            set {
                SetPropertyValue("EntregaEmCasa", ref entregaEmCasa, value);
            }
        }

        [RuleRequiredField("RuleRequiredField Lavagem.DataHoraDeRecebimento", DefaultContexts.Save)]
        public DateTime DataHoraDeRecebimento
        {
            get { return dataHoraDeRecebimento; }
            set {
                SetPropertyValue("DataHoraDeRecebimento", ref dataHoraDeRecebimento, value);
            }
        }

        [RuleRequiredField("RuleRequiredField Lavagem.DataHoraPreferivelParaEntrega", DefaultContexts.Save)]
        public DateTime DataHoraPreferivelParaEntrega
        {
            get { return dataHoraPreferivelParaEntrega; }
            set
            {
                SetPropertyValue("DataHoraPreferivelParaEntrega", ref dataHoraPreferivelParaEntrega, value);
            }
        }

        public DateTime DataHoraEntrega
        {
            get { return dataHoraEntrega; }
            set
            {
                SetPropertyValue("DataHoraEntrega", ref dataHoraEntrega, value);
            }
        }

        public PontoDeColeta PontoDeColetaDeRecebimento
        {
            get { return pontoDeColetaDeRecebimento; }
            set {
                SetPropertyValue("PontoDeColetaDeRecebimento", ref pontoDeColetaDeRecebimento, value);
            }
        }

        public PontoDeColeta PontoDeColetaParaEntrega
        {
            get { return pontoDeColetaParaEntrega; }
            set
            {
                SetPropertyValue("PontoDeColetaParaEntrega", ref pontoDeColetaParaEntrega, value);
            }
        }

        public decimal Valor
        {
            get { return valor; }
            set { SetPropertyValue("Valor", ref valor, value); }
        }

        [Association("Lavagem-RoupaLavagem")]
        public XPCollection<RoupaLavagem> Roupas
        {
            get { return GetCollection<RoupaLavagem>("Roupas"); }
        }

        [RuleUniqueValue("Lavagem.MovimentacaoCaixa", DefaultContexts.Save, @"""Movimentação"" já associada a outra lavagem ou entidade.")]
        public MovimentacaoCaixa MovimentacaoCaixa
        {
            get { return movimentacaoCaixa; }
            set { SetPropertyValue("MovimentacaoCaixa", ref movimentacaoCaixa, value); }
        }

        [NonPersistent]
        [System.ComponentModel.Browsable(false)]
        [RuleFromBoolProperty("RuleFromBoolProperty Lavagem.ValidaRoupasDoCliente", DefaultContexts.Save, @"Você cadastrou roupas que não são desse cliente.")]
        public bool ValidaRoupasDoCliente
        {
            get
            {
                if (Cliente == null)
                    return false;

                if (Roupas == null)
                    return true;

                foreach (RoupaLavagem rl in Roupas) {
                    if (!Cliente.Equals(rl.Roupa.Cliente))
                        return false;
                }

                return true;
            }
        }

        [Browsable(false)]
        [NonPersistent]
        public string ToStringProperty
        {
            get
            {
                try
                {
                    string pago = MovimentacaoCaixa != null ? "Pago" : "Não pago";
                    return "(" + DataHoraDeRecebimento + ") " + Cliente.Pessoa.Nome +"; " + pago;
                }
                catch
                {
                    return "Não foi possível exibir a descrição";
                }
            }
        }
    }

}
