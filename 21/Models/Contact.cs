using System.ComponentModel.DataAnnotations;

namespace PhoneBookEF.Models;

public class Contact
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Имя обязательно")]
    [StringLength(100)]
    [Display(Name = "Имя")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Телефон обязателен")]
    [StringLength(20)]
    [Display(Name = "Телефон")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string? Email { get; set; }

    [StringLength(200)]
    [Display(Name = "Заметка")]
    public string? Note { get; set; }
}
