using ArticleOnline.Models;
using ArticleOnline.Service;
using System;
using System.Collections.Generic;
using System.IO;
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
            Session["returnUrl"] = Request.Url.ToString();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment, Guid articleId)
        {
            Guid id = Guid.Parse(Session["Id"].ToString());
            comment.Id = Guid.NewGuid();
            comment.AuthorId = id;
            comment.PublishDate = DateTime.Now;
            comment.ArticleId = articleId;
            articleService.AddComment(comment);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}