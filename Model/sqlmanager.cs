using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SklepexPOL.Model
{
    using ViewModel;
    using ViewModel.BaseClass;
    class sqlmanager : BaseViewModel
    {
        //łączenie sie z bazą
        //sprawdzanie czy jest baza
        //sprawdzanie czy jest save

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
        //poziom sklepu
        //stan sklepu
        //liczba pracowników


    }
}
