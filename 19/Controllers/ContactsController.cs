using Microsoft.AspNetCore.Mvc;
using PhoneBook.Models;

namespace PhoneBook.Controllers
{
    public class ContactsController : Controller
    {
        private static List<Contact> _contacts = new List<Contact>
        {
            new Contact { Id=1, Name="Анна Иванова",   PhoneNumber="+7 900 111-22-33", Email="anna@mail.ru"   },
            new Contact { Id=2, Name="Борис Петров",   PhoneNumber="+7 900 444-55-66", Email="boris@mail.ru"  },
            new Contact { Id=3, Name="Виктор Сидоров", PhoneNumber="+7 900 777-88-99", Email="viktor@mail.ru" }
        };

        // GET: /Contacts
        public IActionResult Index()
        {
            return View(_contacts);
        }

        // GET: /Contacts/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View(new Contact());
        }

        // POST: /Contacts/Add
        [HttpPost]
        public IActionResult Add(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Id = _contacts.Count + 1;
                _contacts.Add(contact);
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: /Contacts/Search?name=...
        public IActionResult Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Message = "Введите имя для поиска.";
                ViewBag.SearchName = "";
                return View(new List<Contact>());
            }

            var results = _contacts
                .Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            ViewBag.SearchName = name;
            ViewBag.Message = results.Any()
                ? $"Найдено контактов: {results.Count}"
                : $"Контакты с именем «{name}» не найдены.";

            return View(results);
        }
    }
}