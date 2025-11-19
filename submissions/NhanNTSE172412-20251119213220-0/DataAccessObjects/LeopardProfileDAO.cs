using BusinessObjects;
using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class LeopardProfileDAO
    {
        private static LeopardProfileDAO instance = null;
        private readonly SU25LeopardDBContext context;

        private LeopardProfileDAO()
        {
            context = new SU25LeopardDBContext();
        }

        public static LeopardProfileDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LeopardProfileDAO();
                }
                return instance;
            }
        }

        public List<LeopardProfile> GetLeopardProfiles()
        {
            return context.LeopardProfiles.Include(c => c.LeopardType).ToList();
        }

        public LeopardProfile GetLeopardProfileById(int id)
        {
            var item = context.LeopardProfiles.Include(c => c.LeopardType).FirstOrDefault(i => i.LeopardProfileId == id);
            if (item != null)
            {
                return item;
            }
            return null;
        }

        public void AddLeopardProfile(LeopardProfileDTO leopardProfileDTO)
        {
            try
            {
                var item = new LeopardProfile
                {
                    LeopardName = leopardProfileDTO.LeopardName,
                    Characteristics = leopardProfileDTO.Characteristics,
                    CareNeeds = leopardProfileDTO.CareNeeds,
                    Weight = leopardProfileDTO.Weight,
                    LeopardTypeId = leopardProfileDTO.LeopardTypeId,
                };

                context.LeopardProfiles.Add(item);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to add item: {e.Message}", e);
            }
        }

        public void UpdateLeopardProfile(LeopardProfileDTO leopardProfileDTO)
        {
            var existing = context.LeopardProfiles.FirstOrDefault(i => i.LeopardProfileId == leopardProfileDTO.LeopardProfileId);

            if (existing != null)
            {
                existing.LeopardName = leopardProfileDTO.LeopardName;
                existing.Characteristics = leopardProfileDTO.Characteristics;
                existing.CareNeeds = leopardProfileDTO.CareNeeds;
                existing.Weight = leopardProfileDTO.Weight;
                existing.LeopardTypeId = leopardProfileDTO.LeopardTypeId;

                context.SaveChanges();
            }
        }

        public void DeleteLeopardProfile(int id)
        {
            var item = context.LeopardProfiles.FirstOrDefault(i => i.LeopardProfileId == id);
            if (item != null)
            {
                context.LeopardProfiles.Remove(item);
                context.SaveChanges();
            }
        }

        public List<object> SearchLeopardProfiles(string? LeopardName, double? Weight)
        {
            var query = context.LeopardProfiles.Include(i => i.LeopardType).AsQueryable();

            if (!string.IsNullOrWhiteSpace(LeopardName))
                query = query.Where(i => i.LeopardName.Contains(LeopardName));

            if (Weight.HasValue)
                query = query.Where(i => i.Weight == Weight.Value);

            var grouped = query
                .ToList()
                .GroupBy(i => i.LeopardType?.LeopardTypeName ?? "No LeopardType")
                .Select(g => new
                {
                    BrandName = g.Key,
                    LeopardProfiles = g.Select(i => new
                    {
                        i.LeopardProfileId,
                        i.LeopardName,
                        i.Weight,
                        i.Characteristics,
                        i.CareNeeds,
                        i.ModifiedDate
                    }).ToList()
                }).ToList<object>();

            return grouped;
        }
    }
}
