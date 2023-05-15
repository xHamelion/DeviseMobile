using DeviseMobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace DeviseMobile.Views
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