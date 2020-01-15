# ReactDotnetCoreGraphQL
React client app with GraphQL and .net Core Web API
Steps
1.	Create .net core react app
2.	Add new folder “Models”
3.	Add your Blog Objects here
```
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
```

4.	Add DBContext to your project
5.	Do Entity Framework Migrations by running following commands on package-manager console

```
add-migration "InitialCreate"
update-database
```

6.	Add Repository Folder and add BlogRepository.cs

```
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
```
7.	Check your API if its fetching data
```
[HttpGet("[action]")]
        public async Task<List<BlogPost>> List()
        {
            IEnumerable<BlogPost> blogs= await _BlogsRepository.GetAllBlogs();
            return blogs.ToList();
        }
```
8.	Add GraphQL references 
```
GraphQL.Server.Ui.Playground (where you can run your GraphQL queries)
GraphQL.Server.Transports.AspNetCore (Middleware for ASP.NET Core)
```
9.	Add GraphQL types for all the defined Entities
```
public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType()
        {
            Field(x => x.AuthorID);
            Field(x => x.FirstName);
            Field(x => x.LastName);
            Field(x => x.LastLogin);
            Field(x => x.UserPassword);
            Field(x => x.IsActive);
            Field(x => x.DateJoined);
            Field(x => x.EmailID);
        }
    }

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
```
10.	Create Schema
```
public class BlogSchema : Schema
    {
        public BlogSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<BlogQuery>();
        }
    }
```
11.	In StartUp.cs, Add following code in ConfigureServices method
```
            services.AddScoped<IDependencyResolver>(x => new FuncDependencyResolver(x.GetRequiredService));

            services.AddScoped<BlogSchema>();

            services.AddGraphQL(x =>
            {
                x.ExposeExceptions = true; //set true only in dev mode.
            })
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddUserContextBuilder(httpContext => httpContext.User)
                .AddDataLoader();
```
12.	To add GraphQL Playground to run your queries. Add following  code to Configure method of StartUp.cs
```
  app.UseGraphQL<BlogSchema>();
  app.UseGraphQLPlayground(new GraphQLPlaygroundOptions()); 
```
 
13.	To add filtering facility to your GraphqQLQuery you need to specify the arguments as filtering criteria. You need to specify their type and how it would be evaluated
```
public class BlogQuery : ObjectGraphType
{
public BlogQuery(BlogsRepository blogsRepository)
{
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
context.Errors.Add(new ExecutionError("BlogID must be greater than zero!"));
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
});

}
}
```
