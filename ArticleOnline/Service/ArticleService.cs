﻿using ArticleOnline.Controllers;
using ArticleOnline.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ArticleOnline.Helpers;
using System;

namespace ArticleOnline.Service
{
    public class ArticleService
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

        public Category GetCategoryById(Guid id)
        {
            return GetCategories().FirstOrDefault(category => category.Id == id);
        }

        public List<Category> GetCategorySearch(string searchString)
        {
            diacriticsHelper diacriticsHelper = new diacriticsHelper();
            string normalizedSearchString = diacriticsHelper.RemoveDiacritics(searchString.ToUpper());

            return db.Categories
                .ToList()
                .Where(n => diacriticsHelper.RemoveDiacritics(n.Name.ToUpper()).Contains(normalizedSearchString) && !n.Deleted)
                .ToList();
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
            return db.Articles.OrderByDescending(a => a.PublishDate).Where(a => !a.Deleted).ToList();
        }

        public List<Article> GetArticleSearch(string searchString)
        {
            diacriticsHelper diacriticsHelper = new diacriticsHelper();
            string normalizedSearchString = diacriticsHelper.RemoveDiacritics(searchString.ToUpper());

            return db.Articles
                .ToList()
                .Where(n => diacriticsHelper.RemoveDiacritics(n.Title.ToUpper()).Contains(normalizedSearchString) && !n.Deleted)
                .ToList();
        }

        public void AddCategory(Category category)
        {
            db.Categories.Add(category);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            var existingCategory = db.Categories.Find(category.Id);

            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                db.SaveChanges();
            }
        }

        public void DeleteCategory(Category category)
        {
            category.Deleted = true;
            db.SaveChanges();
        }

        public List<Article> GetArticleCategory(Guid id)
        {
            return db.Articles.Where(n => n.CategoryId == id && !n.Deleted).ToList();
        }

        public Category CurrentCategory(Guid id)
        {
            return GetCategories().FirstOrDefault(category => category.Id == id);
        }

        public Article CurrentArticle(Guid id)
        {
            return GetArticles().FirstOrDefault(article => article.Id == id);
        }

        public void IncreaseViewCount(Article article)
        {
            article.ViewCount++;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public ArticleManagementModel GetHomeModel()
        {
            ArticleManagementModel objHomeModel = new ArticleManagementModel
            {
                ListCategory = GetCategories(),
                ListArticle = GetArticles(),
                ListArticleAll = GetArticles(),
                ListUser = GetUsers(),
            };
            return objHomeModel;
        }

        public ArticleManagementModel GetArticleModel()
        {
            ArticleManagementModel objArticleModel = new ArticleManagementModel
            {
                ListArticle = GetArticles()
            };
            return objArticleModel;
        }

        public ArticleManagementModel GetUserModel()
        {
            ArticleManagementModel objUserModel = new ArticleManagementModel
            {
                ListUser = GetUsers()
            };
            return objUserModel;
        }

        public USER GetUserById(Guid id)
        {
            return GetUsers().FirstOrDefault(user => user.Id == id);
        }

        public List<USER> GetUserSearch(string searchString)
        {
            diacriticsHelper diacriticsHelper = new diacriticsHelper();
            string normalizedSearchString = diacriticsHelper.RemoveDiacritics(searchString.ToUpper());

            return db.USERs
                .ToList()
                .Where(user => diacriticsHelper.RemoveDiacritics(user.Email.ToUpper()).Contains(normalizedSearchString) && !user.Deleted)
                .ToList();
        }

        public void AddArticle(Article article)
        {
            db.Articles.Add(article);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void UpdateArticle(Article article)
        {
            var existingArticle = db.Articles.Find(article.Id);

            if (existingArticle != null)
            {
                existingArticle.CategoryId = article.CategoryId;
                existingArticle.Title = article.Title;
                existingArticle.Description = article.Description;
                existingArticle.Content = article.Content;
                existingArticle.Avatar = article.Avatar;

                db.SaveChanges();
            }
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
        }
        public void UpdateUser(USER User)
        {
            var existingUser = db.USERs.Find(User.Id);

            if (existingUser != null)
            {
                existingUser.Avatar = User.Avatar;
                existingUser.DisplayName = User.DisplayName;
                existingUser.Password = User.Password;

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
            }
        }

        public void UpdateUserAdmin(USER User)
        {
            var existingUser = db.USERs.Find(User.Id);

            if (existingUser != null)
            {
                existingUser.Avatar = User.Avatar;  
                existingUser.DisplayName = User.DisplayName;
                existingUser.Role = User.Role;

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
            }
        }

        public void DeleteUser(USER user)
        {
            Guid targetUserId = new Guid("4a7a0469-ea5e-4eb3-8071-79744228d184");

            if (user.Id != targetUserId)
            {
                user.Deleted = true;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
            }
        }

        public void DeleteArticle(Article article)
        {
            article.Deleted = true;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        public void AddComment(Comment comment)
        {
            db.Comments.Add(comment);
            db.Configuration.ValidateOnSaveEnabled = false;
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
