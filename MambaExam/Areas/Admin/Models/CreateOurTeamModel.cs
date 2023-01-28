using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;

namespace MambaExam.Areas.Admin.Models;

public class CreateOurTeamModel
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Role { get; set; }
    public IFormFile? img { get; set; }
}
