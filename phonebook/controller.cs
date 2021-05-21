using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace phonebook
{

    class ListComparer : IComparer<List<string>>
    {
        public int Compare(List<string> x, List<string> y)
        {
            int cnt = Math.Max(x.Count, y.Count);
            for (int i = 1; i < cnt; i++)
            {
                if (i == x.Count)
                    return 1;
                if (i == y.Count)
                    return -1;
                int res = String.CompareOrdinal(x[i], y[i]);
                if (res != 0)
                    return res;
            }
            return 0;
        }
    }
    class controller
    {
        public userlist PBook;

        private string path =  Directory.GetCurrentDirectory();
        public controller()
        {
            PBook = new userlist();
        }

        public int UsersCount()
        {
            return PBook.UsersCount();
        }
        public user getUserByid(string id)
        {
            return PBook.getUser(id);
        }

        public bool AddUser(string id, string name, string phonenumber)
        {
            user Rec = new user(id, name, phonenumber);
            bool add = PBook.AddUser(Rec);

            List<List<string>> users = new List<List<string>>();

            for (int i = 0; i < PBook.UsersCount(); i++)
            {
                user rec = PBook.getUser((i + 1).ToString());
                users.Add(new List<string>());
                users[i].Add(rec.getRecId());
                users[i].Add(rec.getName());
                users[i].Add(rec.getPhonenumber());
            }

            users.Sort(new ListComparer());

            PBook.ClearUsers();

            for (int i = 0; i < users.Count; i++)
            {
                user rec = new user((i + 1).ToString(), users[i][1], users[i][2]);
                PBook.AddUser(rec);
            }

            return add;
        }

        public void DeleteUserById(int id)
        {
            PBook.DeleteByIndex(id);
        }

        public void ClearBook()
        {
            PBook.ClearUsers();
        }

        public void Save()
        {
            List<List<string>> users = new List<List<string>>();

            for (int i = 0; i < PBook.UsersCount(); i++)
            {
                user Rec = PBook.getUser((i + 1).ToString());
                users.Add(new List<string>());
                users[i].Add(Rec.getRecId());
                users[i].Add(Rec.getName());
                users[i].Add(Rec.getPhonenumber());
            }

            string jsonBook = Newtonsoft.Json.JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path + @"\data.json", jsonBook);
        }

        public void Load()
        {
            if (File.Exists(path + @"\data.json"))
            {
                string jsonData = File.ReadAllText(path + @"\data.json");
                if (jsonData.Length != 0)
                {
                    List<List<string>> users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<string>>>(jsonData);

                    for (int i = 0; i < users.Count; i++)
                    {
                        user Rec = new user(users[i][0], users[i][1], users[i][2]);
                        PBook.AddUser(Rec);
                    }
                }
            }
        }

        public List<int> SearchByName(string name)
        {
            List<int> ids = PBook.SearchRecName(name);

            return ids;
        }

        public List<int> SearchByPhoneNumber(string phonenumber)
        {
            List<int> ids = PBook.SearchRecPhonenumber(phonenumber);

            return ids;
        }

        public void setUser(user Rec)
        {
            PBook.setUser(Rec.getRecId(), Rec.getName(), Rec.getPhonenumber());
        }
    }
}

