using PRN231_SU25_SE173171.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173171.BLL.Interfaces
{
    public interface ILeopardAccountService
    {
        Task<LoginResponse> Login(LoginRequest request);
    }
}
