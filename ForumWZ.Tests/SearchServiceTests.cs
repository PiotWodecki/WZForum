using System;
using System.Linq;
using ForumWZ.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ForumWZ.Data.Models;
using ForumWZ.Service;

namespace ForumWZ.Tests
{
    [TestFixture]
    public class SearchServiceTests
    {
        [TestCase("coffee", 3)]
        [TestCase("teA", 1)]
        [TestCase("water", 0)]
        public void Return_Results_Corresponding_To_Query(string query, int expected)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Forums.Add(new Data.Models.Forum
                {
                    Id = 19
                });

                context.Posts.Add(new Post
                {
                    Forum = context.Forums.Find(19),
                    ID = 25323,
                    Title = "First Post",
                    Content = "Coffee"
                });

                context.Posts.Add(new Post
                {
                    Forum = context.Forums.Find(19),
                    ID = -25323,
                    Title = "Coffee",
                    Content = "SomeContent"
                });

                context.Posts.Add(new Post
                {
                    Forum = context.Forums.Find(19),
                    ID = 2332,
                    Title = "Tea",
                    Content = "Coffee"
                });

                context.SaveChanges();
            }


            //Act

            using (var context = new ApplicationDbContext(options))
            {
                var postService = new PostService(context);
                var result = postService.GetFilteredPosts(query);
                var postCount = result.ToList().Count;

                //Assert

                Assert.AreEqual(expected,postCount);
            }
        }
    }
}
