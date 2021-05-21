using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace phonebook
{
    public partial class Phonebook : Form
    {
        private controller Book;
        public Phonebook()
        {
            InitializeComponent();
        }

        private void Phonebook_Load(object sender, EventArgs e)
        {
            Book = new controller();
            Book.Load();
            LoadUsers();
            PhoneBookList.View = View.Details;
            PhoneBookList.Columns.Add("id", 50, HorizontalAlignment.Left);
            PhoneBookList.Columns.Add("Имя", 298, HorizontalAlignment.Left);
            PhoneBookList.Columns.Add("Номер", 298, HorizontalAlignment.Left);
        }

        private void LoadUsers()
        {
            for (int i = 0; i < Book.UsersCount(); i++)
            {
                user Rec = Book.getUserByid((i + 1).ToString());
                ListViewItem item = new ListViewItem(new[] { Rec.getRecId().ToString(), Rec.getName(), Rec.getPhonenumber().ToString() }); 
                PhoneBookList.Items.Add(item);
            }
        }

        private bool isNumeric(String str)
        {
            try
            {
                long v = long.Parse(str);
                return true;
            }
            catch (Exception e)
            {
            }
            return false;
        }
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (PhonenumberTextBox.Text.Length == 0 || FIOTextBox.Text.Length == 0)
            {
                MessageBox.Show("Заполните поля ФИО и Номер");
                return;
            }

            if (!isNumeric(PhonenumberTextBox.Text))
            {
                MessageBox.Show("Поле `Номер` содержит некоррктные символы");
                return;
            }

            
            bool add = Book.AddUser((Book.UsersCount() + 1).ToString(), FIOTextBox.Text, PhonenumberTextBox.Text);

            if (!add)
            {
                MessageBox.Show("Такой пользователь уже есть в телефонной книге!");
            }

            PhoneBookList.Items.Clear();

            LoadUsers();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            PhoneBookList.Items.Clear();
            Book.ClearBook();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (PhonenumberTextBox.Text.Length == 0 && FIOTextBox.Text.Length == 0)
            {
                PhoneBookList.Items.Clear();
                LoadUsers();
                return;
            }

            if (PhonenumberTextBox.Text.Length != 0 && FIOTextBox.Text.Length == 0)
            {
                PhoneBookList.Items.Clear();

                List<int> ids = Book.SearchByPhoneNumber(PhonenumberTextBox.Text);

                for (int i = 0; i < ids.Count; i++)
                {
                    user Rec = Book.getUserByid((ids[i] + 1).ToString());
                    ListViewItem item = new ListViewItem(new[] { Rec.getRecId().ToString(), Rec.getName(), Rec.getPhonenumber().ToString() });
                    PhoneBookList.Items.Add(item);
                }

                return;
            }

            if (PhonenumberTextBox.Text.Length == 0 && FIOTextBox.Text.Length != 0)
            {
                PhoneBookList.Items.Clear();

                List<int> ids = Book.SearchByName(FIOTextBox.Text);

                for (int i = 0; i < ids.Count; i++)
                {
                    user Rec = Book.getUserByid((ids[i] + 1).ToString());
                    ListViewItem item = new ListViewItem(new[] { Rec.getRecId().ToString(), Rec.getName(), Rec.getPhonenumber().ToString() });
                    PhoneBookList.Items.Add(item);
                }

                return;
            }

            if (PhonenumberTextBox.Text.Length != 0 && FIOTextBox.Text.Length != 0)
            {
                PhoneBookList.Items.Clear();

                List<int> idP = Book.SearchByPhoneNumber(PhonenumberTextBox.Text);
                List<int> idN = Book.SearchByName(FIOTextBox.Text);
                List<int> idPN = new List<int>();

                for (int i = 0; i < idN.Count; i++)
                {
                    for (int j = 0; j < idP.Count; j++)
                    {
                        if (idN[i] == idP[j])
                        {
                            idPN.Add(idP[j]);
                        }
                    }
                }

                for (int i = 0; i < idPN.Count; i++)
                {
                    user Rec = Book.getUserByid((idPN[i] + 1).ToString());
                    ListViewItem item = new ListViewItem(new[] { Rec.getRecId().ToString(), Rec.getName(), Rec.getPhonenumber().ToString() });
                    PhoneBookList.Items.Add(item);
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите сохранить данные в файл? Вся информация, содержащаяся в файле data.json, будет перезаписана", "Подтверждение действия", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Book.Save();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {

            List<int> ids = new List<int>();
            for (int i = 0; i < PhoneBookList.SelectedItems.Count; i++)
            {
                ids.Add(Convert.ToInt32(PhoneBookList.SelectedItems[i].Text));
            }

            string indexes = "";

            for (int i = 0; i < ids.Count; i++)
            {
                indexes += ids[i].ToString() + " ";
            }

            if (indexes.Length > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить записи с индексами " + indexes + "? Отменить это действие будет нельзя", "Подтверждение действия", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    for (int i = 0; i < ids.Count; i++)
                    {
                        Book.DeleteUserById(ids[i] - 1);

                        for (int j = 0; j < ids.Count; j++)
                        {
                            ids[j]--;
                        }
                    }

                    PhoneBookList.SelectedItems.Clear();
                    PhoneBookList.Items.Clear();

                    LoadUsers();
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите загрузить данные из файла? Вся информация телефонной книги будет перезаписана", "Подтверждение действия", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                PhoneBookList.Items.Clear();
                Book.ClearBook();
                Book.Load();
                LoadUsers();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }

        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            if (FIOTextBox.Text.Length == 0 && PhonenumberTextBox.Text.Length == 0)
            {
                MessageBox.Show("Заполните поля ФМО и Номер для внесения изменений");
                return;
            }

            if (PhoneBookList.SelectedItems.Count != 1)
            {
                MessageBox.Show("Выберите одну запись для редактирования");
                return;
            }

            if (FIOTextBox.Text.Length != 0 && PhonenumberTextBox.Text.Length == 0)
            {
                user rec = Book.getUserByid((Convert.ToInt32(PhoneBookList.SelectedItems[0].Text) - 1).ToString());
                user Rec = new user(PhoneBookList.SelectedItems[0].Text, FIOTextBox.Text, rec.getPhonenumber());
                Book.setUser(Rec);

                PhoneBookList.Items.Clear();
                LoadUsers();
                return;
            }

            if (FIOTextBox.Text.Length == 0 && PhonenumberTextBox.Text.Length != 0)
            {
                user rec = Book.getUserByid((Convert.ToInt32(PhoneBookList.SelectedItems[0].Text) - 1).ToString());
                user Rec = new user(PhoneBookList.SelectedItems[0].Text, rec.getName(), PhonenumberTextBox.Text);
                Book.setUser(Rec);

                PhoneBookList.Items.Clear();
                LoadUsers();
                return;
            }

            if (FIOTextBox.Text.Length != 0 && PhonenumberTextBox.Text.Length != 0)
            {
                user rec = Book.getUserByid((Convert.ToInt32(PhoneBookList.SelectedItems[0].Text) - 1).ToString());
                user Rec = new user(PhoneBookList.SelectedItems[0].Text, FIOTextBox.Text, PhonenumberTextBox.Text);
                Book.setUser(Rec);

                PhoneBookList.Items.Clear();
                LoadUsers();
                return;
            }

        }

        private void ClearTextBoxesButton_Click(object sender, EventArgs e)
        {
            FIOTextBox.Clear();
            PhonenumberTextBox.Clear();
        }
    }
}
