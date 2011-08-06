using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Collections.Generic;
using DevExpress.Persistent.Base.Security;
using System.Security;

namespace ERP.Lavanderia.Module.PacoteSeguranca
{
    [DefaultClassOptions]
    public class Papel : RoleBase, IRole, ICustomizableRole
    {
        public Papel(Session session) : base(session) { }

        [Association("User-Role")]
        public XPCollection<Usuario> Users
        {
            get { return GetCollection<Usuario>("Users"); }
        }
        IList<IUser> IRole.Users
        {
            get
            {
                return new ListConverter<IUser, Usuario>(Users);
            }
        }
        void ICustomizableRole.AddPermission(IPermission permission)
        {
            base.AddPermission(permission);
        }
    }

}
