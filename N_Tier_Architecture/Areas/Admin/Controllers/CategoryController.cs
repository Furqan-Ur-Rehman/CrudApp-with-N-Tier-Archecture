using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier_Architecture_DataAccess.Data;
using N_Tier_Architecture_DataAccess.Repository.IRepository;
using N_Tier_Architecture_Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace N_Tier_Architecture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController(IUnitofWork _unitofwork) : Controller
    {
        //private readonly IUnitofWork _unitofwork;

        public IActionResult Index()
        {
            var category = _unitofwork.Category.GetAll();
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Add(category);
                _unitofwork.Save();
                TempData["success"] = "Category has been Added Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["success"] = "Category has not been Added!";
                return View();
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var databy_id = _unitofwork.Category.GetById(x => x.Id == id);
            return View(databy_id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Update(category);
                _unitofwork.Save();
                TempData["success"] = "Category has been Updated Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["success"] = "Category Updation Failed!";
                return View();
            }

        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = _unitofwork.Category.GetById(x=>x.Id == id);
            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var data = _unitofwork.Category.GetById(x=>x.Id==id);
            if (data != null)
            {
                _unitofwork.Category.Remove(data);
                _unitofwork.Save();
                TempData["success"] = "Category has been Deleted Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["success"] = "Category Deletion Failed!";
                return View();
            }
        }
    }
}
