using Forum.Models;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.GraphQL.Types
{
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
}
