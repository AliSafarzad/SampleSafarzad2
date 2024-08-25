using Class_Models;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Services : IRepository
    {
        string filePath = @"D:\dataSample2.json";
        public bool DeleteContact(Contact model)
        {
            var contacts = GetContacts();
            contacts.Remove(model);
            bool _result = SaveContact(contacts);
            return _result;
        }

        public List<Contact> GetContacts()
        {
            var result = new List<Contact>();

            try
            {
                var fileString = System.IO.File.ReadAllText(filePath);
                result = JsonConvert.DeserializeObject<List<Contact>>(fileString);
                if (result == null)
                {
                    result = new List<Contact>();
                }

            }
            catch (Exception)
            {

            }


            return result;
        }

        public bool SaveContact(List<Contact> model)
        {
            try
            {
                var stringModel = JsonConvert.SerializeObject(model);
                System.IO.File.WriteAllText(filePath, stringModel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
