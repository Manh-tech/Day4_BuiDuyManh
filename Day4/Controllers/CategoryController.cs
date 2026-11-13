
using Microsoft.AspNetCore.Mvc;
using Day4.Models;
using Day4.Data;

namespace Day4.Controllers
{
    public class CategoryController : Controller
    {
        // DANH SACH + TIM KIEM + SAP XEP + PHAN TRANG
        public IActionResult Index(
            string? search,
            string? sort,
            int page = 1)
        {
            var categories = SeedData.categories.AsEnumerable();

            // TIM KIEM
            if (!string.IsNullOrWhiteSpace(search))
            {
                categories = categories.Where(c =>
                    c.Name != null &&
                    c.Name.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase));
            }

            // SAP XEP
            switch (sort)
            {
                case "name_desc":
                    categories = categories.OrderByDescending(c => c.Name);
                    break;

                case "id_desc":
                    categories = categories.OrderByDescending(c => c.Id);
                    break;

                case "id_asc":
                    categories = categories.OrderBy(c => c.Id);
                    break;

                default:
                    categories = categories.OrderBy(c => c.Name);
                    break;
            }

            // PHAN TRANG
            int pageSize = 5;
            int totalItems = categories.Count();

            int totalPages = (int)Math.Ceiling(
                (double)totalItems / pageSize);

            if (page < 1)
                page = 1;

            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            var result = categories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(result);
        }

        // THEM CATEGORY - GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // THEM CATEGORY - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = SeedData.categories.Any()
                    ? SeedData.categories.Max(c => c.Id) + 1
                    : 1;

                SeedData.categories.Add(category);

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // SUA CATEGORY - GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = SeedData.categories
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // SUA CATEGORY - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var oldCategory = SeedData.categories
                    .FirstOrDefault(c => c.Id == id);

                if (oldCategory == null)
                    return NotFound();

                oldCategory.Name = category.Name;

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // XOA CATEGORY - GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = SeedData.categories
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // XOA CATEGORY - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = SeedData.categories
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            // Khong cho xoa Category dang co Product
            if (SeedData.products.Any(p => p.CategoryId == id))
            {
                ModelState.AddModelError("",
                    "Khong the xoa danh muc dang co san pham.");

                return View("Delete", category);
            }

            SeedData.categories.Remove(category);

            return RedirectToAction("Index");
        }
    }
}
