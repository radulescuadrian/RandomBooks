using RandomBooks.Couriers.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace RandomBooks.Couriers.Views
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