using Couriers.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Couriers.Mobile.Views
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