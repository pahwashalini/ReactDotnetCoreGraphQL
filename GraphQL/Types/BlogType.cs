using Forum.Models;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.GraphQL.Types
{
    public class BlogType: ObjectGraphType<BlogPost>
    {
        public BlogType()
        {
            Field(x => x.BlogPostID);
            Field(x => x.AuthorID);
            Field(x => x.BlogText);
            Field(x => x.PostDate);
            Field(x => x.Title);
            Field<AuthorType>(nameof(BlogPost.BlogPostAuthor));
           

        }
    }
}
