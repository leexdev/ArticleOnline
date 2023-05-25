using PagedList;
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
        public List<Article> ListArticleAll { get; set; }
        public List<Comment> ListComment { get; set; }
        public List<Advertisement> advertisements { get; set; }
        public List<USER> ListUser { get; set; }
        public Category SelectedCategory { get; set; }
        public Article SelectedArticle { get; set; }
        public Category Category { get; set; }
        public Article Article { get; set; }
        public Comment Comment { get; set; }
        public IPagedList<ArticleManagementModel> PagedCategories { get; set; }
        public IPagedList<ArticleManagementModel> PagedArticles { get; set; }
        public IPagedList<ArticleManagementModel> PagedUsers { get; set; }
        public Article ArticleDelete { get; set; }
        public USER User { get; set; }
    }
}