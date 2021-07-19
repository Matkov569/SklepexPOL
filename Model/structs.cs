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
            public int ID;
            public string Name;
            public int Days;
            public int Count;
            public nowe(int _id, string _name, int _days, int _count)
            {
                ID = _id;
                Name = _name;
                Days = _days;
                Count = _count;
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
    }
}
