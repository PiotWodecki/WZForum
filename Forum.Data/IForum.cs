using ForumWZ.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ForumWZ.Data
{
    public interface IForum
    {
        Models.Forum GetById(int id);
        IEnumerable<Models.Forum> GetAll();
        IEnumerable<ApplicationUser> GetAllActiveUsers(int id);

        Task Create(Models.Forum forum);
        Task Delete(int forumId);
        Task UpdateForumTitle(int forumId, string newTitle);
        Task UpdateForumDescription(int forumId, string newDescription);
        bool HasRecentPosts(int forumId);
    }
}
