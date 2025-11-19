using DataAccess.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bisiness.Iservice
{
    public interface ILeopardAccountService
    {
        Task<LoginRespont> login(LoginModelRequest loginModelRequest);
    }
}
