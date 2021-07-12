using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SklepexPOL.Model
{
    using ViewModel;
    using ViewModel.BaseClass;
    class shopInfo : BaseViewModel
    {
        //co jest na stanie 
        //{"produkt":[ilosc, cena, wysokość podatku, marża dostawcy, termin ważności(ile dni zostało)]}
        private Dictionary<string, int[]> onHouseItems;
        public Dictionary<string, int[]> OnHouseItems
        {
            get { return onHouseItems; }
            set
            {
                onHouseItems = value;
                onPropertyChanged(nameof(OnHouseItems));
            }
        }

        //ilość klientów
        private int todayClientsValue;
        public int TodayClientsValue
        {
            get { return todayClientsValue; }
            set
            {
                todayClientsValue = value;
                onPropertyChanged(nameof(TodayClientsValue));
            }
        }
        //wartość sprzedanych przedmiotów
        private int todaySellValue;
        public int TodaySellValue
        {
            get { return todaySellValue; }
            set
            {
                todaySellValue = value;
                onPropertyChanged(nameof(TodaySellValue));
            }
        }
        //lista sprzedanych przedmiotów
        private Dictionary<string, int[]> soldItems;
        public Dictionary<string, int[]> SoldItems
        {
            get { return soldItems; }
            set
            {
                onHouseItems = value;
                onPropertyChanged(nameof(SoldItems));
            }
        }
        //string sprzedanych przedmiotów
        public string TodaySoldItems
        {
            get {
                string t = "";
                foreach (KeyValuePair<string, int[]> key in SoldItems)
                    t += key.Key + " : " + key.Value[0].ToString() + "\n";
                return t; 
            }            
        }
        //poziom sklepu
        private int shopLevel;
        public int ShopLevel
        {
            get { return shopLevel; }
            set
            {
                shopLevel = value;
                onPropertyChanged(nameof(ShopLevel));
            }
        }
        //stan sklepu
        private int shopState;
        public int ShopState
        {
            get { return shopState; }
            set
            {
                shopState = value;
                onPropertyChanged(nameof(ShopState));
            }
        }
        //liczba pracowników
        //marża sklepu
        private int shopMargin;
        public int ShopMargin
        {
            get { return shopMargin; }
            set
            {
                shopMargin = value;
                onPropertyChanged(nameof(ShopMargin));
            }
        }


    }
}
