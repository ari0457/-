using System.ComponentModel.DataAnnotations;

namespace PhoneBook.ViewModels
{
    public class ContactViewModel
    {
        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Телефон обязателен для заполнения")]
        [Phone(ErrorMessage = "Введите корректный номер телефона")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email обязателен для заполнения")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}