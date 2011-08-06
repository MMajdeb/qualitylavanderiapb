using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace ERP.Lavanderia.Module.PacoteSeguranca
{
    [NonPersistent]
    public class ParametrosLogon : INotifyPropertyChanged
    {
        private IObjectSpace objectSpace;
        //private ReadOnlyCollection<Empresa> availableCompanies;
        //private XPCollection<Usuario> availableUsers; //comentado pois não está sendo usado.

        //private Empresa empresa;
        private string userName;
        private string password;

        [Browsable(false)]
        public IObjectSpace ObjectSpace
        {
            get { return objectSpace; }
            set { objectSpace = value; }
        }
        //[Browsable(false)]
        //public ReadOnlyCollection<Empresa> AvailableCompanies
        //{
        //    get
        //    {
        //        if (objectSpace == null)
        //        {
        //            throw new InvalidOperationException("objectSpace is null");
        //        }
        //        if (availableCompanies == null)
        //        {
        //            availableCompanies = new ReadOnlyCollection<Empresa>(ObjectSpace.GetObjects<Empresa>(null));
        //        }
        //        return availableCompanies;
        //    }
        //}

        //[DataSourceProperty("AvailableCompanies"), ImmediatePostData]
        //public Empresa Empresa
        //{
        //    get { return empresa; }
        //    set
        //    {
        //        empresa = value;
        //    }
        //}

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
                }
            }
        }

        [PasswordPropertyText(true)]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
