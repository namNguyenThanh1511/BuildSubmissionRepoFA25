using BOs;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Services.UnitOfWorks;

namespace Service
{
    public class LeopardAccountService : ILeopardAccountService
    {
        private readonly ILeopardAccountRepository _leopardAccountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeopardAccountService(ILeopardAccountRepository leopardAccountRepository, IUnitOfWork unitOfWork)
        {
            _leopardAccountRepository = leopardAccountRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<LeopardAccount> CreateAsync(LeopardAccount leopardAccount)
        {
            _leopardAccountRepository.Create(leopardAccount);
            await _unitOfWork.SaveChange();
            return leopardAccount;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _leopardAccountRepository.FindById(id);
            if (existing == null)
                return false;

            _leopardAccountRepository.Delete(existing);
            await _unitOfWork.SaveChange();
            return true;
        }

        public async Task<List<LeopardAccount>> GetAllAsync()
        {
            return _leopardAccountRepository.FindAll().ToList();
        }

        public async Task<LeopardAccount?> GetByIdAsync(int id)
        {
            return await _leopardAccountRepository.FindById(id);
        }


        public async Task<LeopardAccount?> Login(string email, string password)
        {
            return await _leopardAccountRepository.Login(email, password);
        }
    }
}
