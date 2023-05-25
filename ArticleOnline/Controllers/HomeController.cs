using ArticleOnline.Helpers;
using ArticleOnline.Models;
using ArticleOnline.Service;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ArticleOnline.Controllers
{
    public class HomeController : Controller
    {
        private ArticleService articleService;

        public HomeController()
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
            int pageSize = 4;
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
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(USER user)
        {
            if (ModelState.IsValid)
            {
                var check = articleService.CheckEmailExists(user.Email);
                if (check == null)
                {
                    user.Id = Guid.NewGuid();
                    user.DisplayName = "user";
                    user.Avatar = "/Content/img/defaultuser.png";
                    user.Deleted = false;
                    user.Role = "user";
                    user.JoinDate = DateTime.Now;
                    user.Password = articleService.GetMD5(user.Password);
                    articleService.AddUser(user);
                    TempData["SuccessMessage"] = "Đăng ký thành công!";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.errorRegister = "Email đã tồn tại";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            string returnUrl = Request.UrlReferrer?.ToString();
            Session["returnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(USER user)
        {
            ModelState.Remove("User.ConfirmPassword");
            if (ModelState.IsValid)
            {
                var data = articleService.CheckEmailExists(user.Email);
                if (data == null)
                {
                    ViewBag.errorEmail = "Email chưa được đăng ký!";
                    return View();
                }
                if (data.Password != articleService.GetMD5(user.Password))
                {
                    ViewBag.errorPassword = "Mật khẩu không chính xác!";
                    return View();
                }
                Session["DisplayName"] = data.DisplayName;
                Session["Email"] = data.Email;
                Session["Id"] = data.Id;
                Session["Role"] = data.Role;
                Session["Avatar"] = data.Avatar;

                string returnUrl = (string)Session["returnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    Session.Remove("returnUrl");
                    if (returnUrl.Contains("Register"))
                    {
                        return RedirectToAction("Index");
                    }

                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index");

            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        [CustomAuthorize(Roles = "admin")]
        public ActionResult Admin()
        {
            return RedirectToAction("Index", "Article", new { area = "Admin" });
        }

        //[HttpGet]
        //[CustomAuthorize(Roles = "admin,user")]
        //public ActionResult User()
        //{
        //    ArticleManagementModel objArticleModel = articleService.GetHomeModel();
        //    if (Session["Email"] != null && Session["Id"] != null)
        //    {
        //        string email = Session["Email"].ToString();
        //        string avatar = Session["Avatar"].ToString();
        //        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(avatar))
        //        {
        //            objArticleModel.User = new USER();
        //            objArticleModel.User.Email = email;
        //            objArticleModel.User.Avatar = avatar;
        //        }
        //    }
        //    return View(objArticleModel);
        //}

        [HttpGet]
        [CustomAuthorize(Roles = "admin, user")]
        public ActionResult User()
        {
            Guid id = Guid.Parse(Session["Id"].ToString());
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            objArticleModel.User = articleService.GetUserById(id);
            return View(objArticleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "admin, user")]
        public ActionResult User(USER User, HttpPostedFileBase ImageUpLoad)
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            Guid id = Guid.Parse(Session["Id"].ToString());
            USER user = articleService.GetUserById(id);
            if (string.IsNullOrEmpty(User.Password))
            {
                ModelState.Remove("User.Password");
                ModelState.Remove("User.ConfirmPassword");
                if (ImageUpLoad != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageUpLoad.FileName);
                    string extension = Path.GetExtension(ImageUpLoad.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    User.Avatar = "/Content/img/" + fileName;
                    ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), fileName));
                }
                else
                {
                    User.Avatar = user.Avatar;
                }
                User.Password = user.Password;
                articleService.UpdateUser(User);
                Session["SuccessMessage"] = "Thay đổi thông tin thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(ImageUpLoad.FileName);
                        string extension = Path.GetExtension(ImageUpLoad.FileName);
                        fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                        User.Avatar = "/Content/img/" + fileName;
                        ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), fileName));
                    }
                    else
                    {
                        User.Avatar = user.Avatar;
                    }
                    User.Password = articleService.GetMD5(User.Password);
                    articleService.UpdateUser(User);
                    objArticleModel.User = articleService.GetUserById(id);
                    Session["SuccessMessage"] = "Thay đổi thông tin thành công!";
                    return RedirectToAction("Index");
                }
            }
            objArticleModel.User = articleService.GetUserById(id);
            return View(objArticleModel);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "admin, user")]
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
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "admin, user")]
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
                Session["SuccessMessage"] = "Đăng thành công!";
                return Redirect(Request.UrlReferrer.ToString());
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin, user")]
        public ActionResult UploadImage(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N") + Path.GetExtension(upload.FileName);
                var imagePath = Path.Combine(Server.MapPath("~/Content/img/"), fileName);
                upload.SaveAs(imagePath);

                var imageUrl = Url.Content("~/Content/img/" + fileName);
                return Content("<script>window.parent.CKEDITOR.tools.callFunction(" + Request.QueryString["CKEditorFuncNum"] + ", '" + imageUrl + "');</script>");
            }

            return HttpNotFound();
        }
    }
}