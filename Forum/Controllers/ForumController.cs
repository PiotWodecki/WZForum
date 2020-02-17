using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using ForumWZ.Models.Forum;
using Microsoft.AspNetCore.Mvc;

namespace ForumWZ.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
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
    }
}