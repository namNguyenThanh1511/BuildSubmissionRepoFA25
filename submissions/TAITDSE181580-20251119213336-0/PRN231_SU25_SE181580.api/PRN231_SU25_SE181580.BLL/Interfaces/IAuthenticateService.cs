using PRN231_SU25_SE181580.BLL.DTOs;
using PRN231_SU25_SE181580.DAL.Entities;

namespace PRN231_SU25_SE181580.BLL.Interfaces;

public interface IAuthenticateService {
    Task<LoginResponseDTO> Login(string email, string password);
}
