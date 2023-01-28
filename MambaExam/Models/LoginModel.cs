
using System.ComponentModel.DataAnnotations;

namespace MambaExam.Models;

public class LoginModel
{
    [Required]
    public string? UserOrEmail { get; set; }
    [Required,DataType(DataType.Password)]
    public string? Password { get; set; }
}
