using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SklepexPOL.Model
{
    class interactions
    {
        public bool YesOrNo(string text, string caption)
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result;
            result = MessageBox.Show(text, caption, buttons);
            if (result == MessageBoxResult.Yes) return true;
            else return false;
        }
        public void message(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }
        public void infoBox(string param)
        {
            switch (param)
            {
                case "Type":
                    MessageBox.Show(types, "Rodzaje sklepu");
                    break;
                case "Level":
                    MessageBox.Show(levels, "Poziom sklepu");
                    break;
                case "State":
                    MessageBox.Show(states, "Stan sklepu");
                    break;
                default:
                    break;
            }
        }
        private string types = "Sklep może być rodzaju:\n" +
            "Market - sklep oferujący różnorodny towar,\n" +
            "Spożywczy - sklep oferujący wyłącznie artykuły spożywcze,\n" +
            "Warzywniak - sklep oferujący wyłącznie warzywa i/lub owoce,\n" +
            "RTV AGD - sklep oferujący w większości sprzęt RTV AGD,\n" +
            "Mięsny - sklep oferujący wyłącznie mięso i/lub ryby,\n" +
            "Drogeria - sklep oferujący głównie kosmetyki i/lub artykuły medyczne i higieniczne,\n" +
            "Butik - sklep oferujący wyłącznie ubrania i/lub obuwie,\n" +
            "Papierniczy - sklep oferujący wyłącznie artykuły biurowe, książki i/lub prasę,\n" +
            "Piekarnia - sklep oferujący wyłącznie pieczywo i/lub wyroby cukiernicze,\n" +
            "Ogrodniczy - sklep oferujący w większości rośliny,\n" +
            "Monopolowy - sklep soferujący głównie alkohol.";
        private string levels = "Sklep może mieć następujący poziom:\n" +
            "(0) Pustostan,\n" +
            "(1) Sklepik,\n" +
            "(2) Sklep,\n" +
            "(3) Dyskont,\n" +
            "(4) Supermarket,\n" +
            "(5) Hipermarket.\n";
        private string states = "Sklep może mieć następujący stan:\n" +
            "Zły,\n" +
            "Średni,\n" +
            "Dobry,\n" +
            "Świetny.\n" +
            "\n" +
            "Stan sklepu wpływa na liczbę klientów i wielkość ich zakupów.";
    }
}
