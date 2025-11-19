using BusinessObjects;
using DataTransferObjects;
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

        public List<LeopardProfile> GetLeopardProfile()
        {
            return context.LeopardProfiles.Include(c => c.LeopardType).ToList();
        }

        public LeopardProfile GetLeopardProfileById(int id)
        {
            var item = context.LeopardProfiles.Include(c => c.LeopardType).FirstOrDefault(na => na.LeopardProfileId == id);
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
                    LeopardProfileId = leopardProfileDTO.LeopardProfileId,
                    LeopardName = leopardProfileDTO.LeopardName,
                    Weight = leopardProfileDTO.Weight,
                    Characteristics = leopardProfileDTO.Characteristics,
                    CareNeeds = leopardProfileDTO.CareNeeds,
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

        public void UpdateLeopardProfile(UpdateLeopardProfileDTO leopardProfileDTO)
        {
            var existing = context.LeopardProfiles.FirstOrDefault(na => na.LeopardProfileId == leopardProfileDTO.LeopardProfileId);

            if (existing != null)
            {
                existing.LeopardName = leopardProfileDTO.LeopardName;
                existing.Weight = leopardProfileDTO.Weight;
                existing.Characteristics = leopardProfileDTO.Characteristics;
                existing.CareNeeds = leopardProfileDTO.CareNeeds;
                existing.LeopardTypeId = leopardProfileDTO.LeopardTypeId;


                context.SaveChanges();
            }
        }

        public void DeleteLeopardProfile(int id)
        {
            var item = context.LeopardProfiles.FirstOrDefault(na => na.LeopardProfileId == id);
            if (item != null)
            {
                context.LeopardProfiles.Remove(item);
                context.SaveChanges();
            }
        }
    }
}
