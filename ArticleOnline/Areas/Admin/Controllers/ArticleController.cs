using ArticleOnline.Models;
using ArticleOnline.Service;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ArticleOnline.Areas.Admin.Controllers
{
    public class ArticleController : Controller
    {

        private ArticleService articleService;

        public ArticleController()
        {
            articleService = new ArticleService();
        }


        public ActionResult Index()
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            return View(objArticleModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Article article, HttpPostedFileBase ImageUpLoad)
        {
            if (ImageUpLoad != null)
            {
                article.Id = Guid.NewGuid();
                string fileName = Path.GetFileNameWithoutExtension(ImageUpLoad.FileName);
                string extension = Path.GetExtension(ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                article.Avatar = "/Content/img/" + fileName;
                ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), fileName));
                article.PublishDate = DateTime.Now;
                Guid authorId;
                if (Session["Id"] != null && Guid.TryParse(Session["Id"].ToString(), out authorId))
                {
                    article.AuthorId = authorId;
                }
                articleService.AddArticle(article);
                List<Article> articles = articleService.GetLatestArticles();
                ArticleManagementModel objArticleModel = articleService.GetHomeModel();
                objArticleModel.ListArticle = articles;
                return View("Index", objArticleModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(Article article)
        {
            var data = articleService.GetArticle(article.Id);
            articleService.DeleteArticle(data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var data = articleService.GetArticle(id);

            var model = new ArticleManagementModel
            {
                Article = data
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Article article, HttpPostedFileBase ImageUpLoad)
        {
            if (ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(ImageUpLoad.FileName);
                string extension = Path.GetExtension(ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                article.Avatar = "/Content/img/" + fileName;
                ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), fileName));
            }
            articleService.UpdateArticle(article);
            return RedirectToAction("Index");
        }
    }
}