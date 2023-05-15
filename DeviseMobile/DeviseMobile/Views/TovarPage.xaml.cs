using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration;

namespace DeviseMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TovarPage : ContentPage
    {
        TovarS _tovarS = null;
        int koll = 1;
        public TovarPage(TovarS tovarOpisan)
        {
            InitializeComponent();
            _tovarS = tovarOpisan;
            Tovaars();

            this.Opisans.Text = _tovarS.Opis;
        }


        void Tovaars()
        {

            this.Opisans.Text = _tovarS.Opisanie;
            this.Img.Source = _tovarS.Photo;
            this.Tovarss.Text = _tovarS.Tovar;
            this.Coll.Text = "Осталось " + _tovarS.KollSh;
            this.Zena.Text = _tovarS.ZenaR;

        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                string st = Preferences.Get("Zakaz", "0");
                string f = st.Substring(0, st.LastIndexOf('*'));
                int nakl = Convert.ToInt32(f);
                int tov = _tovarS.ID_Tovar;
                var client = new WebClient();
                var sz = _tovarS.Zena * koll;
                var s = client.DownloadString($"{Hold.Adress}/api/post?ID_Nakl={nakl}&ID_Tov={tov}&Koll={koll}&Zena={_tovarS.Zena}&Summ={sz}");
                if (s == "1")
                    DisplayAlert("Внимание!", "Такой товар уже есть. Удалите его из корзины.", "ОК");
                else if(s == "-1")
                    DisplayAlert("Внимание!", "Вы уже приобретали товар. Для безопастности перезайлите в аккаунт.", "ОК");

            }
            catch
            {
                DisplayAlert("Внимание!", "Вы уже приобретали товар. Для безопастности перезайлите в аккаунт.", "ОК");

            }
          
        }

        void Obnov()
        {
            this.Kollss.Text = koll.ToString();
        }
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if(koll!=1)
                koll--;
            Obnov();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            if (koll < Convert.ToInt32(_tovarS.Koll))
                koll++;
            Obnov();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            if ((sender as Switch).IsToggled == true)
            {
                this.Opisans.Text = _tovarS.Opisanie;
                TogledStatus.Text = "Раскрыто";
            }
            else
            {
                this.Opisans.Text = _tovarS.Opis;
                TogledStatus.Text = "Скрыто";
            }
        }
    }
    public abstract class bb 
    { 
        public bb()
        {

        }
    }
}