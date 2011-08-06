using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using ERP.Lavanderia.Module.PacoteSeguranca;
using ERP.Lavanderia.Module.PacoteEmpresa;

namespace ERP.Lavanderia.Module.PacoteControladoress.ControladoresEmpresa
{
    /// <summary>
    /// Controller que adiciona a seleção de empresa e que contem qual a empresa ativa do sistema
    /// </summary>
    /// 
    public delegate XafApplication GetApplicationInstanceCallback();

    public partial class SelecaoEmpresaController : WindowController
    {
        IObjectSpace space;
        private string empresaPadraoOid;

        public static GetApplicationInstanceCallback GetApplicationInstance;

        public SelecaoEmpresaController()
        {
            InitializeComponent();
            RegisterActions(components);
            this.TargetWindowType = WindowType.Main;

        }

        public static SelecaoEmpresaController Instance()
        {
            if (GetApplicationInstance == null)
            {
                return null;
            }
            else
            {
                XafApplication application = GetApplicationInstance();
                if (application == null)
                    return null;
                else
                    return application.MainWindow.GetController<SelecaoEmpresaController>();
            }
        }

        public string EmpresaPadraoOid
        {
            get {
                if (empresaPadraoOid == null)
                {
                    var empresa = this.Application.ObjectSpaceProvider.CreateObjectSpace().GetObjects<Empresa>()[0];

                    if (empresa != null)
                    {
                        empresaPadraoOid = empresa.Oid.ToString();
                    }
                }

                return empresaPadraoOid; 
            }
            set { empresaPadraoOid = value; }
        }


        private void SelecaoEmpresaController_Activated(object sender, EventArgs e)
        {
            //space = this.Application.ObjectSpaceProvider.CreateObjectSpace();

            //ParametrosLogon logonParam = (ParametrosLogon)SecuritySystem.LogonParameters;

            //string usuarioLogado = logonParam.UserName;

            //Usuario usr = Usuario.RetornaUsuarioPorId(space, SecuritySystem.CurrentUserId.ToString());

            //if (this.SelecaoEmpresaAction.Items.Count == 0)
            //{

            //    IList<Empresa> list;

            //    //Se for o usuário Administrador, carrega todas as empresas
            //    //caso contrário, carrega somente as empresas que o usuário tem permissão
            //    if (usuarioLogado.ToLower() == "administrador")
            //    {
            //        list = space.GetObjects<Empresa>();
            //    }
            //    else
            //    {
            //        list = usr.Empresas;
            //    }

            //    foreach (Empresa emp in list)
            //    {
            //        ChoiceActionItem item = new ChoiceActionItem(emp.Pessoa.Nome, emp.Oid.ToString());
            //        this.SelecaoEmpresaAction.Items.Add(item);
            //    }

            //}

            ////Verifica se o usuário selecionou uma empresa no Logon
            //    //Coloca ativa a primeira empresa cadastrada
            //if (this.SelecaoEmpresaAction.Items.Count > 0)
            //{
            //    this.SelecaoEmpresaAction.SelectedItem = this.SelecaoEmpresaAction.Items[0];
            //    this.empresaPadraoOid = this.SelecaoEmpresaAction.Items[0].Data.ToString();
            //}
            

        }

        private void SelecaoEmpresaAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            //empresaPadraoOid = this.SelecaoEmpresaAction.SelectedItem.Data.ToString();

            ////Ao selecionar uma nova empresa, redireciona a navegação para a Tela Inicial
            //ShowViewParameters svpInternal = new ShowViewParameters();

            //string viewID = Application.FindListViewId(typeof(UIContainerObject));
            //ViewShortcut shortcut = new ViewShortcut();
            //shortcut.ViewId = viewID;

            //svpInternal.Context = TemplateContext.ApplicationWindow;
            //svpInternal.TargetWindow = TargetWindow.Current;

            //svpInternal.CreatedView = Application.ProcessShortcut(shortcut);

            //Application.ShowViewStrategy.ShowView(svpInternal, new ShowViewSource(Application.MainWindow, null));
        }

        private void SelecaoEmpresaController_ViewClosed(object sender, EventArgs e)
        {
            space.Dispose();
        }
    }
}
