using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumWZ.Data;
using ForumWZ.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumWZ.Service
{
    public class ForumService : IForum
    {
        private readonly ApplicationDbContext _context;
        public ForumService(ApplicationDbContext context)
        {
            _context = context;
        }


        public Data.Models.Forum GetById(int id)
        {
            var forum = _context.Forums.Where(x => x.Id == id)
                .Include(x => x.Posts)
                    .ThenInclude(p => p.User)
                .Include(x => x.Posts)
                    .ThenInclude(p => p.PostReplies)//??? w tutorial jest replies do sprawdzenia
                        .ThenInclude(r => r.User).FirstOrDefault();

            return forum;
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

        public async Task Create(Data.Models.Forum forum)
        {
            _context.Add(forum);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int forumId)
        {
            var forum = GetById(forumId);
            _context.Remove(forum);

            await _context.SaveChangesAsync();
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
