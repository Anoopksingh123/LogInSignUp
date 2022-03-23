using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSPROJECT.ViewModel
{
    public class SignInModel
    {
		[Required]
		public string Email { get; set; }
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}
