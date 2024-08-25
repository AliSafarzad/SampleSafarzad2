using Class_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository
    {
        List<Contact> GetContacts();
        bool SaveContact(List<Contact> model);
        bool DeleteContact(Contact model);
    }
}
