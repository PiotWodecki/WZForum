using System.Collections.Generic;
using System.Security.AccessControl;
using ForumWZ.Models.Post;

namespace ForumWZ.Models.Home
{
    public class HomeIndexModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<PostListingModel> LatestPosts { get; set; }
        
    }
}
