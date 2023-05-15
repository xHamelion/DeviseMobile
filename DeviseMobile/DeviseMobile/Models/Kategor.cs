using System;
using System.Collections.Generic;
using System.Text;

namespace DeviseMobile
{
 
    public class Rootobject2
    {
        public Kategor[] Property1 { get; set; }
    }

    public class Kategor
    {
        public int ID_Tip { get; set; }
        public int ID_PodTip { get; set; }
        public string Tip { get; set; }
        public string PodTip { get; set; }
    }

}
