using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace DeviseMobile.Views
{
    public partial class AboutPage : ContentPage
    {
        Dictionary<string, List<string>> kategor = new Dictionary<string, List<string>>();
        void AoutoRis()
        {
            if (!Preferences.ContainsKey("ID"))
            {
                Hold.Aitoriz = false;
                Hold.AppShell.Reload();

            }

        }

        public async void View_Sklad_Load(object sender, DownloadStringCompletedEventArgs e)
        {

            try
            {
                string responTov = e.Result;

                DatesTovar.ItemsSource = JsonConvert.DeserializeObject<List<TovarS>>(responTov);

                

                kategors = kategors.Distinct().ToList();
                foreach (Kategor d in kategors)
                {
                    if (!kategor.ContainsKey(d.Tip))
                    {

                        List<string> sss = new List<string>();
                        sss.Add("Все подкатегории");
                        sss.Add(d.PodTip);

                        kategor.Add(d.Tip, sss);
                        kategor[d.Tip] = kategor[d.Tip].Distinct().ToList();
                        Tip.Items.Add(d.Tip);
                    }
                    else
                    {
                        kategor[d.Tip] = kategor[d.Tip].Distinct().ToList();
                        (kategor[d.Tip] as List<string>).Add(d.PodTip);
                    }
                    kategor[d.Tip] = kategor[d.Tip].Distinct().ToList();
                }
                int i = Tip.Items.Count(p => p == "Все категории");
                if (i >= 2)
                    Tip.Items.RemoveAt(0);
                Tip.SelectedIndex = 0;
            }
            catch
            {
                await Navigation.PushAsync(new NonInternet());
            }

        }
        List<Kategor> kategors = new List<Kategor>();
        public async void Pod_tip(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string response = e.Result;
                kategors = JsonConvert.DeserializeObject<List<Kategor>>(response);

                WebClient client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(View_Sklad_Load);
                kategor.Clear();
                kategor.Add("Все категории", new List<string> { "Все подкатегории" });
                Tip.Items.Add("Все категории");
                client.DownloadStringAsync(new Uri($"{Hold.Adress}/api/View_Sklad"));
                int i = Tip.Items.Count(p => p == "Все категории");
                if (i >= 2)
                    Tip.Items.RemoveAt(0);
            }
            catch
            {
                await Navigation.PushAsync(new NonInternet());
            }
        }


        private async void Load()
        {
            var client = new WebClient();
            kategor.Clear();
            Tip.Items.Clear();
            kategors.Clear();
            PodTip.Items.Clear();
            string s = $"{Hold.Adress}/api/View_PodTip";
            try
            {
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Pod_tip);
                client.DownloadStringAsync(new Uri( $"{Hold.Adress}/api/View_PodTip"));

                
            }
            catch
            {
               await  Navigation.PushAsync(new NonInternet());

            }

            PodTip.Items.Add("Все подкатегории");
            Tip.SelectedIndex = 0;
            PodTip.SelectedIndex = 0;
        }
        public async void DownloadNazenka(object sender, DownloadStringCompletedEventArgs e)
        {
            string n = e.Result;
            nazenka = Convert.ToDouble(n.Substring(1, n.LastIndexOf('\"') - 1));
            Hold.Nazenka = nazenka;

        }
        double nazenka = Hold.Nazenka;

        public AboutPage()
        {
            InitializeComponent();
            try
            {
                var client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadNazenka);

                client.DownloadStringAsync(new Uri($"{Hold.Adress}/api/Nazenka"));
            }
            catch
            {
                Navigation.PushAsync(new NonInternet());

            }
            base.OnAppearing();
            if (!Hold.Aitoriz)
                return;
            Tip.Items.Clear();
            PodTip.Items.Clear();
            kategor.Clear();
            kategors.Clear();
            Load();
            //try
            //{
            //    var client = new WebClient();
            //    client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadNazenka);

            //     client.DownloadStringAsync(new Uri($"{Hold.Adress}/api/Nazenka"));
            //}
            //catch
            //{
            //    Navigation.PushAsync(new NonInternet());

            //}
            AoutoRis();
            //if (!Hold.Aitoriz)
            //    return;
            //Tip.Items.Clear();
            //PodTip.Items.Clear();
            //kategor.Clear();
            //kategors.Clear();
            //Load();
            //DatesTovar.ItemsSource = JsonConvert.DeserializeObject<List<Kategor>>(response);
        }
        protected override void OnAppearing()
        {
            //try
            //{
            //    var client = new WebClient();
            //    client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadNazenka);

            //    client.DownloadStringAsync(new Uri($"{Hold.Adress}/api/Nazenka"));
            //}
            //catch
            //{
            //    Navigation.PushAsync(new NonInternet());

            //}
            //base.OnAppearing();
            //if (!Hold.Aitoriz)
            //    return;
            //Tip.Items.Clear();
            //PodTip.Items.Clear();
            //kategor.Clear();
            //kategors.Clear();
            //Load();

        }
        private void Tip_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (kategor.Count == 0)
                    return;
                if (Tip.SelectedItem == null)
                    return;
                PodTip.Items.Clear();
                List<string> str = kategor[Tip.SelectedItem.ToString()];
                foreach (string s in str)
                {
                    PodTip.Items.Add(s);
                }
                PodTip.SelectedIndex = 0;
                Sorter();
            }
            catch
            {

            }
            

        }

        private  async void DatesTovar_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //DisplayAlert("d", $"{(DatesTovar.SelectedItem as TovarS).Koll}", "d");
            await Navigation.PushAsync(new TovarPage((DatesTovar.SelectedItem as TovarS)));
        }


        public async void Sklad_Tov(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {

                string responTovs = e.Result;
                var responTov = JsonConvert.DeserializeObject<List<TovarS>>(responTovs);
                if (Tip.SelectedItem != null && PodTip.SelectedItem != null)
                    if (Tip.SelectedItem.ToString() != "Все категории")
                    {
                        if (PodTip.SelectedItem.ToString() != "Все подкатегории")
                        {
                            responTov = responTov.Where(p => p.Tip == Tip.SelectedItem.ToString()).ToList();
                            responTov = responTov.Where(p => p.PodTip == PodTip.SelectedItem.ToString()).ToList();
                        }
                        else
                        {
                            responTov = responTov.Where(p => p.Tip == Tip.SelectedItem.ToString()).ToList();

                        }
                    }
                if (NameTovar.Text != "" && NameTovar.Text != " " &&
                    Tip.SelectedItem != null && PodTip.SelectedItem != null
                    && NameTovar.Text != null)
                {
                    responTov = responTov.Where(p => p.Tovar.ToLower().Contains(NameTovar.Text.ToLower())).ToList();
                }

                DatesTovar.ItemsSource = responTov;
            }
            catch
            {
               await Navigation.PushAsync(new NonInternet());
            }
        }

        async void  Sorter()
        {
            try
            {
                var client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Sklad_Tov);
                client.DownloadStringAsync(new Uri($"{Hold.Adress}/api/View_Sklad"));
               
            }
            catch
            {
                await Navigation.PushAsync(new NonInternet());

            }
           
        }

        private void PodTip_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sorter();
        }

        private void NameTovar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sorter();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Spravka());

        }

        private void Image_SizeChanged(object sender, EventArgs e)
        {
            int max = 500;
            (sender as Image).WidthRequest = max;
            if((sender as Image).Width > max)
            {
                (sender as Image).WidthRequest = max;
            }

        }
    }
}