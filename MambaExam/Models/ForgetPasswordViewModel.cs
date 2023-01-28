
using System.ComponentModel.DataAnnotations;

namespace MambaExam.Models;

public class ForgetPasswordViewModel
{
    [Required,DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
