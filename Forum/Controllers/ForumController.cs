using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Data.Models;
using ForumWZ.Data;
using ForumWZ.Models.Forum;
using ForumWZ.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace ForumWZ.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;
        public ForumController(IForum forumService)
        {
            _forumService = forumService; //potrzebne do dependency injection
            //.NEt za każdym razem jak prosimy o coś co implementuje IForum interfejs to daj konkretną instację
            //ForumWZ.Service ale kontrolerowi potrzebne jest tylko jego zachowanie czyli to co robi
        }

        public IActionResult Index()
        {
            IEnumerable<ForumListingModel> forums = _forumService.GetAll().Select(forum=>new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description
            });
            var model = new ForumIndexModel
            {
                ForumList = forums
            };
            return View(model);
        }

        public IActionResult Topic(int id)
        {
            var forum = _forumService.GetById(id);
            //var posts = _postService.GetPostsByForum(id);
            var posts = forum.Posts;

            var postListings = posts.Select(post => new PostListingModel
            {
                Id = post.ID,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                AuthorName = post.User.UserName,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.PostReplies.Count(),//????
                Forum = BuildForumListing(post)
            });

            var model = new ForumTopicModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };
            return View(model);
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;

            return BuildForumListing(forum);
        }

        private ForumListingModel BuildForumListing(Forum.Data.Models.Forum forum)
        {

            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }
    }
}