using Microsoft.EntityFrameworkCore;
using PRN232_SU25_SE181544.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SU25_SE181544.DAL.Repositories
{
    public class LeopardProfileRepository
    {
        public async Task<List<LeopardType>> GetAllBrand()
        {
            using (Su25leopardDbContext context = new())
            {
                return await context.LeopardTypes.ToListAsync();
            }
        }

        public async Task<LeopardType> GetBrandById(int id)
        {
            using (Su25leopardDbContext context = new())
            {
                return await context.LeopardTypes.FindAsync(id);
            }
        }

        public async Task<List<LeopardProfile>> GetAll()
        {
            using (Su25leopardDbContext context = new())
            {
                return await context.LeopardProfiles.Include(x => x.LeopardType).ToListAsync();
            }
        }
        public async Task<LeopardProfile> GetById(int id)
        {
            using (Su25leopardDbContext context = new())
            {
                return await context.LeopardProfiles.Include(x => x.LeopardType).FirstOrDefaultAsync(x => x.LeopardProfileId == id);
            }
        }

        //public async Task<int> GenerateId()
        //{
        //    using (Su25leopardDbContext context = new())
        //    {
        //        var all = await context.LeopardProfiles.ToListAsync();
        //        return all.Count + 1;
        //    }
        //}

        public async Task<LeopardProfile> Add(LeopardProfile product)
        {
            using (Su25leopardDbContext context = new())
            {
                await context.LeopardProfiles.AddAsync(product);
                await context.SaveChangesAsync();

                var check = await context.LeopardProfiles.FindAsync(product.LeopardProfileId);
                return check;
            }
        }
        public async Task<LeopardProfile> Update(LeopardProfile product)
        {
            using (Su25leopardDbContext context = new())
            {
                context.LeopardProfiles.Update(product);
                await context.SaveChangesAsync();

                var check = await context.LeopardProfiles.FindAsync(product.LeopardProfileId);

                return product;
            }
        }
        public async Task<LeopardProfile> Delete(LeopardProfile product)
        {
            using (Su25leopardDbContext context = new())
            {
                context.LeopardProfiles.Remove(product);
                await context.SaveChangesAsync();
                var check = await context.LeopardProfiles.FindAsync(product.LeopardProfileId);
                //if (check == null)
                //{
                //    return null;
                //}
                return product;
            }
        }

        public async Task<List<LeopardProfile>> Search(string leopardName, double weight)
        {
            using (Su25leopardDbContext context = new())
            {
                var list = context.LeopardProfiles.Include(x => x.LeopardType).AsQueryable();
                if (!string.IsNullOrEmpty(leopardName))
                {
                    list = list.Where(h => h.LeopardName.Contains(leopardName));
                }

                if (weight>0)
                {
                    list = list.Where(h => h.Weight==weight);
                }

                return await list.ToListAsync();
            }
        }
    }
}
