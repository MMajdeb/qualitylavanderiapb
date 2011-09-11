namespace ERP.Lavanderia.Module.PacoteControladoress.ControladoresLavagem
{
    partial class ImprimirLavagemViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.saImprimir = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // saImprimir
            // 
            this.saImprimir.Caption = "Imprimir";
            this.saImprimir.Category = "View";
            this.saImprimir.ConfirmationMessage = null;
            this.saImprimir.Id = "saImprimir";
            this.saImprimir.ImageName = "imprimir";
            this.saImprimir.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.saImprimir.Shortcut = null;
            this.saImprimir.Tag = null;
            this.saImprimir.TargetObjectsCriteria = null;
            this.saImprimir.TargetObjectType = typeof(ERP.Lavanderia.Module.PacoteLavagem.Lavagem);
            this.saImprimir.TargetViewId = null;
            this.saImprimir.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.saImprimir.ToolTip = "Imprimir uma lavagem";
            this.saImprimir.TypeOfView = null;
            this.saImprimir.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.saImprimir_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction saImprimir;
    }
}
