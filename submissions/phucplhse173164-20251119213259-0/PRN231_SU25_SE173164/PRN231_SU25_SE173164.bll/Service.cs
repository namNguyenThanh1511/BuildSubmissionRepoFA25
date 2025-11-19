using PRN231_SU25_SE173164.bll.Core;
using PRN231_SU25_SE173164.bll.DTOs;
using PRN231_SU25_SE173164.dal.Entities;
using PRN231_SU25_SE173164.dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRN231_SU25_SE173164.bll
{
    public class Service : IService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtHelper _jwtHelper;
        public Service(IUnitOfWork unitOfWork, JwtHelper jwtHelper)
        {
            _unitOfWork = unitOfWork;
            _jwtHelper = jwtHelper;
        }

        public async Task<List<LeopardProfile>> GetAllLeopardProfile()
        {
            return await _unitOfWork.GenericRepository<LeopardProfile>()
                                    .GetAll()
                                    .Include(_ => _.LeopardType)
                                    .ToListAsync();
        }

        public async Task<AuthenRespDTO> LoginAsync(AuthenDTO authenDTO)
        {
            try
            {
                var user = await _unitOfWork.GenericRepository<LeopardAccount>()
                                      .GetFirstOrDefaultAsync(_ => _.Email == authenDTO.Email
                                                                && _.Password == authenDTO.Password,
                                                                null);
                if (user == null)
                {
                    throw new BaseException(400, ErrorHelper.GetErrorMessage(400));
                }
                return new AuthenRespDTO
                {
                    Token = _jwtHelper.GenerateJwtToken(user),
                    Role = user.RoleId,
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<LeopardProfile> GetLeopardProfileByIdAsync(int id)
        {
            var handbag = await _unitOfWork.GenericRepository<LeopardProfile>()
                                           .GetFirstOrDefaultAsync(_ => _.LeopardProfileId == id, "LeopardType");
            if (handbag == null)
            {
                throw new BaseException(404, ErrorHelper.GetErrorMessage(404));
            }
            return handbag;
        }

        public async Task CreateLeopardProfileAsync(LeopardProfileDTO leopard)
        {
            try
            {

                var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
                if (!regex.IsMatch(leopard.LeopardName))
                {
                    throw new BaseException(400, "LeopardName is invalid format");
                }

                if (leopard.Weight <= 15)
                {
                    throw new BaseException(400, "Weight must be greater than 15");
                }

                var listTpyeId = await _unitOfWork.GenericRepository<LeopardType>()
                                                .GetByIdAsync(leopard.LeopardTypeId);
                if (listTpyeId == null)
                {
                    throw new BaseException(400, "LeopardTypeId invalid");
                }

                var leo = new LeopardProfile
                {
                    LeopardTypeId = leopard.LeopardTypeId,
                    LeopardName = leopard.LeopardName,
                    Weight = leopard.Weight,
                    Characteristics = leopard.Characteristics,
                    CareNeeds = leopard.CareNeeds,
                    ModifiedDate = leopard.ModifiedDate,
                };

                await _unitOfWork.GenericRepository<LeopardProfile>().InsertAsync(leo);
                await _unitOfWork.SaveChangeAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateLeopardProfileAsync(int id, LeopardProfileUpdateDTO leopard)
        {
            try
            {
                var existingLeopard = await _unitOfWork.GenericRepository<LeopardProfile>()
                                                .GetByIdAsync(id);
                if (existingLeopard == null) throw new BaseException(404, ErrorHelper.GetErrorMessage(404));

                var regex = new Regex(@"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$");
                if (!regex.IsMatch(leopard.LeopardName))
                {
                    throw new BaseException(400, "LeopardName is invalid format");
                }

                if (leopard.Weight <= 15)
                {
                    throw new BaseException(400, "Weight must be greater than 15");
                }

                var listTypeId = await _unitOfWork.GenericRepository<LeopardType>()
                                                .GetByIdAsync(leopard.LeopardTypeId);
                if (listTypeId == null)
                {
                    throw new BaseException(400, "LeopardTypeId invalid");
                }

                existingLeopard.LeopardName = leopard.LeopardName;
                existingLeopard.LeopardTypeId = leopard.LeopardTypeId;
                existingLeopard.Weight = leopard.Weight;
                existingLeopard.Characteristics = leopard.Characteristics;
                existingLeopard.CareNeeds = leopard.CareNeeds;
                existingLeopard.ModifiedDate = leopard.ModifiedDate;

                _unitOfWork.GenericRepository<LeopardProfile>().Update(existingLeopard);
                await _unitOfWork.SaveChangeAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteLeopardProfileAsync(int id)
        {
            var leopard = await _unitOfWork.GenericRepository<LeopardProfile>()
                                            .GetByIdAsync(id);
            if (leopard == null)
            {
                throw new BaseException(404, ErrorHelper.GetErrorMessage(404));
            }
            try
            {
                _unitOfWork.GenericRepository<LeopardProfile>().Delete(leopard);
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new BaseException(500, ex.Message);
            }
        }

        public IQueryable<LeopardProfile> GetSearch()
        {
            try
            {
                var handbags = _unitOfWork.GenericRepository<LeopardProfile>()
                                          .GetAll()
                                          .Include(_ => _.LeopardType);
                return handbags;
            }
            catch (Exception ex)
            {
                throw new BaseException(500, ex.Message);
            }
        }
    }
}
