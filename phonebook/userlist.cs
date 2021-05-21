using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phonebook
{
    class userlist
    {
        private List<user> Users;
        public userlist()
        {
            Users = new List<user>();
        }

        public bool AddUser(user Rec)
        {

            for (int i = 0; i < Users.Count; i++)
            {
                if (Rec.Equals(Users[i]))
                {
                    return false;
                }
            }

            Users.Add(Rec);
            return true;
        }

        public int UsersCount()
        {
            return this.Users.Count;
        }

        public List<int> SearchRecName(string name)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].getName().ToLower().Contains(name.ToLower()))
                {
                    ids.Add(i);
                }
            }

            return ids;
        }

        public void ClearUsers()
        {
            Users.Clear();
        }

        public user getUser(string id)
        {
            int get = -1;
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].getRecId() == id)
                {
                    get = i;
                    break;
                }
            }
            return Users[get].getRec();
        }

        public List<int> SearchRecPhonenumber(string phonenumber)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].getPhonenumber() == phonenumber)
                {
                    ids.Add(i);
                }
            }
            return ids;
        }
        public void DeleteByIndex(int index)
        {
            Users.RemoveAt(index);
            for (int i = index; i < Users.Count; i++)
            {
                Users[i].setId((Convert.ToInt32(Users[i].getRecId()) - 1).ToString());
            }
        }
        public string getName(int index)
        {
            return Users[index].getName();
        }
        public string getPhonenumber(int index)
        {
            return Users[index].getPhonenumber();
        }
        public void DeleteByValue(string phonenumber)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].getPhonenumber() == phonenumber)
                {
                    Users.RemoveAt(i);
                    break;
                }
            }
        }
        public List<user> getUsers()
        {
            return Users;
        }

        public void setUser(string id, string name, string phonenumber)
        {
            Users[Convert.ToInt32(id) - 1].setId(id);
            Users[Convert.ToInt32(id) - 1].setRecName(name);
            Users[Convert.ToInt32(id) - 1].setRecPhonenumber(phonenumber);
        }

        ~userlist()
        {
            ClearUsers();
        }

    }
}
