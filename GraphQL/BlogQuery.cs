using Forum.GraphQL.Types;
using Forum.Models;
using Forum.Repositories;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Forum.GraphQL
{
    public class BlogQuery : ObjectGraphType
    {
        /*
        -- Simple test query --

         query TestQuery {
  blogPosts {
    blogPostID
    title
    blogText
    postDate
    blogPostAuthor {
      authorID
      firstName
      lastName
      emailID
    }
  }
}
               */

        public BlogQuery(BlogsRepository blogsRepository)
        {
            /*Version: 1 get all*/
            //Field<ListGraphType<ReservationType>>("reservations",
            //    resolve: context => blogsRepository.GetQuery());

            /*Version: 2 filtering*/
            Field<ListGraphType<BlogType>>("blogPosts",
                arguments: new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<IdGraphType>
                    {
                        Name = "BlogPostID"
                    },
                    new QueryArgument<StringGraphType>
                    {
                        Name = "Title"
                    },
                     new QueryArgument<StringGraphType>
                    {
                        Name = "BlogText"
                    },
                    new QueryArgument<DateGraphType>
                    {
                        Name = "PostDate"
                    }
                }),
                resolve: context =>
                {
                    var query = blogsRepository.GetQuery();

                    var user = (ClaimsPrincipal)context.UserContext;
                    var isUserAuthenticated = ((ClaimsIdentity)user.Identity).IsAuthenticated;

                    var blogID = context.GetArgument<int?>("blogPostID");
                    if (blogID.HasValue)
                    {
                        if (blogID.Value <= 0)
                        {
                            context.Errors.Add(new ExecutionError("BlogID must be greater than zero!"));//Add messages to correct the error easily
                            return new List<BlogPost>();
                        }

                        return blogsRepository.GetQuery().Where(r => r.BlogPostID == blogID.Value);
                    }

                    var postDate = context.GetArgument<DateTime?>("postDate");
                    if (postDate.HasValue)
                    {
                        return blogsRepository.GetQuery()
                            .Where(r => r.PostDate.Date == postDate.Value.Date);
                    }

                    var postTitle = context.GetArgument<string>("title");
                    if (!postTitle.IsEmpty())
                    {
                        return blogsRepository.GetQuery()
                            .Where(r => r.Title.ToString() == postTitle);
                    }



                    return query.ToList();
                }
            );

        }
    }
}
