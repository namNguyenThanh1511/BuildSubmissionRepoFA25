using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using Models.Models;
using Polly;
using Repositories.Interfaces;

namespace Repositories.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly LeopardAccountDAO _dao;

        public AuthRepository()
        {
            Su25leopardDbContext context = new Su25leopardDbContext();
            _dao = new LeopardAccountDAO(context);
        }

        public async Task<LeopardAccount> AuthenticateAsync(string email, string password)
        {
            return await _dao.AuthenticateAsync(email, password);
        }
    }
}
