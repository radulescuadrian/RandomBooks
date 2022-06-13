using System.ComponentModel;
using Warehouse.Mobile.ViewModels;
using Xamarin.Forms;

namespace Warehouse.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}