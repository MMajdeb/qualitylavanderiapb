using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using ERP.Lavanderia.Module.PacoteEmpresa;
using System.Collections;
using DevExpress.Xpo.Metadata;
using ERP.Lavanderia.Module.PacoteConfiguracoes;
using System.Collections.Generic;
using System.Security;
using DevExpress.Persistent.Base.Security;

namespace ERP.Lavanderia.Module.PacoteSeguranca
{
    [DefaultProperty("UserName")]
    [DefaultClassOptions]
    public class Usuario : BaseObject, IUserWithRoles, IAuthenticationActiveDirectoryUser, IAuthenticationStandardUser
    {
        private string _userName;
        private string _storedPassword;
        private bool _isActive = true;
        private bool _changePasswordAfterLogon = false;
        private List<IPermission> permissions;
        private ConfiguracaoUsuario configuracao;

        public Usuario(Session session)
            : base(session)
        {
            permissions = new List<IPermission>();
        }

        public void ReloadPermissions()
        {
            Roles.Reload();
            foreach (Papel role in Roles)
            {
                role.PersistentPermissions.Reload();
            }
        }
        public bool ComparePassword(string password)
        {
            return new PasswordCryptographer().AreEqual(this._storedPassword, password);
        }
        public void SetPassword(string password)
        {
            this._storedPassword = new PasswordCryptographer().GenerateSaltedPassword(password);
        }
#if MediumTrust
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[Persistent]
		public string StoredPassword {
			get { return this._storedPassword; }
			set {
				this._storedPassword = value;
				OnChanged("StoredPassword");
			}
		}
#else
        [Persistent]
        private string StoredPassword
        {
            get { return this._storedPassword; }
            set
            {
                this._storedPassword = value;
                OnChanged("StoredPassword");
            }
        }
#endif
        [Association("User-Role")]
        public XPCollection<Papel> Roles
        {
            get { return GetCollection<Papel>("Roles"); }
        }
        IList<IRole> IUserWithRoles.Roles
        {
            get
            {
                return new ListConverter<IRole, Papel>(Roles);
            }
        }
        [RuleRequiredField("Fill User Name", "Save", "O nome de usuario não pode ser vazio")]
        [RuleUniqueValue("Unique User Name", "Save", "Esse nome de usuário já está em uso")]
        public string UserName
        {
            get { return this._userName; }
            set
            {
                this._userName = value;
                OnChanged("UserName");
            }
        }
        public bool ChangePasswordOnFirstLogon
        {
            get { return this._changePasswordAfterLogon; }
            set
            {
                this._changePasswordAfterLogon = value;
                OnChanged("ChangePasswordOnFirstLogon");
            }
        }
        public bool IsActive
        {
            get { return this._isActive; }
            set
            {
                this._isActive = value;
                OnChanged("IsActive");
            }
        }

        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public ConfiguracaoUsuario Configuracao
        {
            get { return configuracao; }
            set
            {
                SetPropertyValue("Configuracao", ref configuracao, value);
            }
        }

        /// <summary>
        /// Define a associação do usuário com as empresas que ele pode acessar.
        /// </summary>
        [Association("Usuario-Empresas")]
        public XPCollection<Empresa> Empresas
        {
            get { return GetCollection<Empresa>("Empresas"); }
        }

        /// <summary>
        /// Define a associação do usuário com os departamentos que ele pode supervisionar.
        /// </summary>
        [Association("Usuario-Departamento")]
        public XPCollection<Departamento> Departamentos
        {
            get { return GetCollection<Departamento>("Departamentos"); }
        }

        public IList<IPermission> Permissions
        {
            get
            {
                permissions.Clear();
                foreach (Papel role in Roles)
                {
                    permissions.AddRange(role.Permissions);
                }
                return permissions.AsReadOnly();
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            this.ChangePasswordOnFirstLogon = true;
            this.Configuracao = new ConfiguracaoUsuario(Session);
            IsActive = true;
        }

        [RuleFromBoolProperty("Usuario.RuleFromBoolProperty.ValidaDeletar", DefaultContexts.Delete,
    CustomMessageTemplate = "Esse usuário possui associações e não pode ser deletado")]
        [Browsable(false)]
        public bool ValidaDeletar
        {
            get
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
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
        }

        [RuleFromBoolProperty("Usuario.RuleFromBoolProperty.NaoDeixaDeletarAdministrador", DefaultContexts.Delete,
CustomMessageTemplate = "O administrador não pode ser deletado")]
        [Browsable(false)]
        public bool NaoDeixaDeletarAdministrador
        {
            get
            {
                return _userName == null && !_userName.Equals("administrador");
            }
        }

        /// <summary>
        /// Retorna um objeto Usuario pelo id passado.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="oid"></param>
        /// <returns></returns>
        public static Usuario RetornaUsuarioPorId(IObjectSpace os, string oid)
        {
            Usuario usr = os.FindObject<Usuario>(new BinaryOperator("Oid", oid));
            return usr;
        }

        /// <summary>
        /// Retorna um objeto Usuario pelo 'user name' passado.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Usuario RetornaUsuarioPorUserName(IObjectSpace objectSpace, string userName)
        {
            Usuario usr = objectSpace.FindObject<Usuario>(new BinaryOperator("UserName", userName));
            return usr;
        }

        /// <summary>
        /// Verifica se o usuário tem permissão de acesso a uma determinada empresa.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="usuario">Usuário que se deseja verificar.</param>
        /// <param name="empresa">Empresa para verificação de permissão.</param>
        /// <returns></returns>
        public static bool EmpresaPermitida(Usuario usuario, Empresa empresa)
        {
            XPCollection<Empresa> lista = usuario.Empresas;

            foreach (Empresa emp in lista)
            {
                if (emp.Oid == empresa.Oid)
                    return true;
            }

            return false;
        }
    }

}
