using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.DTO;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthService
    {
        private readonly AuthRepository _repo;
        private readonly JWTTokenProvider _jwt;

        public AuthService(AuthRepository repo, JWTTokenProvider jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        public async Task<(int code, LeopardAccountView item)> Login(string email, string password)
        {
            var existing = await _repo.Login(email, password);

            if (existing == null)
                return (404, null);

            LeopardAccountView view = new LeopardAccountView();

            if(existing.RoleId < 4 || existing.RoleId > 7)
            {
                view = new LeopardAccountView
                {
                    token = null,
                    role = existing.RoleId
                };
            }
            else
            {
                view = new LeopardAccountView
                {
                    token = _jwt.GenerateAccessToken(existing),
                    role = existing.RoleId
                };

                if (view.token.IsNullOrEmpty())
                    return (500, view);
            }
            return (200, view);

        }
    }
}
