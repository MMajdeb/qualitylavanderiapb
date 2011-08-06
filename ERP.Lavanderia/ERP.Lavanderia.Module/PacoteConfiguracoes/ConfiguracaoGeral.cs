using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace ERP.Lavanderia.Module.PacoteConfiguracoes
{
    [DefaultClassOptions]
    public class ConfiguracaoGeral : BaseObject
    {
        //Servidor SMTP
        private string servidorSmtp;
        private int portaSmpt;
        private bool utilizarAutenticacaoSmpt;
        private string userSmtp;
        private string senhaSmtp;
        private bool utilizarSsl;
        //Comunicação com coletores
        private string mensagemAniversario;
        private DiaMensagemFelicitacao diaMensagemAniversario;

        private string emailAlerta;

        public ConfiguracaoGeral(Session session) : base(session) { }

        //[Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        //public Empresa EmpresaPadrao { get { return empresaPadrao; } set { SetPropertyValue("EmpresaPadrao", ref empresaPadrao, value); } }

        public string ServidorSmtp { get { return servidorSmtp; } set { SetPropertyValue("ServidorSmtp", ref servidorSmtp, value); } }

        public int PortaSmtp { get { return portaSmpt; } set { SetPropertyValue("PortaSmtp", ref portaSmpt, value); } }

        public bool UtilizarAutenticacaoSmtp { get { return utilizarAutenticacaoSmpt; } set { SetPropertyValue("UtilizarAutenticacaoSmtp", ref utilizarAutenticacaoSmpt, value); } }

        public string UsuarioSmtp { get { return userSmtp; } set { SetPropertyValue("UsuarioSmtp", ref userSmtp, value); } }

        public string SenhaSmtp { get { return senhaSmtp; } set { SetPropertyValue("SenhaSmtp", ref senhaSmtp, value); } }

        /// <summary>
        /// Define se irá utilizar conexão via SSL para envio de e-mail
        /// </summary>
        public bool UtilizarSsl { get { return utilizarSsl; } set { SetPropertyValue("UtilizarSsl", ref utilizarSsl, value); } }

        public string EmailAlerta
        {
            get
            {
                return emailAlerta;
            }
            set
            {
                SetPropertyValue("EmailAlerta", ref emailAlerta, value);
            }
        }

        public string MensagemAniversario
        {
            get { return mensagemAniversario; }
            set { SetPropertyValue("MensagemAniversario", ref mensagemAniversario, value); }
        }

        public DiaMensagemFelicitacao DiaMensagemAniversario
        {
            get { return diaMensagemAniversario; }
            set { SetPropertyValue("DiaMensagemAniversario", ref diaMensagemAniversario, value); }
        }

        public static ConfiguracaoGeral RetornaConfiguracaoGeral(IObjectSpace objectSpace)
        {
            XPCollection cfg = objectSpace.CreateCollection(typeof(ConfiguracaoGeral)) as XPCollection;
            if (cfg.Count > 0)
            {
                return cfg[0] as ConfiguracaoGeral;
            }
            else
                return null;
        }

        public static ConfiguracaoGeral RetornaConfiguracaoGeral(Session session)
        {
            XPCollection<ConfiguracaoGeral> cfg = new XPCollection<ConfiguracaoGeral>(session);
            if (cfg.Count > 0)
                return cfg[0];
            else
                return null;
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

    }

    public enum DiaMensagemFelicitacao
    {
        DiaAnteriorAniversario,
        DiaAniversario
    }

}
