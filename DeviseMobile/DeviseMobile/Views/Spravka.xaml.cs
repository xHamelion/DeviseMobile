using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DeviseMobile.Views;

namespace DeviseMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Spravka : ContentPage
    {
        public Spravka()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");

        }
    }
}