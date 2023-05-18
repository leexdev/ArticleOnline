﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArticleOnline.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;
    using System.Web.Mvc;

    public partial class Article
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Article()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public System.Guid Id { get; set; }

        [Required(ErrorMessage = "Vui lòng thêm ảnh đại diện!")]
        [DisplayName("Ảnh đại diện")]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "Vui lòng thêm tiêu đề!")]
        [DisplayName("Tiêu đề")]
        [StringLength(200, ErrorMessage = "Độ dài không được vượt quá 200 ký tự")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng thêm mô tả!")]
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng thêm nội dung")]
        [DisplayName("Nội dung")]
        [AllowHtml]
        public string Content { get; set; }

        public System.Guid AuthorId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục!")]
        [DisplayName("Thể loại")]
        public System.Guid CategoryId { get; set; }

        [DisplayName("Ngày đăng")]
        public System.DateTime PublishDate { get; set; }
        public int ViewCount { get; set; }
        public bool Deleted { get; set; }
    
        public virtual USER USER { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}