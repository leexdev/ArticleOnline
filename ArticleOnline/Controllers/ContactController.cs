using ArticleOnline.Models;
using ArticleOnline.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArticleOnline.Controllers
{
    public class ContactController : Controller
    {
        private ArticleService articleService;

        public ContactController()
        {
            articleService = new ArticleService();
        }

        public ActionResult Index()
        {
            ArticleManagementModel objArticleModel = articleService.GetHomeModel();
            return View(objArticleModel);
        }
    }
}