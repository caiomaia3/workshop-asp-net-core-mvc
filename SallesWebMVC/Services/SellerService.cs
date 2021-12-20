using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SallesWebMVC.Data;
using SallesWebMVC.Models;
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


        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj)
        {
            _context.Seller.Add(obj);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            return _context.Seller.Include(x => x.Department).FirstOrDefault(x => x.Id == id);
        }

        public void Remove(int id)
        {
            var sellerToRemove = FindById(id);
            _context.Seller.Remove(sellerToRemove);
            _context.SaveChanges();
        }
    }

}
