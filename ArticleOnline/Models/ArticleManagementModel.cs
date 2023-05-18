using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArticleOnline.Models
{
    public class ArticleManagementModel
    {
        public List<Category> ListCategory { get; set; }
        public List<Article> ListArticle { get; set; }
        public List<Comment> ListComment { get; set; }
        public List<Advertisement> advertisements { get; set; }
        public List<USER> ListUser { get; set; }
        public Category SelectedCategory { get; set; }
        public Article SelectedArticle { get; set; }
        public Article Article { get; set; }
        public Article ArticleDelete { get; set; }
        public List<ArticleOnline.Models.Article> ListContextArticle { get; set; }
        public USER User { get; set; }
    }
}