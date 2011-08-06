using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacoteEmpresa;

namespace ERP.Lavanderia.Module.PacoteConfiguracoes
{
    [DefaultClassOptions]
    public class ConfiguracaoUsuario : BaseObject
    {
        private Empresa empresaPadrao;

        public ConfiguracaoUsuario(Session session) : base(session) { }

        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public Empresa EmpresaPadrao
        {
            get { return empresaPadrao; }
            set { SetPropertyValue("EmpresaPadrao", ref empresaPadrao, value); }

        }

    }

}
