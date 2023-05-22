using ArticleOnline.Helpers;
using ArticleOnline.Models;
using ArticleOnline.Service;
using System;
using System.Collections.Generic;
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

        public ActionResult Index(string SearchString)
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            if (!string.IsNullOrEmpty(SearchString))
            {
                objArticleModel.ListArticle = articleService.GetArticleSearch(SearchString);
                objArticleModel.ListArticleAll = articleService.GetArticles();
                ViewBag.CurrentFilter = SearchString;
            }
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
            // Lưu địa chỉ trang trước đó vào biến session
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
                TempData["SuccessMessage"] = "Đăng nhập thành công!";

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
        public ActionResult User()
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            return View(objArticleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult User(USER User, HttpPostedFileBase ImageUpLoad)
        {
            Guid id = Guid.Parse(Session["Id"].ToString());
            var existingUser = articleService.GetUserById(id);
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
                User.Avatar = existingUser.Avatar;
            }
            User.Id = id;
            if (string.IsNullOrEmpty(User.Password))
            {
                User.Password = existingUser.Password;
            }
            articleService.UpdateUser(User);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult CreateArticle()
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            return View(objArticleModel);
        }
    }
}