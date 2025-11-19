using DAL.Models;

namespace BLL
{
    public interface ILeopardProfileBL
    {
        Task<List<LeopardProfile>> GetAll();
        Task<LeopardProfile> GetById(int id);
        Task<LeopardProfile> Create(LeopardProfile obj);

        Task<LeopardProfile> Update(LeopardProfile obj);

        Task<LeopardProfile> Delete(int id);

        Task<List<LeopardProfile>> SearchAsync(string? field1, string? field2);
    }
}
