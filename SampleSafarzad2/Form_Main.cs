using Class_Models;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleSafarzad2
{
    public partial class Form_Main : Form
    {
        string filePath = @"D:\dataSample2.json";
        int _indexSelectd=-1;
        Services _service = new Services();


        public Form_Main()
        {
            InitializeComponent();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
           
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath);
            }
            var contacts = _service.GetContacts();
           /* var contacts =  GetContacts();*/
            FillGridView(contacts);
        }

        private void FillGridView(List<Contact> contacts)
        {
            grd_contacts.Rows.Clear();
            foreach (Contact contact in contacts)
            {
                grd_contacts.Rows.Add(contact.Id, contact.Firstname, contact.Lastname, contact.PhoneNumber);
            }
        }

      

      
    
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (
                 txt_FirstName.Text.Trim() != ""
                 && txt_LastName.Text.Trim() != ""
                 && txt_PhoneNumber.Text.Trim() != ""

                 )
            {
                var newContact = new Contact();
                newContact.Firstname = txt_FirstName.Text;
                newContact.Lastname = txt_LastName.Text;
                newContact.PhoneNumber = txt_PhoneNumber.Text;
                if (_indexSelectd == -1 )
                {
                    newContact.Id = Guid.NewGuid();

                    var contacts = _service.GetContacts();

                    int _Count = contacts.Where(x => x.PhoneNumber == newContact.PhoneNumber).Count();
                    if (_Count == 0)
                    {
                        contacts.Add(newContact);

                        if (_service.SaveContact(contacts))
                            MessageBox.Show("اطلاعات با موفقیت ذخیره شد", "کاربر گرامی", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        FillGridView(contacts);

                        txt_FirstName.Text = "";
                        txt_LastName.Text = "";
                        txt_PhoneNumber.Text = "";
                      
                    }
                    else
                    {
                        MessageBox.Show("تلفن همراه در جدول وجود دارد", "کاربر گرامی", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                else{

                    var contacts = _service.GetContacts();
                    var id = grd_contacts.Rows[_indexSelectd].Cells[0].Value.ToString();
                    contacts.FirstOrDefault(x=>x.Id.ToString() == id.ToString()).Firstname = txt_FirstName.Text;
                    contacts.FirstOrDefault(x=>x.Id.ToString() == id.ToString()).Lastname = txt_LastName.Text;
                    contacts.FirstOrDefault(x=>x.Id.ToString() == id.ToString()).PhoneNumber = txt_PhoneNumber.Text;

                    if (_service.SaveContact(contacts))
                        MessageBox.Show("اطلاعات با موفقیت ذخیره شد", "کاربر گرامی", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FillGridView(contacts);
                    _indexSelectd = -1;
                    txt_FirstName.Text = "";
                    txt_LastName.Text = "";
                    txt_PhoneNumber.Text = "";
                }


            }
            else
            {
                MessageBox.Show("لطفا اطلاعات را تکمیل نمایید", "کاربر گرامی", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (_indexSelectd >= 0)
            {
                if (MessageBox.Show("آیا می خواهید اطلاعات حذف شود","کاربر گرامی",
                MessageBoxButtons.YesNo,MessageBoxIcon.Question)
                ==DialogResult.Yes)
                 {
                
                    var id = grd_contacts.Rows[_indexSelectd].Cells[0].Value.ToString();
                    var contacts = _service.GetContacts();

                    Contact removecontact=new Contact();

                    foreach (var item in contacts)
                    {
                        if(item.Id.ToString()==id)
                        {
                            removecontact=item;
                        }
                    }
                    contacts.Remove(removecontact);
                    if (_service.SaveContact(contacts))
                        MessageBox.Show("اطلاعات با موفقیت حذف شد", "کاربر گرامی", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FillGridView(contacts);
                    _indexSelectd = -1;
                    txt_FirstName.Text = "";
                    txt_LastName.Text = "";
                    txt_PhoneNumber.Text = "";
                }
               
            }
            else
            {
                MessageBox.Show("رکوردی برای حذف انتخاب نشده است", "کاربر گرامی", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void grd_contacts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _indexSelectd = e.RowIndex;
            var id = grd_contacts.Rows[_indexSelectd].Cells[0].Value.ToString();
            var contacts = _service.GetContacts();
            txt_FirstName.Text = contacts.FirstOrDefault(x => id.ToString() == id.ToString()).Firstname;
            txt_LastName.Text = contacts.FirstOrDefault(x => id.ToString() == id.ToString()).Lastname;
            txt_PhoneNumber.Text = contacts.FirstOrDefault(x => id.ToString() == id.ToString()).PhoneNumber;

        }
    }
}
