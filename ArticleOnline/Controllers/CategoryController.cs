using ArticleOnline.Models;
using ArticleOnline.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArticleOnline.Controllers
{
    public class CategoryController : Controller
    {
        private ArticleService articleService;

        public CategoryController()
        {
            articleService = new ArticleService();
        }

        public ActionResult Index()
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            return View(objArticleModel);
        }

        public ActionResult ArticleCategory(Guid id)
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            var listArticle = objArticleModel.ListArticle.Where(n => n.CategoryId == id).ToList();
            objArticleModel.ListContextArticle = listArticle;

            Category currentCategory = CurrentCategory(id);
            ViewBag.CurrentCategory = currentCategory;
            ViewBag.Title = currentCategory.Name;

            return View(objArticleModel);
        }

        public Category CurrentCategory(Guid id)
        {
            var categories = articleService.GetCategories();
            if (categories != null)
            {
                foreach (Category category in categories)
                {
                    if (category.Id == id)
                    {
                        return category;
                    }
                }
            }
            return null;
        }

    }
}