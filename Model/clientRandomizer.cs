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
    using System.Security.Cryptography;
    //tworzenie losowego ruchu klientów w sklepie i ich zakupów
    class clientRandomizer : BaseViewModel
    {
        //generator kupionych przedmiotów - ile czego kupiono
        public Dictionary<string, double[]> shopListGenerator(int customers, Dictionary<string, double[]> onHouseItems, int level, DateTime date)
        {
            Dictionary<string, double[]> dict = new Dictionary<string, double[]>();
            int sum;
            int val;
            int iter;
            uint rand;
 
            foreach (KeyValuePair<string,double[]> item in onHouseItems)
            {
                dict.Add((string)item.Key.Clone(), (double[])item.Value.Clone());
            }
            foreach(KeyValuePair<string,double[]> item in dict)
            {
                item.Value[0] = 0;
            }
            double wsp = lvlDeppender(level) * dayDeppender(date) / (dict.Count-(dict.Count>1?1:0));
            //Console.WriteLine(wsp);
            for (int i = 0; i < customers; i++)
            {
                sum = 0;
                iter = 0;
                do
                {
                    foreach (KeyValuePair<string,double[]> item in dict)
                    {
                        RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
                        var bytes = new byte[4]; 
                        random.GetBytes(bytes);
                        var randomInt = BitConverter.ToUInt32(bytes, 0);
                        if (onHouseItems[item.Key][0] != 0)
                        {
                            rand = randomInt % 2;
                            val = (int)(((randomInt % dict.Count) + rand) * wsp);
                            if (val > onHouseItems[item.Key][0])
                                val = 0;
                            sum += val;
                            item.Value[0] += val;
                        } 
                    }
                    iter++;
                } while (sum == 0 && iter<3);
            }
            return dict;
        }

        //współczynnik marży sklepu
        private double marginDeppender(double margin, int level)
        {
            double x, y;

            if (level == 1) x = 0.8;
            else if (level == 2) x = 0.7;
            else if (level == 3) x = 0.6;
            else if (level == 4) x = 0.5;
            else if (level == 5) x = 0.4;
            else return 0;

            y = margin - x;

            if (y > 0) return Math.Abs(y / x);
            else if (y == 0) return 0.99;
            else
            {
                if (level == 1) return 1.05;
                else if (level == 2) return 1.0002;
                else if (level == 3) return 1.00005;
                else if (level == 4) return 1.00001;
                else return 1.000001;
            }
        }
        //współczynnik stanu sklepu
        private double stateDeppender(int state, int level, int clients)
        {
            //zły stan
            if (state == 0) return 0.9999;
            //średni
            else if (state == 1) return 1;
            //dobry
            else if (state == 2)
            {
                if (level == 1 && clients < 20) return 1.045;
                else return 1.02; 
            }
            //świetny
            else return 1.03;
        }
        //współczynnik poziomu sklepu
        private double lvlDeppender(int level)
        {
            //poprawić
            if (level == 1) return 1;
            else if (level == 2) return 1.0001;
            else if (level == 3) return 1.0002;
            else if (level == 4) return 1.0003;
            else if (level == 5) return 1.0004;
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
            else if (x >= a && x < b) return (x - a) / (b - a);
            else if (x >= b && x <= c) return 1;
            else if (x > c && x < d) return (d - x) / (d - c);
            else return 0;
        }
        private double[] dayDep(DateTime date)
        {
            int day = date.Day;
            int month = date.Month;
            //wypłata 10 - każdego miesiąca
            double x = trapFunction(8, 10, 12, 14, day);

            //święta 
            double y = 0;
            //spadek po sylwestrze
            if (month == 1) y = trapFunction(1, 1, 1, 4, day);
            //walentynki,
            else if (month == 2) y = trapFunction(12, 14, 14, 16, day);
            //wielkanoc, 
            else if (month == 3 && day > 25) y = trapFunction(25, 29, 31, 31, day);
            else if (month == 4 && day < 6) y = trapFunction(0, 0, 0, 4, day);
            //majówka,
            else if (month == 4 && day > 25) y = trapFunction(25, 28, 30, 30, day);
            else if (month == 5 && day < 5) y = trapFunction(0, 0, 0, 3, day);
            //dzień dziecka,
            else if (month == 5 && day > 27) y = trapFunction(27, 31, 31, 31, day);
            else if (month == 6 && day < 7) y = trapFunction(0, 0, 0, 6, day);
            //wszystkich świętych, 
            else if (month == 10 && day > 26) y = trapFunction(26, 31, 31, 31, day);
            else if (month == 11 && day < 6) y = trapFunction(0, 0, 0, 5, day);
            //święto niepodległości, 
            else if (month == 11 && day > 5) y = trapFunction(6, 9, 10, 13, day);
            //mikołajki,
            else if (month == 12 && day > 3 && day < 8) y = trapFunction(3, 5, 6, 8, day);
            //boże narodzenie,
            else if (month == 12 && day > 18 && day < 26) y = trapFunction(18, 22, 23, 27, day);
            //sylwester
            else if (month == 12 && day > 26) y = trapFunction(28, 31, 31, 31, day);

            //poniedziałek - mały ruch, piątek, sobota - wielkie zakupy
            double z;
            if ((int)date.DayOfWeek == 0)
                z = trapFunction(-1, -1, -1, 1, (int)date.DayOfWeek);
            else
                z = trapFunction(3, 5, 6, 7, (int)date.DayOfWeek);

            //min - 1, max - 8 -> min 1, max 2
            double[] ret = new double[] { x, y, z };
            return ret;
        }
        private double trapFunction(double a, double b, double c, double d, double x)
        {
            //b-a == d-c
            if (x < a) return 0;
            else if (x >= a && x < b) return (x - a) / (b - a);
            else if (x >= b && x <= c) return 0;
            else if (x > c && x < d)
            {
                return -(d - x) / (1.2 * (d - c));
            }
            else return 0;
        }

        //liczba klientów jednorazowych
        private int holidayCustomers = 0;
        private int weekCustomers = 0;
        private int paydayCustomers = 0;

        //generowanie liczby klientów
        public int clientsCreator(int clients, double margin, int state, int level, DateTime date)
        {
            double k = clients;
            double[] wsp = dayDep(date);
            //Console.WriteLine(wsp[0]+" "+wsp[1]+" "+wsp[2]);
            int clientel = clients - holidayCustomers - weekCustomers - paydayCustomers;
            k = k * stateDeppender(state, level, clientel) * marginDeppender(margin, level);
            int holi = (int)(clientel * wsp[1])/3;
            int week = (int)(clientel * wsp[2])/3;
            int payd = (int)(clientel * wsp[0])/3;
            holidayCustomers = holi;
            weekCustomers = week;
            paydayCustomers = payd;
            k += holi;
            k += week;
            k += payd;
            return (int)k;
        }

    }
}
