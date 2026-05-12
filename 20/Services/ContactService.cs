using PhoneBook.Models;

namespace PhoneBook.Services
{
    public class ContactService : IContactService
    {
        private static List<Contact> _contacts = new List<Contact>
        {
            new Contact { Id=1, Name="Анна Иванова",   PhoneNumber="+7 900 111-22-33", Email="anna@mail.ru"   },
            new Contact { Id=2, Name="Борис Петров",   PhoneNumber="+7 900 444-55-66", Email="boris@mail.ru"  },
            new Contact { Id=3, Name="Виктор Сидоров", PhoneNumber="+7 900 777-88-99", Email="viktor@mail.ru" }
        };

        private static int _nextId = 4;

        public List<Contact> GetAll()
        {
            return _contacts.ToList();
        }

        public List<Contact> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Contact>();

            return _contacts
                .Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void Add(Contact contact)
        {
            contact.Id = _nextId++;
            _contacts.Add(contact);
        }
    }
}