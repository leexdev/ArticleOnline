using ArticleOnline.Models;
using ArticleOnline.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArticleOnline.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private ArticleService articleService;

        public CategoryController()
        {
            articleService = new ArticleService();
        }

        [CustomAuthorize(Roles = "admin")]
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var objArticleModel = articleService.GetHomeModel();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                objArticleModel.ListCategory = articleService.GetCategorySearch(searchString).ToList();
            }
            else
            {
                objArticleModel.ListCategory = articleService.GetCategories().ToList();
            }

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedCategories = objArticleModel.ListCategory.ToPagedList(pageNumber, pageSize);
            var pagedCategoryModels = new StaticPagedList<ArticleManagementModel>(
                pagedCategories.Select(a => new ArticleManagementModel
                {
                    ListCategory = new List<Category> { a }
                }),
                pagedCategories.GetMetaData()
            );
            objArticleModel.PagedCategories = pagedCategoryModels;
            return View(objArticleModel);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create(Guid id)
        {
            var data = articleService.GetCategoryById(id);

            var model = new ArticleManagementModel
            {
                Category = data
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create(Category category)
        {
            category.Id = Guid.NewGuid();
            articleService.AddCategory(category);
            Session["SuccessMessage"] = "Thêm thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Delete(Guid id)
        {
            var category = articleService.GetCategoryById(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            articleService.DeleteCategory(category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Edit(Guid id)
        {
            var data = articleService.GetCategoryById(id);

            var model = new ArticleManagementModel
            {
                Category = data
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Edit(Category category)
        {
            articleService.UpdateCategory(category);
            Session["SuccessMessage"] = "Sửa thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}