using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPROJECT.Models
{
    public class Tbl_Users
    {
		public int Id { get; set; }
		public string Fname { get; set; }
		public string Lname { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }	
		public string Gender { get; set; }
		public string Profile_Image { get; set; }
		public bool Isactive { get; set; }
		public DateTime? Created_On { get; set; }
		public DateTime? Updated_On { get; set; }
		public bool IsVerified { get; set; }
	}
}
