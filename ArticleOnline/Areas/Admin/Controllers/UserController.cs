using ArticleOnline.Models;
using ArticleOnline.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArticleOnline.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private ArticleService articleService;

        public UserController()
        {
            articleService = new ArticleService();
        }


        public ActionResult Index()
        {
            ArticleManagementModel objArticleModel = articleService.GetUserModel();
            return View("Index", objArticleModel);
        }
    }
}