using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SklepexPOL.DAL
{
    class NaStanie
    {
        public string Nazwa { get; set; }
        public int Ilosc { get; set; }
        public int Cena { get; set; }
        public int Wysokosc { get; set; }
        public int Marza { get; set; }
        public int Magazyn { get; set; }

        public Country(string nazwa, int ilosc, int cena, int wysokosc, int marza, int magazyn)
        {
            Nazwa = nazwa;
            Ilosc = ilosc;
            Cena = cena;
            Wysokosc = wysokosc;
            Marza = marza;
            Magazyn = magazyn;

        }

        public override string ToString()
        {
            return $"{Nazwa} {Ilosc} {Cena} {Wysokosc} {Marza} {Magazyn}";
        }
    }

}

