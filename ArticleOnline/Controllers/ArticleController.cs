using ArticleOnline.Models;
using ArticleOnline.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ArticleOnline.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleService articleService;

        public ArticleController()
        {
            articleService = new ArticleService();
        }

        public ActionResult Detail(Guid id)
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            objArticleModel.SelectedArticle = articleService.GetArticle(id);
            ViewBag.CurrentArticle = articleService.CurrentArticle(id);
            return View(objArticleModel);
        }

        public ActionResult IncreaseViewCount(Guid id)
        {
            var article = articleService.GetArticle(id);
            if (article == null)
            {
                return HttpNotFound();
            }

            articleService.IncreaseViewCount(article);
            return RedirectToAction("Detail", new { id = article.Id });
        }
    }
}