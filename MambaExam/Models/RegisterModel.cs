
using System.ComponentModel.DataAnnotations;

namespace MambaExam.Models;

public class RegisterModel
{
    [Required]
    public string? UserName { get; set; }
    [Required,DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required, DataType(DataType.Password),Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }

}
