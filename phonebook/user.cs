using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phonebook
{
    class user
    {

        private string id, phonenumber;
        private string name;
        public user(string id, string name, string phonenumber)
        {
            this.id = id;
            this.name = name;
            this.phonenumber = phonenumber;
        }

        public bool Equals(user Rec)
        {
            if (this.phonenumber == Rec.phonenumber && this.name == Rec.name)
                return true;

            return false;
        }

        public user getRec()
        {
            return this;
        }

        public void setRecName(string name)
        {
            this.name = name;
        }

        public void setId(string id)
        {
            this.id = id;
        }

        public void setRecPhonenumber(string phonenumber)
        {
            this.phonenumber = phonenumber;
        }

        public string getRecId()
        {
            return this.id;
        }


        public string getName()
        {
            return this.name;
        }

        public string getPhonenumber()
        {
            return this.phonenumber;
        }

        ~user()
        {
        }
    }
}
