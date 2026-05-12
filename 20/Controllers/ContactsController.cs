using Microsoft.AspNetCore.Mvc;
using PhoneBook.Models;
using PhoneBook.Services;
using PhoneBook.ViewModels;

namespace PhoneBook.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IActionResult Index()
        {
            var contacts = _contactService.GetAll();
            return View(contacts);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public IActionResult Add(ContactViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var contact = new Contact
            {
                Name = viewModel.Name,
                PhoneNumber = viewModel.PhoneNumber,
                Email = viewModel.Email
            };

            _contactService.Add(contact);

            TempData["SuccessMessage"] = $"✅ Контакт «{contact.Name}» успешно добавлен!";

            return RedirectToAction("Index");
        }

        public IActionResult Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Message = "Введите имя для поиска.";
                ViewBag.SearchName = "";
                return View(new List<Contact>());
            }

            var results = _contactService.Search(name);

            ViewBag.SearchName = name;
            ViewBag.Message = results.Any()
                ? $"Найдено контактов: {results.Count}"
                : $"Контакты с именем «{name}» не найдены.";

            return View(results);
        }
    }
}