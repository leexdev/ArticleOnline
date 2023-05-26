using ArticleOnline.Models;
using ArticleOnline.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ArticleOnline.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private ArticleService articleService;

        public UserController()
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
                objArticleModel.ListUser = articleService.GetUserSearch(searchString).ToList();
            }
            else
            {
                objArticleModel.ListUser = articleService.GetUsers().ToList();
            }

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedUsers = objArticleModel.ListUser.OrderByDescending(a => a.JoinDate).ToPagedList(pageNumber, pageSize);
            var pagedUserModels = new StaticPagedList<ArticleManagementModel>(
                pagedUsers.Select(a => new ArticleManagementModel
                {
                    ListUser = new List<USER> { a }
                }),
                pagedUsers.GetMetaData()
            );
            objArticleModel.PagedUsers = pagedUserModels;
            return View(objArticleModel);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create(Guid id)
        {
            var data = articleService.GetUserById(id);

            var model = new ArticleManagementModel
            {
                User = data
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create(USER user, HttpPostedFileBase ImageUpLoad)
        {
            if (ImageUpLoad != null)
            {
                user.Id = Guid.NewGuid();
                string fileName = Path.GetFileNameWithoutExtension(ImageUpLoad.FileName);
                string extension = Path.GetExtension(ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                user.Avatar = "/Content/img/" + fileName;
                ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), fileName));
                user.JoinDate = DateTime.Now;
                user.Password = articleService.GetMD5("123456Aa@");
                articleService.AddUser(user);
                var objArticleModel = articleService.GetHomeModel();
                Session["SuccessMessage"] = "Thêm thành công!";
                return Redirect(Request.UrlReferrer.ToString());
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var user = articleService.GetUserById(id);

                if (user == null)
                {
                    return HttpNotFound();
                }

                articleService.DeleteUser(user);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Không thể xóa!";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Edit(Guid id)
        {
            var data = articleService.GetUserById(id);

            var model = new ArticleManagementModel
            {
                User = data
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Edit(USER user, HttpPostedFileBase ImageUpLoad)
        {
            if (ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(ImageUpLoad.FileName);
                string extension = Path.GetExtension(ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                user.Avatar = "/Content/img/" + fileName;
                ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), fileName));
            }
            articleService.UpdateUserAdmin(user);
            Session["SuccessMessage"] = "Sửa thành công!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [CustomAuthorize(Roles = "user")]
        public ActionResult ShowArticle(string currentFilter, string searchString, int? page)
        {
            var objArticleModel = articleService.GetHomeModel();

            Guid authorId = (Guid)Session["Id"];

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
                objArticleModel.ListArticle = articleService.GetArticleSearch(searchString).Where(a => a.AuthorId == authorId).ToList();
            }
            else
            {
                objArticleModel.ListArticle = articleService.GetArticles().Where(a => a.AuthorId == authorId).ToList();
            }

            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedArticles = objArticleModel.ListArticle.Where(a => a.AuthorId == authorId).OrderByDescending(a => a.PublishDate).ToPagedList(pageNumber, pageSize);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "user")]
        public ActionResult CreateArticle(Article article, HttpPostedFileBase ImageUpLoad)
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
                return RedirectToAction("ShowArticle");
            }

            return RedirectToAction("ShowArticle");
        }

        [HttpPost]
        [CustomAuthorize(Roles = "user")]
        public ActionResult DeleteArticle(Guid id)
        {
            var article = articleService.GetArticle(id);

            if (article == null)
            {
                return HttpNotFound();
            }

            articleService.DeleteArticle(article);
            return RedirectToAction("ShowArticle");
        }

        [HttpPost]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "user")]
        public ActionResult EditArticle(Article article, HttpPostedFileBase ImageUpLoad)
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
            return RedirectToAction("ShowArticle");
        }

        [HttpPost]
        [CustomAuthorize(Roles = "user")]
        public ActionResult UploadImage(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                // Tạo tên tập tin duy nhất bằng cách sử dụng thời gian và Guid
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N") + Path.GetExtension(upload.FileName);
                var imagePath = Path.Combine(Server.MapPath("~/Content/img/"), fileName);
                upload.SaveAs(imagePath);

                // Trả về URL của hình ảnh đã tải lên
                var imageUrl = Url.Content("~/Content/img/" + fileName);
                return Content("<script>window.parent.CKEDITOR.tools.callFunction(" + Request.QueryString["CKEditorFuncNum"] + ", '" + imageUrl + "');</script>");
            }

            return HttpNotFound();
        }
    }
}