using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using N_Tier_Architecture_DataAccess.Repository;
using N_Tier_Architecture_DataAccess.Repository.IRepository;
using N_Tier_Architecture_Models;
using N_Tier_Architecture_Models.ViewModels;

namespace N_Tier_Architecture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitofWork _unitofwork;
        private readonly IWebHostEnvironment _env;

        public ProductController(IUnitofWork unitofWork, IWebHostEnvironment env)
        {
            this._unitofwork = unitofWork;
            this._env = env;
        }
        public IActionResult Index()
        {
            var products = _unitofwork.Product.GetAllProduct();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitofwork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                //Product = new Product()
            };
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM)
        {
            //if (ModelState.IsValid)
            //{
            productVM.CategoryList = _unitofwork.Category
            .GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            //Image Uploading Work Start Here
            string filename = "";
            if (productVM.Photo != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "images");
                filename = Path.GetFileName(productVM.Photo.FileName); // Get only the file name
                string filepath = Path.Combine(folder, filename);
                // Save the file
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    productVM.Photo.CopyTo(fileStream);
                }

            }//Image Uploading Work End Here

            // Create a new Product object
            Product prod = new Product()
            {
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                CategoryId = productVM.CategoryId,
                ImageUrl = filename
            };

            // Add the product to the database
            _unitofwork.Product.Add(prod);
            _unitofwork.Save();
            TempData["success"] = "Product has been Added Successfully!";
            return RedirectToAction("Index");


            //}

            // If model is invalid, reload categories
            //productVM.CategoryList = _unitofwork.Category
            //    .GetAll().Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });
            //else
            //{
            //    TempData["success"] = "Product has not been Added!";
            //}
            //return View(productVM);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var databy_id = _unitofwork.Product.GetById(x => x.Id == id);
            var productVM = new ProductVM()
            {
                Name = databy_id.Name,
                Description = databy_id.Description,
                Price = databy_id.Price,
                CategoryId = databy_id.CategoryId,
                ImageUrl = databy_id.ImageUrl
            };
            productVM.CategoryList = _unitofwork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM)
        {
            var data = _unitofwork.Product.GetById(x => x.Id == productVM.Id);
            if(data != null)
            {
                string filename = string.Empty;
                //If photo is sent from user means not null below
                if (productVM.Photo != null)
                {
                    if (data.ImageUrl != null)
                    {
                        //Find Old Image from folder and then delete that Old image
                        string present_image = Path.Combine(_env.WebRootPath, "images", data.ImageUrl);
                        if (present_image != null)
                        {
                            if (System.IO.File.Exists(present_image))
                            {
                                System.IO.File.Delete(present_image);
                            }
                        }
                    }
                    //Create new Image into the folder
                    string folder = Path.Combine(_env.WebRootPath, "images");
                    filename = Path.GetFileName(productVM.Photo.FileName); // Get only the file name
                    string filepath = Path.Combine(folder, filename);
                    // Save the file
                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        //Image Upload into Images folder
                        productVM.Photo.CopyTo(fileStream);
                    }
                }
                else
                {
                    //Set Old Image from database with updated data 
                    filename = data.ImageUrl;
                }

                // Set data from productVM model
                data.Name = productVM.Name;
                data.Description = productVM.Description;
                data.Price = productVM.Price;
                data.CategoryId = productVM.CategoryId;
                data.ImageUrl = filename;

                _unitofwork.Product.Update(data);
                _unitofwork.Save();
                TempData["success"] = "Product has been Updated Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["success"] = "Product Updation Failed!";
                return View(productVM);
            }

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = _unitofwork.Product.GetById(x => x.Id == id);
            if (data == null) { 
                return NotFound();
            }
            else
            {
                //Create ProductVM object and asign database particular data into ProductVM object
                var productVM = new ProductVM()
                {
                    Name = data.Name,
                    Description = data.Description,
                    Price = data.Price,
                    CategoryId = data.CategoryId,
                    ImageUrl = data.ImageUrl
                };
                //Select Category from Category table
                productVM.CategoryList = _unitofwork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(productVM);
            }
            
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var data = _unitofwork.Product.GetById(x => x.Id == id);
            if (data != null)
            {
                string delete_image_from_folder = Path.Combine(_env.WebRootPath, "images");
                string present_image = Path.Combine(Directory.GetCurrentDirectory(), delete_image_from_folder, data.ImageUrl);
                if (present_image != null)
                {
                    if (System.IO.File.Exists(present_image))
                    {
                        System.IO.File.Delete(present_image);
                    }
                }
                _unitofwork.Product.Remove(data);
                _unitofwork.Save();
                TempData["success"] = "Product and Image has been Deleted Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["success"] = "Product and Image Deletion Failed!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var data = _unitofwork.Product.GetById(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                var productVM = new ProductVM()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description,
                    Price = data.Price,
                    CategoryId = data.CategoryId,
                    ImageUrl = data.ImageUrl
                };
                productVM.CategoryList = _unitofwork.Category.GetAll().Select(cat => new SelectListItem
                {
                    Text = cat.Name,
                    Value = cat.Id.ToString()
                });
                return View(productVM);
            }
        }
    }
}
