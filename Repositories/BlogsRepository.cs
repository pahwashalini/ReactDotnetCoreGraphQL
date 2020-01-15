using Forum.Data;
using Forum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Repositories
{
    public class BlogsRepository
    {
        private readonly BlogDbContext _BlogDbContext;
        public BlogsRepository(BlogDbContext blogDbContext)
        {
            this._BlogDbContext = blogDbContext;
        }
        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _BlogDbContext
                .Author
                .Include(x => x.Blogs)
                .ToListAsync();
        }
       
        public async Task<IEnumerable<BlogPost>> GetAllBlogs()
      
        {
            return await _BlogDbContext
                .BlogPost
                .Include(x => x.BlogPostAuthor)
                .ToListAsync();
        }
        public IIncludableQueryable<BlogPost, Author> GetQuery()
        {
            return _BlogDbContext
                 .BlogPost
                 .Include(x => x.BlogPostAuthor);
                
        }
      
        public BlogPost Get(int id)
        {
            return GetQuery().Single(x => x.BlogPostID == id);
        }

    }
}
