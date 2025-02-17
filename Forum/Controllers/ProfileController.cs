﻿using System.IO;
using System.Linq;
using ForumWZ.Data;
using ForumWZ.Data.Models;
using ForumWZ.Models.ApplicationUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

namespace ForumWZ.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;
        public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload uploadService, IConfiguration configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _configuration = configuration;
        }
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var model = new ProfileModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemberSince = user.MemberSince,
                IsAdmin = userRoles.Contains("Admin"),
                UserRating = user.Rating.ToString()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);
        //    //Connect to an Azure Storage Account Contianer
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
        //    //Get blob container
            var container = _uploadService.GetBlobContainer(connectionString, "profile-images");
        //    //Parse the content disposition response header
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
        //    //grab the filename
            var filename = contentDisposition.FileName.Trim().ToString();
        //    //get a reference to a block blob
            var blockBlob = container.GetBlockBlobReference(filename);
        //    //on that block blob, upload our file <- file uploaded to the cloud
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());

        //    //Set the user's profile image to the uri 
            await _userService.SetProfileImage(userId, blockBlob.Uri);
        //    //redirect to the users profile page
            return RedirectToAction("Detail", "Profile", new {id = userId});
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var profiles = _userService.GetAll()
                .OrderByDescending(user => user.Rating)
                .Select(u => new ProfileModel
                {
                    Email = u.Email,
                    UserName = u.UserName,
                    ProfileImageUrl = u.ProfileImageUrl,
                    UserRating = u.Rating.ToString(),
                    MemberSince = u.MemberSince
                });

            var model = new ProfileListModel
            {
                Profiles = profiles
            };

            return View(model);
        }

        //private string UrlHelper(string id)
        //{

        //}
    }
}