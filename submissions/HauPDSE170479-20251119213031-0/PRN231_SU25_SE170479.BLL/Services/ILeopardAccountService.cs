using PRN231_SU25_SE170479.DAL.ModelExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_SU25_SE170479.BLL.Services
{
    public interface ILeopardAccountService
    {
        Task<Result<TokenResponse>> LoginAsync(string email, string password);
    }
}
