﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum.Data;
using ForumWZ.Data;
using ForumWZ.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumWZ.Service
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Post GetById(int id)
        {
            return _context.Posts.Where(post => post.ID == id)
                .Include(post => post.User)
                .Include(post => post.PostReplies)
                    .ThenInclude(reply => reply.User)
                .Include(post => post.Forum)
                    .First();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts
                .Include(post => post.User)
                .Include(post => post.PostReplies)
                .ThenInclude(reply => reply.User)
                .Include(post => post.Forum);
        }

        public IEnumerable<Post> GetFilteredPosts(Data.Models.Forum forum, string searchQuery)
        {
            return string.IsNullOrEmpty(searchQuery)
                ? forum.Posts 
                : forum.Posts.Where(post => post.Title.Contains(searchQuery) 
                || post.Content.Contains(searchQuery)); 
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return _context.Forums
                .Where(forum => forum.Id == id).First()
                .Posts;
        }

        public IEnumerable<Post> GetLatestPosts(int n)
        {
            return GetAll().OrderByDescending(post => post.Created).Take(n);
        }

        public async Task Add(Post post)
        {
            _context.Add(post);

            await _context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditPostContent(int id, string newContent)
        {
            throw new NotImplementedException();
        }

        public async Task AddReply(PostReply reply)
        {
            _context.PostReplies.Add(reply);

            await _context.SaveChangesAsync();
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            var normalized = searchQuery.ToLower();

            return GetAll().Where
                (post => post.Title.ToLower().Contains(normalized) 
                || post.Content.ToLower().Contains(normalized));
        }
    }
}
