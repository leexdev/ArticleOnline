using ArticleOnline.Controllers;
using ArticleOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ArticleOnline.Helpers;

namespace ArticleOnline.Service
{
    public class ArticleService : DbContext
    {
        private ArticleManagementSystemEntities db;

        public ArticleService()
        {
            db = new ArticleManagementSystemEntities();
        }

        public List<Category> GetCategories()
        {
            return db.Categories.Where(category => !category.Deleted).ToList();
        }

        public List<Article> GetArticles()
        {
            return db.Articles.Where(article => !article.Deleted).ToList();
        }

        public List<USER> GetUsers()
        {
            return db.USERs.Where(user => !user.Deleted).ToList();
        }

        public List<Comment> GetComments()
        {
            return db.Comments.ToList();
        }

        public List<Advertisement> GetAdvertisements()
        {
            return db.Advertisements.ToList();
        }

        public List<Article> GetLatestArticles()
        {
            List<Article> articles = db.Articles.OrderByDescending(a => a.PublishDate).Where(a => !a.Deleted).ToList();
            return articles;
        }

        public List<Article> GetArticleSearch(string SearchString)
        {
            diacriticsHelper diacriticsHelper = new diacriticsHelper();
            string normalizedSearchString = diacriticsHelper.RemoveDiacritics(SearchString.ToUpper());

            List<Article> articles = db.Articles
                .ToList()
                .Where(n => diacriticsHelper.RemoveDiacritics(n.Title.ToUpper()).Contains(normalizedSearchString) && !n.Deleted)
                .ToList();

            return articles;
        }

        public List<Article> GetArticleCategory(Guid id)
        {
            return db.Articles.Where(n => n.CategoryId == id && !n.Deleted).ToList();
        }

        public Category CurrentCategory(Guid id)
        {
            var categories = GetCategories();
            if (categories != null)
            {
                foreach (Category category in categories)
                {
                    if (category.Id == id)
                    {
                        return category;
                    }
                }
            }
            return null;
        }


        public void IncreaseViewCount(Article article)
        {
            article.ViewCount++;
            db.SaveChanges();
        }

        public ArticleManagementModel GetHomeModel()
        {
            ArticleManagementModel objHomeModel = new ArticleManagementModel();
            objHomeModel.ListCategory = GetCategories();
            objHomeModel.ListArticle = GetArticles();
            objHomeModel.ListArticleAll = GetArticles();
            objHomeModel.ListUser = GetUsers();
            return objHomeModel;
        }

        public ArticleManagementModel GetUserModel()
        {
            ArticleManagementModel objUserModel = new ArticleManagementModel();
            objUserModel.ListUser = GetUsers();
            return objUserModel;
        }

        public List<USER> GetUserSearch(string SearchString)
        {
            diacriticsHelper diacriticsHelper = new diacriticsHelper();
            string normalizedSearchString = diacriticsHelper.RemoveDiacritics(SearchString.ToUpper());

            List<USER> uSERs = db.USERs
                .ToList()
                .Where(n => diacriticsHelper.RemoveDiacritics(n.Email.ToUpper()).Contains(normalizedSearchString) && !n.Deleted)
                .ToList();

            return uSERs;
        }

        public void AddArticle(Article article)
        {
            db.Articles.Add(article);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            db.Configuration.ValidateOnSaveEnabled = true;
        }

        public void UpdateArticle(Article article)
        {
            db.Entry(article).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Article GetArticle(Guid id)
        {
            return db.Articles.SingleOrDefault(article => article.Id == id && !article.Deleted);
        }

        public USER CheckEmailExists(string email)
        {
            return db.USERs.FirstOrDefault(u => u.Email == email && !u.Deleted);
        }

        public void AddUser(USER _user)
        {
            db.USERs.Add(_user);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            db.Configuration.ValidateOnSaveEnabled = true;
        }
        public void DeleteCategory(Category category)
        {
            category.Deleted = true;
            db.SaveChanges();
        }
        public void DeleteUser(USER user)
        {
            user.Deleted = true;
            db.SaveChanges();
        }

        public void DeleteArticle(Article article)
        {
            article.Deleted = true;
            db.SaveChanges();
        }

        public string GetMD5(string str)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(str);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}