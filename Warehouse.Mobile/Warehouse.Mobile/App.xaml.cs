using System;
using Warehouse.Mobile.Services;
using Warehouse.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Warehouse.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
