using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SklepexPOL.Model
{
    class structs
    {
        //co jest na stanie
        public struct stan
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Days { get; set; }
            public string Count { get; set; }
            public stan(string _id, string _name, string _days, string _count)
            {
                ID = _id;
                Name = _name;
                Days = _days;
                Count = _count;
            }

        }
        public class stanHandler
        {
            public stanHandler()
            {
                Items = new ObservableCollection<stan>();
            }

            public ObservableCollection<stan> Items { get; private set; }

            public void Add(stan item)
            {
                Items.Add(item);
            }
        }
        //nowe zamówienie
        public struct nowe
        {
            public int ID { get; set; }
            public string PName { get; set; }
            public int Count { get; set; }
            //koszt całkowity, nie jednostkowy!
            public string Cost { get; set; }
            public double Price { get; set; }
            public ICommand Action { get; set; }
            public nowe(int _id, string _pname, int _count, double _price, ICommand _action)
            {
                stringConventer strConv = new stringConventer();
                ID = _id;
                PName = _pname;
                Count = _count;
                Cost = strConv.money(_price);
                Price = _price;
                Action = _action;
            }
        }
        public class noweHandler
        {
            public noweHandler()
            {
                Items = new ObservableCollection<nowe>();
            }

            public ObservableCollection<nowe> Items { get; private set; }
            public void Add(nowe item)
            {
                Items.Add(item);
            }
            public void Remove(nowe item)
            {
                Items.Remove(item);
            }
            public void Remove(int id)
            {
                Items.Remove(Items.Single(r => r.ID == id));
            }
            public void Clear()
            {
                Items.Clear();
            }
            public nowe Item(int index)
            {
                if (index < Items.Count && index >=0)
                    return Items[index];
                else return default(nowe);
            }
            public bool ItemExist(int index)
            {
                foreach(var item in Items)
                {
                    if (item.ID == index) return true;
                }
                return false;
            }
            public int Count()
            {
                return Items.Count();
            }
            public double CostSum()
            {
                double ret = 0;
                foreach (nowe item in Items)
                {
                    ret += item.Price;
                }
                return ret;
            }
            public double SpaceSum()
            {
                double ret = 0;
                foreach (nowe item in Items)
                {
                    ret += item.Count;
                }
                return ret;
            }
        }
        //zamówienia
        public struct zamowienia
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string ODate { get; set; }
            public string DDate { get; set; }
            public string Cost { get; set; }
            public ICommand Action { get; set; }
            public zamowienia(string _id, string _name, string _odate, string _ddate, string _cost, ICommand _action)
            {
                ID = _id;
                Name = _name;
                ODate = _odate;
                DDate = _ddate;
                Cost = _cost;
                Action = _action;
            }
        }
        public class zamowieniaHandler
        {
            public zamowieniaHandler()
            {
                Items = new ObservableCollection<zamowienia>();
            }

            public ObservableCollection<zamowienia> Items { get; private set; }

            public void Add(zamowienia item)
            {
                Items.Add(item);
            }
        }
        //lista dostawców
        public struct dostawcy
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string DDays { get; set; }
            public int DDaysV { get; set; }
            public string Margin { get; set; }
            public double MarginV { get; set; }
            public string Country { get; set; }
            public string TaxName { get; set; }
            public string TaxCost { get; set; }
            public double TaxValue { get; set; }
            public dostawcy(int _id, string _name, int _ddaysv, double _marginv, string _country, string _taxname, double _taxvalue)
            {
                stringConventer strConv = new stringConventer();
                ID = _id;
                Name = _name;
                DDays = _ddaysv.ToString() + " dni";
                DDaysV = _ddaysv;
                Margin = strConv.percentage(_marginv);
                MarginV = _marginv;
                Country = _country;
                TaxName = _taxname;
                TaxCost = strConv.percentage(_taxvalue);
                TaxValue = _taxvalue;
            }
        }
        public class dostawcyHandler
        {
            public dostawcyHandler()
            {
                Items = new ObservableCollection<dostawcy>();
            }

            public ObservableCollection<dostawcy> Items { get; private set; }

            public void Add(dostawcy item)
            {
                Items.Add(item);
            }
        }
        //lista produktów
        public struct produkty
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }
            public double PriceV { get; set; }
            public string TaxName { get; set; }
            public string TaxCost { get; set; }
            public double TaxValue { get; set; }
            public produkty(int _id, string _name, double _pricev, string _taxname, double _taxvalue)
            {
                stringConventer strConv = new stringConventer();
                ID = _id;
                Name = _name;
                Price = strConv.money(_pricev); 
                PriceV = _pricev;
                TaxName = _taxname;
                TaxCost = strConv.percentage(_taxvalue);
                TaxValue = _taxvalue;
            }
        }
        public class produktyHandler
        {
            public produktyHandler()
            {
                Items = new ObservableCollection<produkty>();
            }

            public produkty Item(int index)
            {
                if (index < Items.Count && index >= 0)
                    return Items[index];
                else return default(produkty);
            }

            public ObservableCollection<produkty> Items { get; private set; }

            public void Add(produkty item)
            {
                Items.Add(item);
            }
        }
        //lista list produktów
        public class produktyHandlerList
        {
            public produktyHandlerList()
            {
                Items = new ObservableCollection<produktyHandler>();
            }

            public ObservableCollection<produktyHandler> Items { get; private set; }

            public void Add(produktyHandler item)
            {
                Items.Add(item);
            }

            public produktyHandler Item(int index)
            {
                return index < Items.Count && index >= 0 ? Items[index] : default(produktyHandler);
            }
        }
    }
}
