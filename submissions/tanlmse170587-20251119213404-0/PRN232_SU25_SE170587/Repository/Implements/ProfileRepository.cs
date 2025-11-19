using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Requests;
using Repository.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly Su25leopardDbContext _context;

        public ProfileRepository(Su25leopardDbContext context)
        {
            _context = context;
        }

        public async Task<MethodResult<IQueryable<ProfileResponse>>> GetAllAsync()
        {
            try
            {
                var result = _context.LeopardProfiles.Select(x => new ProfileResponse
                {
                    LeopardProfileId = x.LeopardProfileId,
                    LeopardName = x.LeopardName,
                    LeopardTypeId = x.LeopardTypeId,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate
                });

                return new MethodResult<IQueryable<ProfileResponse>>.Success(result, 200);
            }
            catch (Exception)
            {
                return new MethodResult<IQueryable<ProfileResponse>>.Failure(new ErrorResponse("HB50001", "Internal server error"), 500);
            }
        }

        public async Task<MethodResult<ProfileResponse>> GetOneAsync(int id)
        {
            try
            {
                var result = _context.LeopardProfiles.Select(x => new ProfileResponse
                {
                    LeopardProfileId = x.LeopardProfileId,
                    LeopardName = x.LeopardName,
                    LeopardTypeId = x.LeopardTypeId,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate
                }).FirstOrDefault(x => x.LeopardProfileId == id);

                return new MethodResult<ProfileResponse>.Success(result, 200);
            }
            catch (Exception)
            {
                return new MethodResult<ProfileResponse>.Failure(new ErrorResponse("HB50001", "Internal server error"), 500);
            }
        }

        public async Task<MethodResult<IQueryable<ProfileResponse>>> SearchAsync(string? leopardName, double? weight)
        {
            try
            {
                var query = _context.LeopardProfiles
                    .AsQueryable();

                if (!string.IsNullOrEmpty(leopardName))
                    query = query.Where(x => x.LeopardName.Contains(leopardName));

                if (weight != null)
                    query = query.Where(x => x.Weight == weight);

                var result = query.Select(x => new ProfileResponse
                {
                    LeopardProfileId = x.LeopardProfileId,
                    LeopardName = x.LeopardName,
                    LeopardTypeId = x.LeopardTypeId,
                    Weight = x.Weight,
                    Characteristics = x.Characteristics,
                    CareNeeds = x.CareNeeds,
                    ModifiedDate = x.ModifiedDate
                });                    

                return new MethodResult<IQueryable<ProfileResponse>>.Success(result, 200);
            }
            catch (Exception)
            {
                return new MethodResult<IQueryable<ProfileResponse>>.Failure(new ErrorResponse("HB50001", "Internal server error"), 500);
            }
        }

        public async Task<MethodResult<string>> CreateAsync(ProfileRequest request)
        {
            try
            {
                var type = await _context.LeopardTypes.FindAsync(request.LeopardTypeId);
                if (type == null)
                {
                    return new MethodResult<string>.Failure(new ErrorResponse("HB40001", "Invalid Leopard Type"), 400);
                }

                var profile = new LeopardProfile
                {
                    LeopardName = request.LeopardName,
                    LeopardTypeId = request.LeopardTypeId,
                    Weight = request.Weight,
                    Characteristics = request.Characteristics,
                    CareNeeds = request.CareNeeds,
                    ModifiedDate = request.ModifiedDate
                };

                await _context.LeopardProfiles.AddAsync(profile);
                await _context.SaveChangesAsync();

                return new MethodResult<string>.Success("Created successfully", 201);
            }
            catch (Exception)
            {
                return new MethodResult<string>.Failure(new ErrorResponse("HB50001", "Internal server error"), 500);
            }
        }

        public async Task<MethodResult<string>> UpdateAsync(int id, ProfileRequest request)
        {
            try
            {
                var type = await _context.LeopardTypes.FindAsync(request.LeopardTypeId);
                if (type == null)
                {
                    return new MethodResult<string>.Failure(new ErrorResponse("HB40001", "Invalid Leopard Type"), 400);
                }

                var profile = await _context.LeopardProfiles.FindAsync(id);
                if (profile == null)
                {
                    return new MethodResult<string>.Failure(new ErrorResponse("HB40401", "Leopard Profile not found"), 404);
                }

                profile.LeopardName = request.LeopardName;
                profile.LeopardTypeId = request.LeopardTypeId;
                profile.Weight = request.Weight;
                profile.Characteristics = request.Characteristics;
                profile.CareNeeds = request.CareNeeds;
                profile.ModifiedDate = request.ModifiedDate;

                _context.LeopardProfiles.Update(profile);
                await _context.SaveChangesAsync();

                return new MethodResult<string>.Success("Updated successfully", 200);

            }
            catch (Exception)
            {
                return new MethodResult<string>.Failure(new ErrorResponse("HB50001", "Internal server error"), 500);
            }
        }

        public async Task<MethodResult<string>> DeleteAsync(int id)
        {
            try
            {
                var handBag = await _context.LeopardProfiles.FindAsync(id);
                if (handBag == null)
                {
                    return new MethodResult<string>.Failure(new ErrorResponse("HB40401", "Leopard Profile not found"), 404);
                }

                _context.LeopardProfiles.Remove(handBag);
                await _context.SaveChangesAsync();

                return new MethodResult<string>.Success("Deleted successfully", 200);
            }
            catch (Exception)
            {
                return new MethodResult<string>.Failure(new ErrorResponse("HB50001", "Internal server error"), 500);
            }
        }
    }
}
