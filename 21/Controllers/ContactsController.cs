using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBookEF.Data;
using PhoneBookEF.Models;

namespace PhoneBookEF.Controllers;

public class ContactsController : Controller
{
    private readonly AppDbContext _db;

    public ContactsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: /Contacts
    public async Task<IActionResult> Index(string? search)
    {
        var query = _db.Contacts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(c =>
                c.Name.Contains(search) ||
                c.Phone.Contains(search) ||
                (c.Email != null && c.Email.Contains(search)));

        ViewBag.Search = search;
        var contacts = await query.OrderBy(c => c.Name).ToListAsync();
        return View(contacts);
    }

    // GET: /Contacts/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Contacts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Contact contact)
    {
        if (ModelState.IsValid)
        {
            _db.Contacts.Add(contact);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Контакт «{contact.Name}» добавлен.";
            return RedirectToAction(nameof(Index));
        }
        return View(contact);
    }

    // GET: /Contacts/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact == null) return NotFound();
        return View(contact);
    }

    // POST: /Contacts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Contact contact)
    {
        if (id != contact.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            _db.Contacts.Update(contact);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Контакт «{contact.Name}» обновлён.";
            return RedirectToAction(nameof(Index));
        }
        return View(contact);
    }

    // POST: /Contacts/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact != null)
        {
            _db.Contacts.Remove(contact);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Контакт «{contact.Name}» удалён.";
        }
        return RedirectToAction(nameof(Index));
    }
}
