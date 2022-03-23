using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSPROJECT.ViewModel
{
    public class SignUpModel
    {	 [Required]
		public string Fname { get; set; }
		[Required]
		public string Lname { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		[Compare("Password",ErrorMessage ="Password mismatch!!")]
		public string CoinfirmPassword { get; set; }
		public string Gender { get; set; }
	}
}
