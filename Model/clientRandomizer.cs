using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SklepexPOL.Model
{
    using View;
    using ViewModel;
    using ViewModel.BaseClass;
    //tworzenie losowego ruchu klientów w sklepie i ich zakupów
    class clientRandomizer : BaseViewModel
    {
        //liczba klientów = int(liczba z bazy + ((x% w zależności od stanu sklepu) * 
        //(stosunek marży maksymalnej do marży w sklepie) * 
        //(współczynnik dnia tygodnia/dnia wypłaty/świąt)))

        //ilość kupionych produktów = int( liczba klientów * rand(10 +- 4) * 
        //((y% w zależności od poziomu sklepu) + (x% w zależności od stanu sklepu) + 
        //(współczynnik dnia tygodnia/wypłaty/świąt))

        //generator ilości klientów
        public int clients(DateTime date, int clientsCount, double margin, int state, int level)
        {
            var k = clientsCount + (stateDeppender(state) * marginDeppender(margin,level) * dayDeppender(date));
            return (int)k;
        }
        //generator ilości kupionych produktów (zbędne)
        public int products(int clients, int level, int state, DateTime date)
        {
            Random random = new Random();
            var p = clients * random.Next(3, 11) * (lvlDeppender(level) + stateDeppender(state) + dayDeppender(date));
            return (int)p;
        }


        //generator kupionych przedmiotów - ile czego kupiono
        public Dictionary<string, int[]> shopListGenerator(int customers, Dictionary<string, int[]> onHouseItems, int level, int state, DateTime date)
        {
            double wspolczynnik = lvlDeppender(level) * stateDeppender(state) * dayDeppender(date);
            double trapez = trapezeFunction(0, 8, 8, 8, wspolczynnik);
            Random random = new Random();
            List<int> sizes = new List<int>();
            foreach (KeyValuePair<string, int[]> item in onHouseItems)
            {
                int rand = (int)((random.NextDouble() * ((int)(item.Value[0]/customers) - trapez) + trapez)*customers);
                item.Value[0] = rand;
            }
            return onHouseItems;
        }

        //współczynnik marży sklepu
        private double marginDeppender(double margin, int level)
        {
            if (level == 1) return (0.8/margin);
            else if (level == 1) return (0.7/margin);
            else if (level == 3) return (0.6/margin);
            else if (level == 4) return (0.5/margin);
            else if (level == 5) return (0.4/margin);
            else return 0;
        }
        //współczynnik stanu sklepu
        private double stateDeppender(int state)
        {
            //zły stan
            if (state == 0) return -0.5;
            //średni
            if (state == 1) return 1;
            //dobry
            if (state == 2) return 1.5;
            //świetny
            else return 2;
        }
        //współczynnik poziomu sklepu
        private double lvlDeppender(int level)
        {
            if (level == 1) return 1;
            else if (level == 2) return 1.25;
            else if (level == 3) return 1.5;
            else if (level == 4) return 1.75;
            else if (level == 5) return 2;
            else return 0;
        }
        //współczynnik dnia tygodnia, wypłaty i świąt
        private double dayDeppender(DateTime date)
        {
            int day = date.Day;
            int month = date.Month;
            //wypłata 10 - każdego miesiąca
            double x = 1 + trapezeFunction(9, 10, 12, 25, day);
            
            //święta 
            double y = 1;
            //spadek po sylwestrze
            if (month == 1) y += trapezeFunction(1, 1, 1, 3, day);
            //walentynki,
            if (month == 2) y += trapezeFunction(12, 14, 14, 15, day); 
            //wielkanoc, 
            else if (month == 3 && day > 25) y += trapezeFunction(25, 29, 31, 31, day);
            //majówka,
            else if (month == 4 && day > 25) y += trapezeFunction(25, 28, 30, 30, day);
            //dzień dziecka,
            else if (month == 5 && day > 27) y += trapezeFunction(27, 31, 31, 31, day);
            //wszystkich świętych, 
            else if (month == 10 && day > 26) y += trapezeFunction(26, 31, 31, 31, day);
            //święto niepodległości, 
            else if (month == 11 && day > 5) y += trapezeFunction(6, 9, 10, 10, day);
            //mikołajki,
            else if (month == 12 && day > 3 && day < 8) y += trapezeFunction(3, 5, 6, 8, day);
            //boże narodzenie,
            else if (month == 12 && day > 18 && day < 26) y += trapezeFunction(18, 22, 23, 25,day);
            //sylwester
            else if (month == 12 && day>26) y += trapezeFunction(26, 30, 31, 31, day);

            //poniedziałek - mały ruch, piątek, sobota - wielkie zakupy
            double z = 1 + trapezeFunction(1, 5, 6, 7, (int)date.DayOfWeek);

            //min - 1, max - 8 -> min 1, max 2
            return (1 + trapezeFunction(1, 8, 8, 8, (x * y * z)));
        }
        private double trapezeFunction(double a, double b, double c, double d, double x)
        {
            if (x < a) return 0;
            else if (x >= a && x < b) return (x - a)/(b - a);
            else if (x >= b && x <= c) return 1;
            else if (x > c && x < d) return (d - x)/(d - c);
            else return 0;
        }
    }
}
