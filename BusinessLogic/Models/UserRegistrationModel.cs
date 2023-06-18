using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models;

public class UserRegistrationModel
{
    [Required]
    public string DisplayName { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}   