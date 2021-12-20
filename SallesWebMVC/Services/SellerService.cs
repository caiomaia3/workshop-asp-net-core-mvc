using SallesWebMVC.Data;
using SallesWebMVC.Models;
using SallesWebMVC.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace SallesWebMVC.Services
{
    public class SellerService
    {
        private readonly SallesWebMVCContext _context;

        public SellerService(SallesWebMVCContext context)
        {
            _context = context;
        }


        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller obj)
        {
            _context.Seller.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var sellerToRemove = await FindByIdAsync(id);
                _context.Seller.Remove(sellerToRemove);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }

        }

        public async Task UpdateAsync(Seller seller)
        {
            bool IdIsValid = await _context.Seller.AnyAsync(x => x.Id == seller.Id);

            if(!IdIsValid)
            {
                throw new NotFoundException("Id not found!");
            }
            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

        }
    }

}
