using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SklepexPOL.Model
{
    class stringConventer
    {
        //procenty
        public string percentage(double percent)
        {
            return (percent * 100).ToString() + "%";
        }
        //pieniądze
        public string money(double money)
        {
            return Math.Round(money,2).ToString() + " $";
        }

    }
}
