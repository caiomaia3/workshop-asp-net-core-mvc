using SallesWebMVC.Data;
using SallesWebMVC.Models;
using System.Collections.Generic;
using System.Linq;


namespace SallesWebMVC.Services
{
    public class DepartmentService
    {
        private readonly SallesWebMVCContext _context;

        public DepartmentService(SallesWebMVCContext context)
        {
            _context = context;
        }

        public List<Department> ListAll()
        {
            return _context.Department.OrderBy(x => x.Name).ToList();
        }
    }
}
