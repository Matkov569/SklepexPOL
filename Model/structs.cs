using System;
using System.Collections.Generic;
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
                Items = new List<stan>();
            }

            public List<stan> Items { get; private set; }

            public void Add(stan item)
            {
                Items.Add(item);
            }
        }
        //nowe zamówienie
        public struct nowe
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Count { get; set; }
            //koszt całkowity, nie jednostkowy!
            public string Cost { get; set; }
            public double Price { get; set; }
            public ICommand Action { get; set; }
            public nowe(int _id, string _name, int _count, double _price, ICommand _action)
            {
                stringConventer strConv = new stringConventer();
                ID = _id;
                Name = _name;
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
                Items = new List<nowe>();
            }

            public List<nowe> Items { get; private set; }
            public void Add(nowe item)
            {
                Items.Add(item);
            }
            public void Remove(nowe item)
            {
                Items.Remove(item);
            }
            public void Clear()
            {
                Items.Clear();
            }
            public nowe Item(int index)
            {
                if (index < Items.Count)
                    return Items[index];
                else return default(nowe);
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
                Items = new List<zamowienia>();
            }

            public List<zamowienia> Items { get; private set; }

            public void Add(zamowienia item)
            {
                Items.Add(item);
            }
        }
        //lista dostawców
        public struct dostawcy
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string DDays { get; set; }
            public string Margin { get; set; }
            public double MarginV { get; set; }
            public string Country { get; set; }
            public string TaxName { get; set; }
            public string TaxCost { get; set; }
            public double TaxValue { get; set; }
            public dostawcy(string _id, string _name, string _ddays, double _marginv, string _country, string _taxname, double _taxvalue)
            {
                stringConventer strConv = new stringConventer();
                ID = _id;
                Name = _name;
                DDays = _ddays;
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
                Items = new List<dostawcy>();
            }

            public List<dostawcy> Items { get; private set; }

            public void Add(dostawcy item)
            {
                Items.Add(item);
            }
        }
        //lista produktów
        public struct produkty
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }
            public double PriceV { get; set; }
            public string TaxName { get; set; }
            public string TaxCost { get; set; }
            public double TaxValue { get; set; }
            public produkty(string _id, string _name, double _pricev, string _taxname, double _taxvalue)
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
                Items = new List<produkty>();
            }

            public List<produkty> Items { get; private set; }

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
                Items = new List<produktyHandler>();
            }

            public List<produktyHandler> Items { get; private set; }

            public void Add(produktyHandler item)
            {
                Items.Add(item);
            }

            public produktyHandler Item(int index)
            {
                return index < Items.Count ? Items[index] : default(produktyHandler);
            }
        }
    }
}
