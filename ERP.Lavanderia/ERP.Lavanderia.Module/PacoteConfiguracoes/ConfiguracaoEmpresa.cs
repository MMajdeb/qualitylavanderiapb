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
    public class ConfiguracaoEmpresa : BaseObject
    {
        public ConfiguracaoEmpresa(Session session) : base(session) { }

        /// <summary>
        /// Singleton do objeto.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public static ConfiguracaoEmpresa GetInstance(Session session, Empresa empresa)
        {
            ConfiguracaoEmpresa result = session.FindObject<ConfiguracaoEmpresa>(new BinaryOperator("Empresa", empresa.Oid));

            if (result == null)
            {
                result = new ConfiguracaoEmpresa(session);
                //result.Save();
            }

            return result;
        }

    }

}
