using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE173175.Repository.Base;
using PRN231_SU25_SE173175.Repository.Basic;
using PRN231_SU25_SE173175.Repository.Entities;
using PRN231_SU25_SE173175.Service.DTOs;
using PRN231_SU25_SE173175.Service.Interfaces;

namespace PRN231_SU25_SE173175.Service.Services
{
	public class LeopardProfileService : ILeopardProfileService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public LeopardProfileService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<LeopardProfileResponse> CreateLeopardAsync(LeopardProfileRequest Leopard)
		{
			if (!await _unitOfWork.GetRepository<LeopardType>().Entities.AnyAsync(b => b.LeopardTypeId == Leopard.LeopardTypeId))
			{
				throw new BaseException(404, $"LeopardType with ID {Leopard.LeopardTypeId} does not exist.");
			}

			if (await _unitOfWork.GetRepository<LeopardProfile>().Entities.AnyAsync(b => b.LeopardProfileId == Leopard.LeopardProfileId))
			{
				throw new BaseException(404, $"ID {Leopard.LeopardProfileId} is existed.");
			}

			var leopard = _mapper.Map<LeopardProfile>(Leopard);

			var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
			var vnTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
			leopard.LeopardProfileId = Leopard.LeopardProfileId;
			leopard.ModifiedDate = DateTime.UtcNow;
			await _unitOfWork.GetRepository<LeopardProfile>().InsertAsync(leopard);
			var result = await _unitOfWork.SaveChangesWithTransactionAsync();

			return _mapper.Map<LeopardProfileResponse>(Leopard); ;
		}

		public async Task DeleteLeopardAsync(int id)
		{
			var leopard = await _unitOfWork.GetRepository<LeopardProfile>().GetByIdAsync(id);
			if (leopard == null)
			{
				throw new BaseException(404, $"leopard with ID {id} does not exist.");
			}

			await _unitOfWork.GetRepository<LeopardProfile>().DeleteAsync(id);
			await _unitOfWork.SaveChangesWithTransactionAsync();
		}

		public async Task<IEnumerable<LeopardProfileResponse>> GetAllAsync()
		{
			var result = await _unitOfWork.GetRepository<LeopardProfile>().Entities.Include(h => h.LeopardType).ToListAsync();
			return _mapper.Map<IEnumerable<LeopardProfileResponse>>(result);
		}

		public async Task<LeopardProfileResponse?> GetLeopardByIdAsync(int LeopardId)
		{
			var result = await _unitOfWork.GetRepository<LeopardProfile>().Entities
				.Include(h => h.LeopardType)
				.FirstOrDefaultAsync(h => h.LeopardProfileId == LeopardId);

			return _mapper.Map<LeopardProfileResponse>(result); ;
		}

		public async Task<IQueryable<LeopardProfileResponse>> SearchLeopardsQueryableAsync(string? LeopardName, double? Weight)
		{
			var query = _unitOfWork.GetRepository<LeopardProfile>().Entities
			   .Include(h => h.LeopardType)
			   .AsQueryable();

			if (!string.IsNullOrWhiteSpace(LeopardName))
			{
				query = query.Where(h => h.LeopardName.Contains(LeopardName));
			}

			if (Weight != null)
			{
				query = query.Where(h => h.Weight == Weight);
			}

			return _mapper.ProjectTo<LeopardProfileResponse>(query);
		}

		public async Task UpdateLeopardAsync(int id, LeopardProfileUpdateRequest Leopard)
		{

			var leopard = await _unitOfWork.GetRepository<LeopardProfile>().GetByIdAsync(id);
			var brand = await _unitOfWork.GetRepository<LeopardProfile>().GetByIdAsync(Leopard.LeopardTypeId);

			if (leopard == null)
			{
				throw new BaseException(404, $"LeopardProfile with ID {id} does not exist.");
			}

			if (brand == null)
			{
				throw new BaseException(404, $"LeopardType with ID {Leopard.LeopardTypeId} does not exist.");
			}

			leopard.LeopardName = Leopard.LeopardName;
			leopard.Weight = Leopard.Weight;
			leopard.Characteristics = Leopard.Characteristics;
			leopard.CareNeeds = Leopard.CareNeeds;
			leopard.LeopardTypeId = Leopard.LeopardTypeId;

			await _unitOfWork.GetRepository<LeopardProfile>().UpdateAsync(leopard);
			await _unitOfWork.SaveChangesWithTransactionAsync();
		}
	}
}
