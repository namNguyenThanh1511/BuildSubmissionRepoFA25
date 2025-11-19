using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public interface IAuthService
	{
		Task<LeopardAccount> Authenticate(string email, string password);
	}
	public class AuthService: IAuthService
	{
		private readonly LeopardAccountRepo _repository;

		public AuthService() => _repository = new LeopardAccountRepo();

		public async Task<LeopardAccount> Authenticate(string email, string password)
		{
			return await _repository.GetUserAccountAsync(email, password);
		}
	}
}
