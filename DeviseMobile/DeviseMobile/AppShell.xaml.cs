using DeviseMobile.ViewModels;
using DeviseMobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DeviseMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            //Routing.RegisterRoute($"//{nameof(AboutPage)}", typeof(AboutPage));
            Hold.AppShell = this;
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            string s = Preferences.Get("ID", "F");
            if (s == "F")
            {
                await DisplayAlert("Внимание!", "Войдите в аккаунт.", "Ок");

                await Shell.Current.GoToAsync("//LoginPage");

            }
            else
            {
                bool result = await DisplayAlert("Подтвердить действие", "Вы хотите перезайти?", "Да", "Нет");
                if (result)
                {
                    Preferences.Remove("ID");
                    Preferences.Remove("Zakaz");
                    Preferences.Remove("KeyD");
                    await Shell.Current.GoToAsync("//LoginPage");

                }
            }


        }
        public void Reload()
        {
            OnMenuItemClicked(null, null);
        }
    }
}
