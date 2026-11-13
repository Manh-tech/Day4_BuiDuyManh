
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Day4.Models;
using Day4.Data;

namespace Day4.Controllers
{
    public class ProductController : Controller
    {
        // DANH SACH + TIM KIEM + SAP XEP + PHAN TRANG
        public IActionResult Index(
            string? search,
            string? sort,
            int page = 1)
        {
            var products = SeedData.products.AsEnumerable();

            // TIM KIEM
            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p =>
                    p.Name != null &&
                    p.Name.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase));
            }

            // SAP XEP
            switch (sort)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;

                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;

                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;

                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            // PHAN TRANG
            int pageSize = 5;
            int totalItems = products.Count();

            int totalPages = (int)Math.Ceiling(
                (double)totalItems / pageSize);

            if (page < 1)
                page = 1;

            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            var result = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // LAY THONG TIN CATEGORY
            foreach (var product in result)
            {
                product.Category = SeedData.categories
                    .FirstOrDefault(c =>
                        c.Id == product.CategoryId);
            }

            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(result);
        }

        // LOAD DROPDOWN CATEGORY
        private void LoadCategories()
        {
            ViewBag.Categories = new SelectList(
                SeedData.categories, "Id", "Name");
        }

        // THEM SAN PHAM - GET
        [HttpGet]
        public IActionResult Create()
        {
            LoadCategories();
            return View();
        }

        // THEM SAN PHAM - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!SeedData.categories.Any(c =>
                c.Id == product.CategoryId))
            {
                ModelState.AddModelError(
                    "CategoryId",
                    "Vui long chon danh muc hop le");
            }

            if (ModelState.IsValid)
            {
                product.Id = SeedData.products.Any()
                    ? SeedData.products.Max(p => p.Id) + 1
                    : 1;

                SeedData.products.Add(product);

                return RedirectToAction("Index");
            }

            LoadCategories();
            return View(product);
        }

        // SUA SAN PHAM - GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = SeedData.products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            LoadCategories();
            return View(product);
        }

        // SUA SAN PHAM - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            if (!SeedData.categories.Any(c =>
                c.Id == product.CategoryId))
            {
                ModelState.AddModelError(
                    "CategoryId",
                    "Vui long chon danh muc hop le");
            }

            if (ModelState.IsValid)
            {
                var oldProduct = SeedData.products
                    .FirstOrDefault(p => p.Id == id);

                if (oldProduct == null)
                    return NotFound();

                oldProduct.Name = product.Name;
                oldProduct.Price = product.Price;
                oldProduct.Description = product.Description;
                oldProduct.CategoryId = product.CategoryId;

                return RedirectToAction("Index");
            }

            LoadCategories();
            return View(product);
        }

        // XOA SAN PHAM - GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = SeedData.products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            product.Category = SeedData.categories
                .FirstOrDefault(c =>
                    c.Id == product.CategoryId);

            return View(product);
        }

        // XOA SAN PHAM - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = SeedData.products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            SeedData.products.Remove(product);

            return RedirectToAction("Index");
        }
    }
}
