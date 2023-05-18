using ArticleOnline.Helpers;
using ArticleOnline.Models;
using ArticleOnline.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
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

        public ActionResult Index()
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
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
                TempData["SuccessMessage"] = "Đăng nhập thành công!";
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

        public ActionResult User()
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            return View(objArticleModel);
        }

        [HttpPost]
        public ActionResult CreateArticle()
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            return View(objArticleModel);
        }
    }
}