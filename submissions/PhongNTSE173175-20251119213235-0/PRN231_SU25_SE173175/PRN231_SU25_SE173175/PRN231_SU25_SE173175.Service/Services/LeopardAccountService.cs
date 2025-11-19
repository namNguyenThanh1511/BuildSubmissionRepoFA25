using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173175.Repository.Basic;
using PRN231_SU25_SE173175.Repository.Entities;
using PRN231_SU25_SE173175.Service.Interfaces;

namespace PRN231_SU25_SE173175.Service.Services
{
	public class LeopardAccountService : ILeopardAccountService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public LeopardAccountService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<LeopardAccount>> GetAllAccountsAsync()
		{
			return await _unitOfWork.GetRepository<LeopardAccount>().GetAllAsync();
		}

		public async Task<LeopardAccount?> GetAccountByIdAsync(int accountId)
		{
			return await _unitOfWork.GetRepository<LeopardAccount>().GetByIdAsync(accountId);
		}

		public async Task<LeopardAccount?> GetAccountByUsernameAsync(string username)
		{
			return await _unitOfWork.GetRepository<LeopardAccount>().FirstOrDefaultAsync(u => u.UserName.Equals(username));
		}

		public async Task<LeopardAccount?> GetAccountByEmailAsync(string email)
		{
			return await _unitOfWork.GetRepository<LeopardAccount>().FirstOrDefaultAsync(u => u.Email.Equals(email));
		}

		public async Task<IEnumerable<LeopardAccount>> GetAccountsByRoleAsync(int role)
		{
			return await _unitOfWork.GetRepository<LeopardAccount>().Entities.Where(u => u.RoleId == role).ToListAsync();
		}


		public async Task<LeopardAccount> CreateAccountAsync(LeopardAccount account)
		{
			if (!await IsUsernameUniqueAsync(account.UserName))
				throw new InvalidOperationException("Username already exists");

			if (!await IsEmailUniqueAsync(account.Email))
				throw new InvalidOperationException("Email already exists");

			await _unitOfWork.GetRepository<LeopardAccount>().InsertAsync(account);
			var result = await _unitOfWork.SaveChangesWithTransactionAsync();
			if (result <= 0)
			{
				throw new Exception("Failed to create account");
			}
			return account;
		}

		public async Task UpdateAccountAsync(LeopardAccount account)
		{
			var existingAccount = await _unitOfWork.GetRepository<LeopardAccount>().GetByIdAsync(account.AccountId);
			if (existingAccount == null)
				throw new InvalidOperationException("Account not found");

			if (existingAccount.UserName != account.UserName && !await IsUsernameUniqueAsync(account.UserName))
				throw new InvalidOperationException("Username already exists");

			if (existingAccount.Email != account.Email && !await IsEmailUniqueAsync(account.Email))
				throw new InvalidOperationException("Email already exists");

			_unitOfWork.GetRepository<LeopardAccount>().Update(account);
			var result = await _unitOfWork.SaveChangesWithTransactionAsync();
			if (result <= 0)
			{
				throw new Exception("Failed to update account");
			}
		}

		public async Task DeleteAccountAsync(int accountId)
		{
			var account = await _unitOfWork.GetRepository<LeopardAccount>().GetByIdAsync(accountId);
			if (account != null)
			{
				_unitOfWork.GetRepository<LeopardAccount>().Delete(account);
				var result = await _unitOfWork.SaveChangesWithTransactionAsync();
				if (result <= 0)
				{
					throw new Exception("Failed to delete account");
				}
			}
		}

		public async Task<bool> AccountExistsAsync(int accountId)
		{
			return await _unitOfWork.GetRepository<LeopardAccount>().Entities.AnyAsync(a => a.AccountId == accountId);
		}

		public async Task<bool> IsUsernameUniqueAsync(string username)
		{
			return await _unitOfWork.GetRepository<LeopardAccount>().Entities.AnyAsync(a => a.UserName == username);
		}

		public async Task<bool> IsEmailUniqueAsync(string email)
		{
			return await _unitOfWork.GetRepository<LeopardAccount>().Entities.AnyAsync(a => a.Email == email);
		}

		public async Task ToggleAccountStatusAsync(int accountId)
		{
			var account = await _unitOfWork.GetRepository<LeopardAccount>().GetByIdAsync(accountId);
			if (account != null)
			{
				_unitOfWork.GetRepository<LeopardAccount>().Update(account);
				var result = await _unitOfWork.SaveChangesWithTransactionAsync();
				if (result <= 0)
				{
					throw new Exception("Failed to toggle account status");
				}
			}
		}

		public async Task UpdateAccountRoleAsync(int accountId, int newRole)
		{
			var account = await _unitOfWork.GetRepository<LeopardAccount>().GetByIdAsync(accountId);
			if (account != null)
			{
				account.RoleId = newRole;
				_unitOfWork.GetRepository<LeopardAccount>().Update(account);
				var result = await _unitOfWork.SaveChangesWithTransactionAsync();
				if (result <= 0)
				{
					throw new Exception("Failed to update account role");
				}
			}
		}
	}
}
