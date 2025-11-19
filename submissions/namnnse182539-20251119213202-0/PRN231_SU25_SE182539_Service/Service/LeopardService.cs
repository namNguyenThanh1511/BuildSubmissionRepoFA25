using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE182539_Repository.Models;
using Repository.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Service
{
    public class LeopardService : ILeopardService
    {
        private readonly SU25LeopardDBContext _context;

        public LeopardService(SU25LeopardDBContext context)
        {
            _context = context;
        }

        public IEnumerable<LeopardResponse> GetAll()
        {
            return _context.LeopardProfiles.Include(h => h.LeopardType)
                .Select(h => new LeopardResponse
                {
                    LeopardProfileId = h.LeopardProfileId,
                    LeopardTypeId = h.LeopardTypeId,
                    LeopardName = h.LeopardName,
                    Weight = h.Weight,
                    Characteristics = h.Characteristics,
                    CareNeeds = h.CareNeeds
                }).ToList();
        }

        public LeopardResponse? GetById(int id)
        {
            var h = _context.LeopardProfiles.Include(h => h.LeopardType).FirstOrDefault(h => h.LeopardProfileId == id);
            if (h == null) return null;

            return new LeopardResponse
            {
                LeopardProfileId = h.LeopardProfileId,
                LeopardTypeId = h.LeopardTypeId,
                LeopardName = h.LeopardName,
                Weight = h.Weight,
                Characteristics = h.Characteristics,
                CareNeeds = h.CareNeeds
            };
        }

        public string? Create(LeopardDto dto)
        {
            if (!Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return "modelName format is invalid";

            if (dto.Weight < 15)
                return "Weight must be < 15";

            var leopard = new LeopardProfile
            {
                LeopardTypeId = dto.LeopardTypeId,
                LeopardName = dto.LeopardName,
                Weight = dto.Weight,
                Characteristics = dto.Characteristics,
                CareNeeds = dto.CareNeeds,
                ModifiedDate = dto.ModifiedDate,    
            };

            _context.LeopardProfiles.Add(leopard);
            _context.SaveChanges();

            return null; 
        }

        public string? Update(int id, LeopardDto dto)
        {
            var h = _context.LeopardProfiles.FirstOrDefault(x => x.LeopardProfileId == id);
            if (h == null) return "not_found";

            if (!Regex.IsMatch(dto.LeopardName, @"^([A-Z0-9][a-zA-Z0-9#]*\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return "modelName format is invalid";

            if (dto.Weight < 15)
                return "Weight must be < 15";

            h.LeopardTypeId = dto.LeopardTypeId;
            h.LeopardName = dto.LeopardName;
            h.Weight = dto.Weight;
            h.Characteristics = dto.Characteristics;
            h.CareNeeds = dto.CareNeeds;
            h.ModifiedDate = dto.ModifiedDate;

            _context.SaveChanges();
            return null;
        }

        public bool Delete(int id)
        {
            var h = _context.LeopardProfiles.Find(id);
            if (h == null) return false;

            _context.LeopardProfiles.Remove(h);
            _context.SaveChanges();
            return true;
        }

      
    }
}
