using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.ExpressApp.Security;

namespace ERP.Lavanderia.Module.PacoteSeguranca
{
    public class Autenticacao : AuthenticationBase, IAuthenticationStandard
    {
        private ParametrosLogon logonParameters;

        public Autenticacao()
        {
            logonParameters = new ParametrosLogon();
        }

        public override void ClearSecuredLogonParameters()
        {
            base.ClearSecuredLogonParameters();
        }

        public override object Authenticate(DevExpress.ExpressApp.IObjectSpace objectSpace)
        {
            if (logonParameters.UserName == "")
            {
                throw new ArgumentNullException("User");
            }

            Usuario usr = Usuario.RetornaUsuarioPorUserName(objectSpace, logonParameters.UserName);

            if (usr == null)
            {
                throw new AuthenticationException(logonParameters.UserName, "Nome de usuário inválido.");
            }

            if (!usr.ComparePassword(logonParameters.Password))
            {
                throw new AuthenticationException(logonParameters.UserName, "Senha inválida.");
            }

            return objectSpace.GetObject(usr);
        }
        public override IList<Type> GetBusinessClasses()
        {
            return new Type[] { typeof(ParametrosLogon) };
        }
        public override bool AskLogonParametersViaUI
        {
            get
            {
                return true;
            }
        }
        public override object LogonParameters
        {
            get { return logonParameters; }
        }

        public override bool IsLogoffEnabled
        {
            get { return true; }
        }
    }
}
