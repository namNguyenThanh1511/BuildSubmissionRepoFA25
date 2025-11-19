using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTOs
{
	public class LoginResponseDTO
	{
		public string token { get; set; }

		public int role { get; set; }
	}
}
