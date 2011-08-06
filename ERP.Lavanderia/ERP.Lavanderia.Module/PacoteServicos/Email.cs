using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Collections.Generic;
using System.Net.Mail;
using ERP.Lavanderia.Module.PacoteConfiguracoes;
using System.Net;

namespace ERP.Lavanderia.Module.PacoteServicos
{
    public class Email
    {
        private string _remetente;
        private string _destinatario;
        private string _assunto;
        private string _mensagem;
        private string _comcopiapara;

        #region Propriedades

        public string Remetente
        {
            get
            {
                return _remetente;
            }
            set
            {
                _remetente = value;
            }
        }

        public string Destinatario
        {
            get
            {
                return _destinatario;
            }
            set
            {
                _destinatario = value;
            }
        }

        public string ComCopiaPara
        {
            get
            {
                return _comcopiapara;
            }
            set
            {
                _comcopiapara = value;
            }
        }

        public string Assunto
        {
            get
            {
                return _assunto;
            }
            set
            {
                _assunto = value;
            }
        }

        public string Mensagem
        {
            get
            {
                return _mensagem;
            }
            set
            {
                _mensagem = value;
            }
        }

        public List<Attachment> Anexos { get; set; }

        #endregion

        /// <summary>
        /// Envia um e-mail através do servidor SMTP padrão configurado.
        /// </summary>
        public void Enviar(Session session)
        {
            ConfiguracaoGeral cfg = new ConfiguracaoGeral(session);
            string Servidor = cfg.ServidorSmtp;
            int Porta = cfg.PortaSmtp;
            bool UsaAutenticacao = cfg.UtilizarAutenticacaoSmtp;
            string UsuarioSmtp = cfg.UsuarioSmtp;
            string SenhaSmtp = cfg.SenhaSmtp;

            SmtpClient smtp = new SmtpClient();

            if (Servidor != "")
                smtp.Host = Servidor;

            if (Porta != 0)
                smtp.Port = Convert.ToInt32(Porta);

            if (UsaAutenticacao)
                smtp.Credentials = new NetworkCredential(UsuarioSmtp, SenhaSmtp);

            MailMessage mail = new MailMessage(_remetente, _destinatario);
            mail.Subject = _assunto;
            mail.Body = _mensagem;

            if (Anexos.Count > 0)
                foreach (Attachment anexo in Anexos)
                    mail.Attachments.Add(anexo);

            smtp.Send(mail);
        }

        public static void EnviarEmail(Session session, string EnderecoRemetente, string EnderecoDestinatario, string Assunto, string Mensagem)
        {
            var email = new Email()
            {
                Remetente = EnderecoRemetente,
                Destinatario = EnderecoDestinatario,
                Assunto = Assunto,
                Mensagem = Mensagem
            };

            email.Enviar(session);
        }
    }

}
