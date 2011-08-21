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
using DevExpress.ExpressApp.Security;
using ERP.Lavanderia.Module.PacoteEmpresa;
using ERP.Lavanderia.Module.PacoteCliente;
using ERP.Lavanderia.Module.PacoteRoupa;
using ERP.Lavanderia.Module.PacoteLavagem;
using ERP.Lavanderia.Module.PacoteColaborador;
using ERP.Lavanderia.Module.PacoteMaterial;
using ERP.Lavanderia.Module.PacotePessoa;

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

        public static void CriarPapeis(Session session) {
            /*** Papel cliente ***/
            criarPapelCliente(session);

            /*** Papel funcionario nivel 1 ***/
            criarPapelFuncionarioNivel1(session);

            /*** Papel funcionario nivel 2 ***/
            criarPapelFuncionarioNivel2(session);

            /*** Papel gerente ***/
            criarPapelGerente(session);

            /*** Papel administrador ***/
            criarPapelAdministrador(session);
        }

        private static void criarPapelCliente(Session session)
        {
            // If a role with the Users name doesn't exist in the database, create this role
            Papel clienteRole = session.FindObject<Papel>(new BinaryOperator("Name", TipoPapelLavanderia.Cliente.ToString()));
            if (clienteRole == null)
            {
                clienteRole = new Papel(session);
                clienteRole.Name = TipoPapelLavanderia.Cliente.ToString();
            }

            while (clienteRole.PersistentPermissions.Count > 0)
            {
                session.Delete(clienteRole.PersistentPermissions[0]);
            }

            clienteRole.AddPermission(new ObjectAccessPermission(typeof(Pessoa), ObjectAccess.Read, ObjectAccessModifier.Allow));
            clienteRole.AddPermission(new ObjectAccessPermission(typeof(Empresa), ObjectAccess.Read, ObjectAccessModifier.Allow));
            clienteRole.AddPermission(new ObjectAccessPermission(typeof(Cliente), ObjectAccess.Read, ObjectAccessModifier.Allow));

            /*** Padrao ***/
            clienteRole.AddPermission(new EditModelPermission(ModelAccessModifier.Allow));
            clienteRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.NoAccess));

            clienteRole.Save();
        }

        private static void criarPapelFuncionarioNivel1(Session session)
        {
            /*** Papel funcionario ***/
            // If a role with the Users name doesn't exist in the database, create this role
            Papel funcionarioRole = session.FindObject<Papel>(new BinaryOperator("Name", TipoPapelLavanderia.FuncionarioNivel1.ToString()));
            if (funcionarioRole == null)
            {
                funcionarioRole = new Papel(session);
                funcionarioRole.Name = TipoPapelLavanderia.FuncionarioNivel1.ToString();
            }

            while (funcionarioRole.PersistentPermissions.Count > 0)
            {
                session.Delete(funcionarioRole.PersistentPermissions[0]);
            }

            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Pessoa), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Empresa), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Colaborador), ObjectAccess.Read, ObjectAccessModifier.Allow));

            /*** Padrao ***/
            funcionarioRole.AddPermission(new EditModelPermission(ModelAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.NoAccess));

            // Save the Users role to the database
            funcionarioRole.Save();
        }

        private static void criarPapelFuncionarioNivel2(Session session)
        {
            /*** Papel funcionario ***/
            // If a role with the Users name doesn't exist in the database, create this role
            Papel funcionarioRole = session.FindObject<Papel>(new BinaryOperator("Name", TipoPapelLavanderia.FuncionarioNivel2.ToString()));
            if (funcionarioRole == null)
            {
                funcionarioRole = new Papel(session);
                funcionarioRole.Name = TipoPapelLavanderia.FuncionarioNivel2.ToString();
            }

            while (funcionarioRole.PersistentPermissions.Count > 0)
            {
                session.Delete(funcionarioRole.PersistentPermissions[0]);
            }

            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Pessoa), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Empresa), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Usuario), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Cliente), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(PontoDeColeta), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Colaborador), ObjectAccess.Read, ObjectAccessModifier.Allow));
            
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Roupa), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Roupa), ObjectAccess.Create, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Roupa), ObjectAccess.ChangeAccess, ObjectAccessModifier.Allow));
            
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Lavagem), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Lavagem), ObjectAccess.Create, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Lavagem), ObjectAccess.ChangeAccess, ObjectAccessModifier.Allow));

            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(RoupaLavagem), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(RoupaLavagem), ObjectAccess.Create, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(RoupaLavagem), ObjectAccess.ChangeAccess, ObjectAccessModifier.Allow));

            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(PacoteDeRoupa), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(PacoteDeRoupa), ObjectAccess.Create, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(PacoteDeRoupa), ObjectAccess.ChangeAccess, ObjectAccessModifier.Allow));

            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(TipoPacoteDeRoupa), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(TipoPacoteDeRoupa), ObjectAccess.Create, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(TipoPacoteDeRoupa), ObjectAccess.ChangeAccess, ObjectAccessModifier.Allow));

            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(Material), ObjectAccess.Read, ObjectAccessModifier.Allow));

            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(MovimentacaoMaterial), ObjectAccess.Read, ObjectAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(MovimentacaoMaterial), ObjectAccess.Create, ObjectAccessModifier.Allow));

            /*** Padrao ***/
            funcionarioRole.AddPermission(new EditModelPermission(ModelAccessModifier.Allow));
            funcionarioRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.NoAccess));

            // Save the Users role to the database
            funcionarioRole.Save();
        }

        private static void criarPapelAdministrador(Session session)
        {
            // If a role with the Administrators name doesn't exist in the database, create this role
            Papel adminRole = session.FindObject<Papel>(new BinaryOperator("Name", TipoPapelLavanderia.Administrador.ToString()));
            if (adminRole == null)
            {
                adminRole = new Papel(session);
                adminRole.Name = TipoPapelLavanderia.Administrador.ToString();
            }

            // Delete all permissions assigned to the Administrators and Users roles
            while (adminRole.PersistentPermissions.Count > 0)
            {
                session.Delete(adminRole.PersistentPermissions[0]);
            }

            /*** Permissoes de administrador ***/
            // Allow full access to all objects to the Administrators role
            adminRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess));
            // Deny editing access to the AuditDataItemPersistent type objects to the Administrators role
            adminRole.AddPermission(new ObjectAccessPermission(typeof(AuditDataItemPersistent), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            // Allow editing the application model to the Administrators role
            adminRole.AddPermission(new EditModelPermission(ModelAccessModifier.Allow));
            // Save the Administrators role to the database
            adminRole.Save();
        }

        private static void criarPapelGerente(Session session)
        {
            // If a role with the Administrators name doesn't exist in the database, create this role
            Papel adminRole = session.FindObject<Papel>(new BinaryOperator("Name", TipoPapelLavanderia.Gerente.ToString()));
            if (adminRole == null)
            {
                adminRole = new Papel(session);
                adminRole.Name = TipoPapelLavanderia.Gerente.ToString();
            }

            // Delete all permissions assigned to the Administrators and Users roles
            while (adminRole.PersistentPermissions.Count > 0)
            {
                session.Delete(adminRole.PersistentPermissions[0]);
            }

            /*** Permissoes de administrador ***/
            // Allow full access to all objects to the Administrators role
            adminRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess));
            // Deny editing access to the AuditDataItemPersistent type objects to the Administrators role
            adminRole.AddPermission(new ObjectAccessPermission(typeof(AuditDataItemPersistent), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            // Allow editing the application model to the Administrators role
            adminRole.AddPermission(new EditModelPermission(ModelAccessModifier.Allow));
            // Save the Administrators role to the database
            adminRole.Save();
        }

        public static Papel RetornaPapel(TipoPapelLavanderia tipoPapel, Session session)
        {
            return session.FindObject<Papel>(new BinaryOperator("Name", tipoPapel.ToString())); ;
        }
    }

    public enum TipoPapelLavanderia {
        Cliente, FuncionarioNivel1, FuncionarioNivel2, Gerente, Administrador
    }

}
