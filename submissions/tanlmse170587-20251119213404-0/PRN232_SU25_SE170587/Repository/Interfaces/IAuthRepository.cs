using Repository.Requests;
using Repository.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<MethodResult<LoginResponse>> LoginAsync(LoginRequest request);
    }
}
