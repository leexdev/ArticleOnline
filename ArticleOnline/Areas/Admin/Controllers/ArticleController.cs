using ArticleOnline.Models;
using ArticleOnline.Service;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArticleOnline.Areas.Admin.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleService articleService;

        public ArticleController()
        {
            articleService = new ArticleService();
        }

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
                objArticleModel.ListArticle = articleService.GetArticleSearch(searchString).ToList();
            }
            else
            {
                objArticleModel.ListArticle = articleService.GetArticles().ToList();
            }

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedArticles = objArticleModel.ListArticle.OrderByDescending(a => a.PublishDate).ToPagedList(pageNumber, pageSize);
            var pagedArticleModels = new StaticPagedList<ArticleManagementModel>(
                pagedArticles.Select(a => new ArticleManagementModel
                {
                    ListArticle = new List<Article> { a }
                }),
                pagedArticles.GetMetaData()
            );
            objArticleModel.PagedArticles = pagedArticleModels;
            return View(objArticleModel);
        }

        [HttpGet]
        public ActionResult Create(Guid id)
        {
            var data = articleService.GetArticle(id);

            var model = new ArticleManagementModel
            {
                Article = data
            };

            return View(model);
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
                if (Session["Id"] is Guid authorId)
                {
                    article.AuthorId = authorId;
                }
                articleService.AddArticle(article);
                var articles = articleService.GetLatestArticles();
                var objArticleModel = articleService.GetHomeModel();
                objArticleModel.ListArticle = articles;
                Session["SuccessMessage"] = "Thêm thành công!";
                return Redirect(Request.UrlReferrer.ToString());
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var article = articleService.GetArticle(id);

            if (article == null)
            {
                return HttpNotFound();
            }

            articleService.DeleteArticle(article);
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
            Session["SuccessMessage"] = "Sửa thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}
