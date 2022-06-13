using System;
using System.Collections.Generic;
using Warehouse.Mobile.ViewModels;
using Warehouse.Mobile.Views;
using Xamarin.Forms;

namespace Warehouse.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
