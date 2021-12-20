using SallesWebMVC.Models;
using SallesWebMVC.Models.ViewModels;
using SallesWebMVC.Services;
using SallesWebMVC.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace SallesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();

            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.ListAllAsync();
            var viewModel = new SellerFormViewModel() { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.ListAllAsync();
                var viewModel = new SellerFormViewModel() { Seller=seller, Departments = departments };
                return View(viewModel);
            }
            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            var sellerToDelete = await _sellerService.FindByIdAsync(id.Value);
            if (sellerToDelete == null) return RedirectToAction(nameof(Error), new { message = "Id not found!" });


            return View(sellerToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided!" });

            var sellerToDetail = await _sellerService.FindByIdAsync(id.Value);
            if (sellerToDetail == null) return RedirectToAction(nameof(Error), new { message = "Id not found!" });

            return View(sellerToDetail);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            var sellerToEdit = await _sellerService.FindByIdAsync(id.Value);
            if (sellerToEdit == null) return RedirectToAction(nameof(Error), new { message = "Id not found!" });

            List<Department> departments =await _departmentService.ListAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel() { Seller = sellerToEdit, Departments = departments };

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.ListAllAsync();
                var viewModel = new SellerFormViewModel() { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch!" });
            }
            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
