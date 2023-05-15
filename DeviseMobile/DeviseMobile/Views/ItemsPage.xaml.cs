using DeviseMobile.Models;
using DeviseMobile.ViewModels;
using DeviseMobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using System.Data.SqlClient;

namespace DeviseMobile.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;
        bool selecteds = false;
        public ItemsPage()
        {
            InitializeComponent();
           
            //BindingContext = _viewModel = new ItemsViewModel();
            Dates();
        }

        List<Sostav_Zakaz> sost = null;

        public async void Update_Tovar(object sender, DownloadStringCompletedEventArgs e)
        {
            string responTov = e.Result;
                double zena = 0;
            if (st != "-1" && responTov != "0")
            {
                sost = JsonConvert.DeserializeObject<List<Sostav_Zakaz>>(responTov);
                DatesTovar.ItemsSource = sost;
                KolTov.Text = $"В корзине: {sost.Count} товаров";
                this.Dell.IsVisible = true;

            }
            else
            {
                DatesTovar.ItemsSource = null;
                this.Zena.Text = "Сумма к оплате: 0";
                sost = null;
                KolTov.Text = $"Корзина пуста";
                this.Dell.IsVisible = false;

            }
            if (sost != null)
            {
                foreach (Sostav_Zakaz SZ in sost)
                {
                    zena += ((double)SZ.Zena * (Hold.Nazenka/100.0)) * (double)SZ.Koll;
                 
                }
                this.Zena.Text = "Сумма к оплате: \n" + zena.ToString("##.##") + " руб.";
            }


            KodVidashi.Text = "Код для получения: \n" + Preferences.Get("KeyD", "---");
        }
        string st = "";
        void Dates()
        {
            try
            {


                var client = new WebClient();

                st = Preferences.Get("Zakaz", "-1");
                string f = st.Substring(0, st.LastIndexOf('*'));
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Update_Tovar);

                client.DownloadStringAsync(new Uri($"{Hold.Adress}/api/View_Sost_Naklad_Prodag/{f}"));
               
            }
            catch
            {
                Navigation.PushAsync(new NonInternet());

            }

        }

          

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //_viewModel.OnAppearing();
            Dates();
        }
        List<Sostav_Zakaz> sostav_ZakazsDel = new List<Sostav_Zakaz>();

        private void Dell_Clicked(object sender, EventArgs e)
        {
            if (DatesTovar.SelectedItem != null)
            {
       
                try
                {
                    var client = new WebClient();


                    int nakl = (DatesTovar.SelectedItem as Sostav_Zakaz).ID_Naklad_Prodag;
                    int tov = (DatesTovar.SelectedItem as Sostav_Zakaz).ID_Tovar;
                    client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Dell_Tovar); 
                    client.DownloadStringAsync(new Uri($"{Hold.Adress}/api/del?ID_Nakl={nakl}&ID_Tov={tov}"));
                }
                catch
                {
                    Navigation.PushAsync(new NonInternet());

                }
            
            }
        }

        public async void Dell_Tovar(object sender, DownloadStringCompletedEventArgs e)
        {
            Dates();

        }



        private void CB_Dell_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            
        }
    }
}