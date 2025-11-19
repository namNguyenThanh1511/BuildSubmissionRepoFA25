using Repository.Requests;
using Repository.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IProfileRepository
    {
        Task<MethodResult<IQueryable<ProfileResponse>>> GetAllAsync();
        Task<MethodResult<ProfileResponse>> GetOneAsync(int id);
        Task<MethodResult<IQueryable<ProfileResponse>>> SearchAsync(string? leopardName, double? weight);
        Task<MethodResult<string>> CreateAsync(ProfileRequest request);
        Task<MethodResult<string>> UpdateAsync(int id, ProfileRequest request);
        Task<MethodResult<string>> DeleteAsync(int id);
    }
}
