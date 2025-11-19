using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Repository;
using Repository.Entities;
using Service.Dto;
using System.Reflection.PortableExecutable;

namespace Service.Impl;

public class LeopardProfileService
{
    private readonly ILogger<LeopardProfileService> _logger;
    private readonly LeopardProfileRepository _leopardProfileRepository;

    public LeopardProfileService(ILogger<LeopardProfileService> logger, LeopardProfileRepository leopardProfileRepository)
    {
        _logger = logger;
		_leopardProfileRepository = leopardProfileRepository;
    }


    public async Task<IList<LeopardProfile>> GetAll()
    {
        try
        {
			var handbags = await _leopardProfileRepository.GetAllAsync(query => query.Include(h => h.LeopardType));
			if (handbags.IsNullOrEmpty()) throw new NullReferenceException();
            return handbags;
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get all bag cause by {}", e.Message);
            throw;
        }
    }

    public async Task<LeopardProfile> GetById(int id)
    {
        try
        {
            var handbag = await _leopardProfileRepository.FindAsync(h => h.LeopardTypeId == id, "Type");
            if (handbag == null) throw new NullReferenceException();
            return handbag;
        }
        catch (Exception e)
        {
            _logger.LogError("Error at get bag by id cause by {}", e.Message);
            throw;
        }
    }


    public async Task AddLeopard(CreateLeopardReq request)
    {
        try
        {
            _logger.LogInformation("Start create handbag with request {}", request);
            var type = await _leopardProfileRepository.FindAsync(h => h.LeopardTypeId == request.LeopardTypeId);
            if (type == null) throw new KeyNotFoundException("Resource not found");

            var leopard = new LeopardProfile
            {
				LeopardProfileId = request.LeopardProfileId,
				LeopardTypeId = request.LeopardTypeId,
				LeopardName = request.LeopardName,
				Weight = request.Weight,
				Characteristics = request.Characteristics,
				CareNeeds = request.CareNeeds,
				ModifiedDate = DateTime.Now
            };

            await _leopardProfileRepository.InsertAsync(leopard);
            await _leopardProfileRepository.SaveAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error at add leopard cause by {}", e.Message);
            throw;
        }
    }

    public async Task UpdateLeopard(int id, UpdateLeopardReq request)
    {
        try
        {
            var leopard = await _leopardProfileRepository.GetByIdAsync(id);
            if (leopard == null) throw new KeyNotFoundException("Resource not found");
            var type = await _leopardProfileRepository.FindAsync(h => h.LeopardTypeId == request.LeopardTypeId);
            if (type == null) throw new KeyNotFoundException("Resource not found");

            leopard.LeopardTypeId = request.LeopardTypeId;
            leopard.LeopardName = request.LeopardName;
            leopard.Weight = request.Weight;
            leopard.Characteristics = request.Characteristics;
            leopard.CareNeeds = request.CareNeeds;
            leopard.ModifiedDate = DateTime.Now;


			await _leopardProfileRepository.UpdateAsync(leopard);
            await _leopardProfileRepository.SaveAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Errror at update leopard cause by {}", e.Message);
            throw;
        }
    }

    public async Task DeleteLeopard(int id)
    {
        await _leopardProfileRepository.DeleteAsync(id);
        await _leopardProfileRepository.SaveAsync();
    }


}