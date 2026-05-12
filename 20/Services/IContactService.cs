using PhoneBook.Models;

namespace PhoneBook.Services
{
    public interface IContactService
    {
        List<Contact> GetAll();

        List<Contact> Search(string name);

        void Add(Contact contact);
    }
}