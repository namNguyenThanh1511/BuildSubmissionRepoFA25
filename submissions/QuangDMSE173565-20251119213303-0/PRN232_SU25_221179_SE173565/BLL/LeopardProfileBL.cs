using DAL;
using DAL.Models;

namespace BLL
{
    public class LeopardProfileBL : ILeopardProfileBL
    {
        private readonly LeopardProfileDAO _dao;
        public LeopardProfileBL(LeopardProfileDAO dao)
        {
            _dao = dao;
        }
        public async Task<LeopardProfile> Create(LeopardProfile obj)
        {
            return await _dao.AddAsync(obj);
        }

        public async Task<List<LeopardProfile>> GetAll()
        {
            return await _dao.GetAllAsync();
        }

        public async Task<LeopardProfile> GetById(int id)
        {
            return await _dao.GetByIdAsync(id);
        }


        public async Task<List<LeopardProfile>> SearchAsync(string? field1, string? field2)
        {
            return await _dao.SearchAsync(field1, field2);
        }

        public async Task<LeopardProfile> Update(LeopardProfile obj)
        {
            return await _dao.UpdateAsync(obj);
        }

        public async Task<LeopardProfile> Delete(int id)
        {
            return await _dao.DeleteAsync(id);
        }
    }
}
