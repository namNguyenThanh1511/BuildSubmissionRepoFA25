using DAL;
using DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class LeopardService
    {
        private readonly IGenericRepo<LeopardProfile> _repo;
        public LeopardService(IGenericRepo<LeopardProfile> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<LeopardProfile>> GetAllLeopardAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<LeopardProfile> GetProfileByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> DeleteLeopardAsync(int id)
        {
            var leopard = await _repo.GetByIdAsync(id);
            if (leopard == null)
            {
                throw new Exception("with ID {id} not found.");
            }
            try
            {
                await _repo.DeleteAsync(leopard);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting leopard: {ex.Message}");
            }
        }

    }
}
