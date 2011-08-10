using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.Web;

namespace ERP.Lavanderia.Web
{
    public partial class LavanderiaAspNetApplication : WebApplication
    {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        private ERP.Lavanderia.Module.LavanderiaModule module3;
        private ERP.Lavanderia.Module.Web.LavanderiaAspNetModule module4;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule module6;
        private System.Data.SqlClient.SqlConnection sqlConnection1;
        private DevExpress.ExpressApp.Security.SecurityComplex securityComplex1;
        private DevExpress.ExpressApp.Security.AuthenticationStandard authenticationStandard1;
        private DevExpress.ExpressApp.ConditionalEditorState.Web.ConditionalEditorStateAspNetModule conditionalEditorStateAspNetModule1;
        private DevExpress.ExpressApp.ConditionalEditorState.ConditionalEditorStateModuleBase conditionalEditorStateModuleBase1;
        private DevExpress.ExpressApp.Validation.ValidationModule module5;

        public LavanderiaAspNetApplication()
        {
            InitializeComponent();
        }

        private void LavanderiaAspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e)
        {
#if EASYTEST
			e.Updater.Update();
			e.Handled = true;
#else
            if (System.Diagnostics.Debugger.IsAttached)
            {
                e.Updater.Update();
                e.Handled = true;
            }
            else
            {
                throw new InvalidOperationException(
                    "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                    "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
                    "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
                    "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                    "or manually create a database using the 'DBUpdater' tool.\r\n" +
                    "Anyway, refer to the 'Update Application and Database Versions' help topic at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm " +
                    "for more detailed information. If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/");
            }
#endif
        }

        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.module3 = new ERP.Lavanderia.Module.LavanderiaModule();
            this.module4 = new ERP.Lavanderia.Module.Web.LavanderiaAspNetModule();
            this.module5 = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.module6 = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.securityComplex1 = new DevExpress.ExpressApp.Security.SecurityComplex();
            this.authenticationStandard1 = new DevExpress.ExpressApp.Security.AuthenticationStandard();
            this.conditionalEditorStateAspNetModule1 = new DevExpress.ExpressApp.ConditionalEditorState.Web.ConditionalEditorStateAspNetModule();
            this.conditionalEditorStateModuleBase1 = new DevExpress.ExpressApp.ConditionalEditorState.ConditionalEditorStateModuleBase();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // module5
            // 
            this.module5.AllowValidationDetailsAccess = true;
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.ConnectionString = "Data Source=(local);Initial Catalog=ERP.Lavanderia;Integrated Security=SSPI;Pooli" +
                "ng=false";
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // securityComplex1
            // 
            this.securityComplex1.Authentication = this.authenticationStandard1;
            this.securityComplex1.RoleType = typeof(DevExpress.Persistent.BaseImpl.Role);
            this.securityComplex1.UserType = typeof(DevExpress.Persistent.BaseImpl.User);
            // 
            // authenticationStandard1
            // 
            this.authenticationStandard1.LogonParametersType = typeof(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters);
            // 
            // LavanderiaAspNetApplication
            // 
            this.ApplicationName = "ERP";
            this.Connection = this.sqlConnection1;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.module6);
            this.Modules.Add(this.securityModule1);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.module5);
            this.Modules.Add(this.conditionalEditorStateModuleBase1);
            this.Modules.Add(this.conditionalEditorStateAspNetModule1);
            this.Security = this.securityComplex1;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.LavanderiaAspNetApplication_DatabaseVersionMismatch);
            this.LoggedOn += new System.EventHandler<DevExpress.ExpressApp.LogonEventArgs>(this.ERP_LoggedOn);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        private void ERP_LoggedOn(object sender, DevExpress.ExpressApp.LogonEventArgs e)
        {
            ((ShowViewStrategy)base.ShowViewStrategy).CollectionsEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
        }
    }
}
