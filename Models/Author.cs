using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Author
    {
        public int AuthorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string UserPassword { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime DateJoined { get; set; }
        public List<BlogPost> Blogs { get; set; }
        public Author(string FirstName, string LastName ,string EmailID , string UserPassword)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.EmailID = EmailID;
            this.UserPassword = UserPassword;
        }
    }
}
