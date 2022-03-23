using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPROJECT.Models
{
    public class VerifyAccount
    {   
        public int Id { get; set; }
        public string Otp { get; set; }
        public string Userid { get; set; }
		public DateTime? send_Time { get; set; }
    }
}
