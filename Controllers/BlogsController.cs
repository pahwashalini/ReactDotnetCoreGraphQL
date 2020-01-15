using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.GraphQL.Client;
using Forum.Models;
using Forum.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly BlogsRepository _BlogsRepository;        
        private readonly BlogHttpGraphqlClient _httpGraphqlClient ;


        public BlogsController(BlogsRepository repo,  BlogHttpGraphqlClient blogHttpGraphqlClient)
        {
            _BlogsRepository = repo;          
            _httpGraphqlClient = blogHttpGraphqlClient;
        }
        [HttpGet("[action]")]
        public async Task<List<BlogPost>> List()
        {
            IEnumerable<BlogPost> blogs= await _BlogsRepository.GetAllBlogs();
            return blogs.ToList();
        }
        [HttpGet("[action]")]
        public async Task<List<BlogPost>> ListFromGraphql()
        {
            /*(Way:1) Native Http Client */
            return await GetViaHttpGraphqlClient();
        }
        private async Task<List<BlogPost>> GetViaHttpGraphqlClient()
        {
            var response = await _httpGraphqlClient.GetBlogsAsync();
            
            return response.Data.BlogPosts;
        }

      

    }
    }
