using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using ERP.Lavanderia.Module.PacoteLavagem;
using DevExpress.Data.Filtering;

namespace ERP.Lavanderia.Module.PacoteControladoress.ControladoresLavagem
{
    public partial class LavagensDoDiaViewController : ViewController
    {
        public LavagensDoDiaViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.ControlsCreated += new EventHandler(View_ControlsCreated);
        }

        void View_ControlsCreated(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var limiteInferior = new DateTime(now.Year, now.Month, now.Day);
            var limiteSuperior = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            ((ListView)View).CollectionSource.Criteria["Filter1"] = new BetweenOperator("DataHoraPreferivelParaEntrega", limiteInferior, limiteSuperior);

        }
    }
}
