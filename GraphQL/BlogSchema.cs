using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.GraphQL
{
    public class BlogSchema : Schema
    {
        public BlogSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<BlogQuery>();
        }
    }
}
