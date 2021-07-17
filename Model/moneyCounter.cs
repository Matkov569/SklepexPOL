using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SklepexPOL.Model
{
    class moneyCounter
    {
        //funkcja obliczająca zysk ze sprzedaży
        public double stonksCalc(Dictionary<string, double[]> soldList, double margin)
        {
            double income = 0;
            double[] helper;
            foreach (KeyValuePair<string, double[]> produkt in soldList)
            {
                helper = produkt.Value;
                //0 - ilość, 1 - cena, 2 - podatek, 3 - marża dostawcy
                //zysk = ilość * (cena = cenaO * marżaD * 1+podatek) * 1+marża * 1+podatek + 
                //(ilość * cena * 1+marża * podatek - ilość * cenaO * 1+marża dostawcy * podatek)
                double c = helper[1] * (1 + helper[3]) * (1 + helper[2]);
                double icm = helper[0] * c * (1 + margin);
                income += icm * (1 + helper[2]) + (icm * helper[2] - helper[0] * helper[1] * (1 + helper[3]) * helper[2]);
            }

            return income;
        }
    }
}
