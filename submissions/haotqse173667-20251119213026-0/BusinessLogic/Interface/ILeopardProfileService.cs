using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.ModalViews;

namespace BusinessLogic.Interface
{
    public interface ILeopardProfileService
    {
        Task<List<LeopardProfileReponse>> GetAll();
        Task<LeopardProfileReponse> GetById(int id);
        Task<bool> Create(LeopardProfileRequest request);
        Task<bool> Update(int id, LeopardProfileRequest request);
        Task<bool> Delete(int id);
        Task<List<LeopardProfileReponse>> Search(string modelName);
    }
}
