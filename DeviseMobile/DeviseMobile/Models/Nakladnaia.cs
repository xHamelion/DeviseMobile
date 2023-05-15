using System;
using System.Collections.Generic;
using System.Text;

namespace DeviseMobile
{
    public class Rootobject3
    {
        public Nakladnaia[] Property1 { get; set; }
    }

    public class Nakladnaia
    {
       
            public int ID_Naklad_Prodag { get; set; }
            public int ID_Pokup { get; set; }
            public int ID_Sotrudnic { get; set; }
            public DateTime Data_Pokup { get; set; }
            public string Nomer { get; set; }
            public float Summa { get; set; }
            public float Dostavka { get; set; }
            public float Summa_Obh { get; set; }
            public int? ID_InternetUser { get; set; }
         

    }
}
