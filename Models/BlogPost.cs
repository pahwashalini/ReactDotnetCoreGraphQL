using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class BlogPost
    {
        [Key]
        public int BlogPostID { get; set; }
        public string Title { get; set; }
        public string BlogText { get; set; }
        public DateTime PostDate { get; set; }
        [ForeignKey("AuthorID")]
        public int AuthorID { get; set; }
        public Author BlogPostAuthor { get; set; }
        public BlogPost(string Title, string BlogText)
        {
            this.Title = Title;
            this.BlogText = BlogText;
        }
    }
}
