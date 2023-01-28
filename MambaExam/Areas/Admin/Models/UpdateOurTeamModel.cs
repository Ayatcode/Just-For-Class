using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;

namespace MambaExam.Areas.Admin.Models
{
	public class UpdateOurTeamModel
	{
		public int Id { get; set; }
		[Required]
		public string? name { get; set; }
        [Required]
        public string? Role { get; set; }
		public int? MediaId { get; set; }
		public IFormFile? img { get; set; }
	}
}
