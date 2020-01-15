using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQL.Common;
using Forum.Models;

namespace Forum.GraphQL.Client
{
    public class BlogHttpGraphqlClient
    {
        private readonly HttpClient _httpClient;

        public BlogHttpGraphqlClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<BlogContainer>> GetBlogsAsync()
        {
            var response = await _httpClient.GetAsync(
                @"?query={ blogPosts {blogPostID, title,blogText,postDate,blogPostAuthor {authorID, firstName,lastName , emailID}  }}");

            var stringResult = await response.Content.ReadAsStringAsync();
            var data= JsonConvert.DeserializeObject<Response<BlogContainer>>(stringResult);
            return data;
        }

    }
}
