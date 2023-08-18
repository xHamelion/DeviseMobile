using DeviseMobile.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DeviseMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private void Swit_Toggled(object sender, ToggledEventArgs e)
        {

        }

        private void Swit_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            this.Tel.IsVisible = false;
            this.eMail.IsVisible = false;
            this.FIO.IsVisible = false;
            BTNlog.Text = "Вход";
        }

        private void RadioButton_CheckedChanged_1(object sender, CheckedChangedEventArgs e)
        {
            this.Tel.IsVisible = true;
            this.eMail.IsVisible = true;
            BTNlog.Text = "Регистрация";
            this.FIO.IsVisible = true;
        }

        public async void Aadd_Zakaz(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {

                string otv = e.Result;



                var Zakaz = JsonConvert.DeserializeObject<Nakladnaia>(otv);
                if (!Preferences.ContainsKey("Zakaz"))
                    Preferences.Set("Zakaz", $"{Zakaz.ID_Naklad_Prodag}*{Zakaz.Nomer}");
                else
                {
                    Preferences.Remove("Zakaz");
                    Preferences.Set("Zakaz", $"{Zakaz.ID_Naklad_Prodag}*{Zakaz.Nomer}");
                }

            }
            catch
            {
                await Navigation.PushAsync(new NonInternet());

            }
        }


        async void AddZakaz(int r)
        {
            try
            {

                var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                Nakladnaia nakladnaia = new Nakladnaia();
                nakladnaia.Data_Pokup = DateTime.Now;
                nakladnaia.Dostavka = 0;
                nakladnaia.ID_InternetUser = r;
                nakladnaia.ID_Pokup = 14;
                nakladnaia.ID_Sotrudnic = 6;
                string s = "";
                Random rnd = new Random();
                for (int i = 0; i <= 7; i++)
                    s += (char)(rnd.Next(65, 90));
                if (!Preferences.ContainsKey("KeyD"))
                    Preferences.Set("KeyD", $"{ s}");
                else
                {
                    Preferences.Remove("KeyD");
                    Preferences.Set("KeyD", $"{ s}");
                }
                nakladnaia.Nomer = s;
                nakladnaia.Summa = 0;
                nakladnaia.Summa_Obh = 0;
                string jsonss = JsonConvert.SerializeObject(nakladnaia);
                jsonss = JsonConvert.SerializeObject(nakladnaia);
                client.UploadStringCompleted += new UploadStringCompletedEventHandler(Aadd_Zakaz);
                client.UploadStringAsync(new Uri($"{Hold.Adress}/api/Naklad_Prodag"), JsonConvert.SerializeObject(nakladnaia));

            }
            catch
            {
                await Navigation.PushAsync(new NonInternet());

            }

        }

        public async void Naklad_Prodag(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                
                string otvs = e.Result;
                string Zakaz = "";
                string numer = "";
                if (otvs != "-1")
                {
                    Zakaz = otvs.Substring(1, otvs.Length - otvs.LastIndexOf('*'));
                    numer = otvs.Substring(otvs.LastIndexOf('*') + 1, otvs.Length - otvs.LastIndexOf('*') - 2);
                    Preferences.Set("KeyD", $"{numer}");
                }


                if (otvs == "-1")
                {
                    AddZakaz(ID_userLocal);
                }
                else
                {
                    if (!Preferences.ContainsKey("Zakaz"))
                        Preferences.Set("Zakaz", $"{Zakaz} ");
                    else
                    {
                        Preferences.Remove("Zakaz");
                        Preferences.Set("Zakaz", $"{Zakaz} ");
                    }
                }

                //await DisplayAlert("2", $"{ID_User}-{Zakaz}","j");
                Hold.Aitoriz = true;
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            catch
            {
                await Navigation.PushAsync(new NonInternet());

            }

        }
        public async void New_User(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                string result = e.Result;
                var r = JsonConvert.DeserializeObject<User>(result);
                AddZakaz(r.ID_InternetUser);
                if (!Preferences.ContainsKey("ID"))
                    Preferences.Set("ID", $"{r.ID_InternetUser}");
                else
                {
                    Preferences.Remove("ID");
                    Preferences.Set("ID", $"{r.ID_InternetUser}");
                }
                Hold.Aitoriz = true;
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            catch
            {
                await Navigation.PushAsync(new NonInternet());

            }
            
        }

        public async void Reg_Login(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string response = e.Result;
                int t = 0;
                t = Convert.ToInt32(response.Substring(1, 1));
                if (t == 0)
                {
                    User newUser = new User();
                    if (Login.Text != "" && Pass.Text != "")
                    {
                        if (Fam.Text != "" && Fam.Text != null)
                            newUser.Familia = Fam.Text;
                        else
                            newUser.Familia = "-";
                        if (Name.Text != "" && Fam.Text != null)
                            newUser.Imia = Name.Text;
                        else
                            newUser.Imia = "-";
                        newUser.eMail = eMail.Text;
                        newUser.Telefon = Tel.Text;
                        newUser.Login = Login.Text;
                        newUser.Pass = Pass.Text;
                    var cl = new WebClient();
                        cl.UploadStringCompleted += new UploadStringCompletedEventHandler(New_User);
                        cl.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                        string jsons = JsonConvert.SerializeObject(newUser);
                        jsons = JsonConvert.SerializeObject(newUser);
                        cl.UploadStringAsync(new Uri($"{Hold.Adress}/api/InternetUsers"), JsonConvert.SerializeObject(newUser));

                    }
                    else
                    {
                        await DisplayAlert("Внимание", "Заполните обязательные поля. Регистрация не завершена.", "Ок");

                    }

                }
                else
                {
                    await DisplayAlert("Внимание", "Подобный логин уже сушествует. Регистрация не завершена.", "Ок");

                }
            }
            catch
            {
                await Navigation.PushAsync(new NonInternet());

            }

        }

        int ID_userLocal = 0;
        public async void AuthorAsinc(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {

                var response = e.Result;
                int t = Convert.ToInt32(response.Substring(1, 1));
                if (RBvhod.IsChecked)
                {
                    string d = response.Substring(3, (response.Length - response.LastIndexOf('-')) - 2);
                    if (t != 0)
                    {
                        int ID_User = Convert.ToInt32(d);
                        if (!Preferences.ContainsKey("ID"))
                        {
                            Preferences.Set("ID", $"{ID_User}");
                        }
                        else
                        {
                            Preferences.Remove("ID");
                            Preferences.Set("ID", $"{ID_User}");
                        }
                        ID_userLocal = ID_User;
                        WebClient naklad = new WebClient();
                        naklad.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Naklad_Prodag);
                        naklad.DownloadStringAsync(new Uri( $"{Hold.Adress}/api/Naklad_Prodag/{ID_User}"));
                    }
                    else
                    {
                        await DisplayAlert("Внимание", "Логин или пороль не правильный. Повторите попытку", "Ок");
                    }///!!!!
                }
                else
                {
                    WebClient clin = new WebClient();
                    clin.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Reg_Login);
                    Uri uri = new Uri($"{Hold.Adress}/api/getLog?Log=" +
                        "{" +
                        $"{Login.Text}" +
                        "}");
                    clin.DownloadStringAsync(uri);
                    

                }
            }
            catch
            {
                await Navigation.PushAsync(new NonInternet());

            }
        }
        WebClient client;
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadStringCompleted += new
        DownloadStringCompletedEventHandler(AuthorAsinc);
                client.DownloadStringAsync(new Uri($"{Hold.Adress}/api/getLog?Log={Login.Text}&Pass={Pass.Text}"));

            }
            catch
            {
                await Navigation.PushAsync(new NonInternet());

            }
            
           

        }
    }
}