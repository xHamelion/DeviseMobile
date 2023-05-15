using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace DeviseMobile
{


    public class Rootobject
    {
        public TovarS[] Property1 { get; set; }
    }

    public class TovarS
    {
        public int ID_Sklad { get; set; }
        public int ID_Tovar { get; set; }
        public int ID_Tip { get; set; }
        public int ID_PodTip { get; set; }
        public int Koll { get; set; }
        public int Zena { get; set; }
        public string Tip { get; set; }
        public string PodTip { get; set; }
        public string Tovar { get; set; }
        public string Opisanie { get; set; }
        public string Image { get; set; }
        public string ZenaR
        {
            get
            {
                return $"{(double)Zena*(Hold.Nazenka/100)} руб.";
            }
        }

        

        public string KollSh
        {
            get
            {
                return $"{Koll} шт.";
            }
        }

        public string Opis
        {
            get
            {
                
                int count = 80;
                if(Opisanie.Length > count)
                {
                    return
                        $"{Tovar}\n\n{Opisanie.Substring(0, count)}" + "...";
                }
                else
                {
                    return $"{Tovar}\n\n{Opisanie}";
                }
            }
        }
        public ImageSource Photo
        {
            get
            {
                if (Image != null)
                    return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(Image)));
                else
                    return null;
            }
        }

    }


}
