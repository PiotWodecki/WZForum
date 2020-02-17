using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Service
{
    public class ForumService : IForum
    {
        private readonly ApplicationDbContext _context;
        private ForumService(ApplicationDbContext context)
        {
            _context = context;
        }


        public Data.Models.Forum GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Data.Models.Forum> GetAll()
        {
            return _context.Forums
                .Include(forum => forum.Posts);
        }

        public IEnumerable<ApplicationUser> GetAllActiveUsers()
        {
            throw new NotImplementedException();
        }

        public Task Create(Data.Models.Forum forum)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int forumId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }
    }
}
