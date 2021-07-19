using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SklepexPOL.Model
{
    class structs
    {
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
        public struct zamowienie
        {
            public int ID;
            public string Name;
            public int Days;
            public int Count;
            public zamowienie(int _id, string _name, int _days, int _count)
            {
                ID = _id;
                Name = _name;
                Days = _days;
                Count = _count;
            }
        }
        public class zamowienieHandler
        {
            public zamowienieHandler()
            {
                Items = new List<zamowienie>();
            }

            public List<zamowienie> Items { get; private set; }

            public void Add(zamowienie item)
            {
                Items.Add(item);
            }
        }
        public struct zamowienia
        {
            public int ID;
            public string Name;
            public int Days;
            public int Count;
            public zamowienia(int _id, string _name, int _days, int _count)
            {
                ID = _id;
                Name = _name;
                Days = _days;
                Count = _count;
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
