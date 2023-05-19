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
            objArticleModel.ListArticle = articleService.GetArticleCategory(id);

            Category currentCategory = articleService.CurrentCategory(id);
            ViewBag.CurrentCategory = currentCategory.Name;

            return View(objArticleModel);
        }

        
    }
}