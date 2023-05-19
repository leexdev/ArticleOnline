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

        public ActionResult Index(string SearchString)
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            if (!string.IsNullOrEmpty(SearchString))
            {
                objArticleModel.ListUser = articleService.GetUserSearch(SearchString);
            }
            return View(objArticleModel);
        }
    }
}